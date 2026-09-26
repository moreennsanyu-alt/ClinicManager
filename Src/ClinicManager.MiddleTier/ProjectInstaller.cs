using System.Collections.Configuration.Install;
using System.ServiceProcess;
using System.Diagnostics;

namespace ClinicManager.MiddleTier
{
    /// <summary>
    /// Service installer for the ClinicManager Middle Tier Windows Service.
    /// 
    /// This class handles the installation and uninstallation of the service
    /// when using InstallUtil.exe or custom installation scripts.
    /// 
    /// Usage from Command Prompt (as Administrator):
    /// 
    /// Install:
    ///   C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe ClinicManager.MiddleTier.exe
    /// 
    /// Uninstall:
    ///   C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u ClinicManager.MiddleTier.exe
    /// 
    /// Or using sc.exe:
    ///   sc create ClinicManagerMiddleTier binPath= "C:\path\to\ClinicManager.MiddleTier.exe"
    ///   sc delete ClinicManagerMiddleTier
    /// </summary>
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller? _serviceProcessInstaller;
        private ServiceInstaller? _serviceInstaller;

        public ProjectInstaller()
        {
            // Service runs under Local System account
            // Alternative options: LocalService, NetworkService, or specify a custom account
            _serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            // Configure the service installer
            _serviceInstaller = new ServiceInstaller
            {
                ServiceName = "ClinicManagerMiddleTier",
                DisplayName = "ClinicManager Middle Tier Service",
                Description = "Robust middleware service for ClinicManager. " +
                              "Provides centralized processing, monitoring, and request handling.",
                StartType = ServiceStartMode.Automatic
            };

            Installers.Add(_serviceProcessInstaller);
            Installers.Add(_serviceInstaller);
        }

        /// <summary>
        /// Called after the service is installed
        /// </summary>
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            
            // Create event log source if it doesn't exist
            string sourceName = "ClinicManagerMiddleTier";
            if (!EventLog.SourceExists(sourceName))
            {
                try
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Note: Could not create event log source (may require admin): {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Called when the service is uninstalled
        /// </summary>
        public override void Uninstall(IDictionary stateSaver)
        {
            base.Uninstall(stateSaver);
            
            // Optionally delete the event log source
            string sourceName = "ClinicManagerMiddleTier";
            if (EventLog.SourceExists(sourceName))
            {
                try
                {
                    EventLog.DeleteEventSource(sourceName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Note: Could not delete event log source: {ex.Message}");
                }
            }
        }
    }
}
