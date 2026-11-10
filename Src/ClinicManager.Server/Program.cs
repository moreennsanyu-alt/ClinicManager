using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Configuration;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp;
using System.Collections;
using DevExpress.ExpressApp.Xpo;
using System.ServiceModel;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.MiddleTier;

namespace ClinicManager.Server
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLowerInvariant())
                {
                    case "/install":
                    case "-install":
                        ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
                        return;
                    case "/uninstall":
                    case "-uninstall":
                        ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
                        return;
                }
            }

            var service = new CMService();

            if (Environment.UserInteractive)
            {
                // Console mode
                Console.WriteLine($"{CMService.Name} running in console mode. Press Enter to stop.");
                service.StartConsole(args);
                Console.ReadLine();
                service.StopConsole();
                Console.WriteLine("Stopped.");
            }
            else
            {
                System.ServiceProcess.ServiceBase.Run(service);
            }
        }
    }
}
