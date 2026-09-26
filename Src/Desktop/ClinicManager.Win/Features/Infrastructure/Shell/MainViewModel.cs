using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ClinicManager.Win.Features.Infrastructure.Shell
{
    /// <summary>
    /// Top-level shell view model responsible for hosting and switching
    /// between the currently displayed feature view model.
    /// </summary>
    public class MainViewModel
    {
        public ObservableCollection<NavigationItem> NavigationItems { get; }
        
    
        public MainViewModel(INavigationTreeBuilder _treebuilder)
        {
            NavigationItems = _treebuilder.Root;
        }

    }
}
