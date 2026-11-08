using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

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
                ServiceBase.Run(service);
            }
        }
    }
}
