using System.Windows;

namespace ClinicManager.Win.Features.Infrastructure.Shell
{
    /// <summary>
    /// Interaction logic for MainView.xaml. Acts as the application shell window
    /// that hosts the currently active feature view.
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
