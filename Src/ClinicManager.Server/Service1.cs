using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClinicManager.Server
{
    public partial class Service1 : ServiceBase
    {
        
        public Service1()
        {
            InitializeComponent();
            this.ServiceName = "WindowsService.NET";
        }

        protected override void OnStart(string[] args)
        {
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
           // WriteLog(String.Format("{0} ms elapsed.", Interval));
        }

        protected override void OnStop()
        {
          //Timer.Stop();
          //  WriteLog("Service has been stopped.");
        }

        
    }
}
