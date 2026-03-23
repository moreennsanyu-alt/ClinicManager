using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using ClinicManager.Win.Views;

namespace ClinicManager.Win;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class ClinicManagerApp : PrismApplication
    {
        static App()
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
    }
