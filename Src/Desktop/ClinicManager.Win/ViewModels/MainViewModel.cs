using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicManager.Win.ViewModels
{
    public partial class MainViewModel(INavigationTreeBuilder navigationTreeBuilder) : ViewModelBase
    {
         [ObservableProperty]
         bool _NavigationPaneVisibility;

         [ObservableProperty]
         RootNavigationItem _selectedRootNavigationItem;
         
         [ObservableProperty]
         NavigationItem _selectedNavigationItem;

         
         [RelayCommand]
         public async Task OnLoaded()
         {
              await Task.CompletedTask; 
         }
          
    public ObservableCollection<NavigationItem> NavigationItems { get; } = navigationTreeBuilder.Root;

    partial void OnSelectedRootNavigationItemChanging(RootNavigationItem oldItem,RootNavigationItem newItem)
    {
        // runs before the field is set; value is the NEW value
    }

    partial void OnSelectedRootNavigationItemChanged(RootNavigationItem oldItem,RootNavigationItem newItem)
    {
        if(newItem.SelectedItem == null)
           SelectedNavigationItem = newItem.Items.First();
        SelectedNavigationItem = newItem.SelectedItem;
    }

    partial void OnSelectedNavigationItemChanged(NavigationItem oldItem,NavigationItem newItem)
    {
        if(SelectedRootNavigationItem.SelectedItem != newItem)
           SelectedRootNavigationItem.SelectedItem = newItem;
        Navigate(newItem);
    }

    public void Navigate(NavigationItem item)
    {

    
    }
    partial void OnSelectedNavigationItemChanging(NavigationItem oldItem,NavigationItem newItem)
    {
        // ...
    }
    }
}
