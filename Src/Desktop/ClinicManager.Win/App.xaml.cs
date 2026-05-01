using Prism.Ioc;
using Prism.DryIoc;
using ClinicManager.Win.Views;
using System.Windows;

namespace ClinicManager.Win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : CMApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
