using System.Windows;
using ClinicManager.Win.ViewModels;

namespace ClinicManager.Win.Views;

public partial class ShellWindow : ThemedWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
