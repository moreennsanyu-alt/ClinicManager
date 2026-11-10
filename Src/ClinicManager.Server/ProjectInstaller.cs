using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ClinicManager.Server
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        string _displayName = "Clinic Manager Server";
        public ProjectInstaller()
        {
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            var serviceInstaller = new ServiceInstaller
            {
                ServiceName = CMService.Name,
                DisplayName = _displayName,
                Description = _displayName,
                StartType = ServiceStartMode.Automatic
            };

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
