using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicManager.Win.Features.Infrastructure.Shell
{
    /// <summary>
    /// View model for the default landing/dashboard view shown after login.
    /// </summary>
    public class HomeViewModel : INotifyPropertyChanged
    {
        private string _welcomeMessage = "Welcome to Clinic Manager";

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetField(ref _welcomeMessage, value);
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
    }
}
