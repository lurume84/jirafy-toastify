using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Management;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using ToastifyAPI;

namespace ToastifyService
{
    public partial class ToastifyService : ServiceBase
    {
        public static readonly int MmfMaxSize = 1024;
        public static readonly int MmfViewSize = 1024;

        private readonly MemoryMappedFile mmf;

        private const string watchQuery = @"SELECT * FROM Win32_ProcessStartTrace " +
                                          @"WHERE ProcessName = ""Spotify.exe""";

        private ManagementEventWatcher spotifyWatcher;

        private Process spotifyProcess;

        internal ToastifyService()
        {
            this.InitializeComponent();

            // Create event source
            this.eventLog = new EventLog();
            if (!EventLog.SourceExists("ToastifyService"))
                EventLog.CreateEventSource("ToastifyService", "ToastifyServiceLog");
            this.eventLog.Source = "ToastifyService";
            this.eventLog.Log = "ToastifyServiceLog";

            // Create the memory-mapped file that's going to be used to perform IPC
            this.mmf = MemoryMappedFile.CreateNew("toastify-ipc.mmf", MmfMaxSize, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, HandleInheritability.Inheritable);
        }

        protected override void OnStart(string[] args)
        {
            this.spotifyProcess = Spotify.FindSpotifyProcess();

            if (this.spotifyProcess != null)
                this.spotifyProcess.Exited += this.SpotifyProcess_Exited;
            else
                this.WaitForSpotify();
        }

        protected override void OnStop()
        {
            this.mmf.Dispose();
            this.DisposeSpotifyWatcher();
        }

        #region Spotify watcher

        public void WaitForSpotify()
        {
            this.spotifyWatcher = new ManagementEventWatcher(new WqlEventQuery(watchQuery));
            this.spotifyWatcher.EventArrived -= this.SpotifyWatcher_EventArrived;
            this.spotifyWatcher.EventArrived += this.SpotifyWatcher_EventArrived;
            this.spotifyWatcher.Start();
        }

        private async void SpotifyWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            uint pid = (uint)e.NewEvent.Properties["ProcessID"].Value;
            var process = Process.GetProcessById(unchecked((int)pid));

            try
            {
                process.WaitForInputIdle();
                await Task.Run(() =>
                {
                    while (this.spotifyProcess == null)
                    {
                        if (Spotify.IsMainSpotifyProcess(pid))
                        {
                            this.eventLog.WriteEntry($"Spotify launched; PID = {pid}", EventLogEntryType.Information);
                            this.spotifyProcess = process;
                            this.spotifyProcess.Exited += this.SpotifyProcess_Exited;

                            this.NotifyToastify(new ToastifyServiceMessage { @event = ToastifyServiceEvent.SpotifyStarted, param = pid });

                            this.spotifyWatcher.EventArrived -= this.SpotifyWatcher_EventArrived;
                            this.spotifyWatcher.Stop();
                        }
                        else
                            Thread.Sleep(500);
                    }
                });
            }
            catch (InvalidOperationException) { /* The process does not have a message loop */ }
        }

        #endregion Spotify watcher

        private void NotifyToastify(ToastifyServiceMessage message)
        {
            Process toastify = GetToastifyProcess();
            if (toastify == null)
                return;

            MemoryMappedViewStream mmvStream = this.mmf.CreateViewStream(0, MmfViewSize);

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(mmvStream, message);
            mmvStream.Seek(0, SeekOrigin.Begin);
        }

        private static Process GetToastifyProcess()
        {
            const string toastifyGUID = "{B8F3CA50-CE27-4ffa-A812-BBE1435C9485}";
            using (Mutex unused = new Mutex(true, toastifyGUID, out bool exclusive))
            {
                return exclusive ? null : Process.GetProcessesByName("Toastify.exe").Single();
            }
        }

        private void DisposeSpotifyWatcher()
        {
            try
            {
                if (this.spotifyWatcher != null)
                {
                    this.spotifyWatcher.EventArrived -= this.SpotifyWatcher_EventArrived;
                    this.spotifyWatcher.Stop();

                    this.spotifyWatcher.Dispose();
                    this.spotifyWatcher = null;
                }
            }
            catch { /* ignore */ }
        }

        private void SpotifyProcess_Exited(object sender, EventArgs e)
        {
            try
            {
                this.NotifyToastify(new ToastifyServiceMessage { @event = ToastifyServiceEvent.SpotifyTerminated });
                this.spotifyProcess = null;
                this.WaitForSpotify();
            }
            catch { /* ignore */ }
        }
    }
}