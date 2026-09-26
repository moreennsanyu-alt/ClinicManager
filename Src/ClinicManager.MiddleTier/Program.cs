using System;
using System.ServiceProcess;

namespace ClinicManager.MiddleTier
{
    /// <summary>
    /// Main entry point for the ClinicManager.MiddleTier Windows Service.
    /// This service provides middleware functionality for ClinicManager applications.
    /// 
    /// For debugging: Set the conditional compilation symbol DEBUG and the service will run as a console application.
    /// For production: Run as a Windows Service via ServiceBase.Run().
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            // Debug mode: Run as console application
            var service = new ClinicManagerMiddleTierService();
            service.OnStart(args);
            Console.WriteLine("=== ClinicManager Middle Tier Service Started (DEBUG MODE) ===");
            Console.WriteLine("Press Enter to stop the service...");
            Console.ReadLine();
            service.OnStop();
#else
            // Release mode: Run as Windows Service
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ClinicManagerMiddleTierService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
