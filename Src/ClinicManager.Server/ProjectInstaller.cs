using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ClinicManager.Server
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            var serviceInstaller = new ServiceInstaller
            {
                ServiceName = CMService.Name,
                DisplayName = "Clinic Manager Server",
                Description = DisplayName,
                StartType = ServiceStartMode.Automatic
            };

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
