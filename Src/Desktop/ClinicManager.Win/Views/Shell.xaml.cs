using System.Windows;
using Syncfusion.Windows.Shared;

namespace ClinicManager.Win.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : ChromelessWindow
    {
        public Shell()
        {
            InitializeComponent();
            //view discovery
            //regionManager.RegisterViewWithRegion("ContentRegion", typeof(ViewA));
        }
    }
}
