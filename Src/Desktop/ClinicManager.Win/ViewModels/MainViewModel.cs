using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicManager.Win.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
         [ObservableProperty]
         bool _NavigationPaneVisibility;
         
         [RelayCommand]
         public async Task OnLoaded()
         {
              await Task.CompletedTask; 
         }
     
    }
}
