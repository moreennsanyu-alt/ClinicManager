using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using ClinicManager.Win.Views;
using ClinicManager.Win.Application.Lifetime;

namespace ClinicManager.Win;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class ClinicManagerApp : PrismApplication
    {
        static ClinicManagerApp()
        {
            SfSkinManager.ApplyThemeAsDefaultStyle = true;
            SfSkinManager.ApplicationTheme = new Theme("Windows11Light");
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

            var first = ApplicationActivator.LaunchOrReturn(otherInstance => { MessageBox.Show("got data"); }, e.Args);
            if (!first)
            {
                Shutdown();
            }


            base.OnStartup(e);
        }
    }
