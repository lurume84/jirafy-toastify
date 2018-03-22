using System.ServiceProcess;

namespace ToastifyService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] servicesToRun = {
                new ToastifyService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}