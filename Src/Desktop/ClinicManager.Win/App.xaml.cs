using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using ClinicManager.Win.Views;

namespace ClinicManager.Win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
