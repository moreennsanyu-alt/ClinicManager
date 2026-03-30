using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using ClinicManager.Win.Views;
using ClinicManager.Win.Application.Lifetime;
using SingleInstanceCore;

namespace ClinicManager.Win;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class ClinicManagerApp : PrismApplication, ISingleInstance
    {
        static ClinicManagerApp()
        {
        }
        
        public void OnInstanceInvoked(string[] args)
        {
		    // What to do with the args another instance has sent
	    }
        
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
        
        public void Activate()
        {
            // Reactivate application's main window
            MainWindow.Activate();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            bool isFirstInstance = this.InitializeAsFirstInstance("soheilkd_ExampleIPC");
		    if (!isFirstInstance)
		    {
			    //If it's not the first instance, arguments are automatically passed to the first instance
			    //OnInstanceInvoked will be raised on the first instance
			    //You may shut down the current instance
			    Current.Shutdown();
		    }

            base.OnStartup(e);
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            SingleInstance.Cleanup();
            base.OnExit(e);
        }
    }
