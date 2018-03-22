using System.ServiceProcess;

namespace ToastifyService
{
    public partial class ToastifyService : ServiceBase
    {
        public ToastifyService()
        {
            this.InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}