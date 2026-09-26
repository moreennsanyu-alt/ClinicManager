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
    public class MainViewModel : INotifyPropertyChanged
    {
        private object? _currentViewModel;
        private string _statusMessage = string.Empty;

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            private set => SetField(ref _currentViewModel, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public ICommand NavigateHomeCommand { get; }

        public MainViewModel()
        {
            NavigateHomeCommand = new RelayCommand(_ => NavigateHome());

            NavigateHome();
        }

        private void NavigateHome()
        {
            CurrentViewModel = new HomeViewModel();
        }

        /// <summary>
        /// Swaps the hosted view model for any feature view model.
        /// </summary>
        public void NavigateTo(object viewModel)
        {
            CurrentViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Minimal ICommand implementation local to this view model.
        /// </summary>
        private sealed class RelayCommand : ICommand
        {
            private readonly Action<object?> _execute;
            private readonly Predicate<object?>? _canExecute;

            public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public event EventHandler? CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

            public void Execute(object? parameter) => _execute(parameter);
        }
    }
}
