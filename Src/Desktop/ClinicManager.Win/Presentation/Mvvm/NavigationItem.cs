namespace ClinicManager.Win.Presentation.Mvvm;

public enum BadgeType
{
    None,
    Info,
    Warning,
    Error,
    Success
}

public class NavigationItem : BindableBase
{
    public string Key { get; set; }
    public string Title { get; set; }
    public string ViewName { get; set; }
    public string Path { get; set; }
    public IDictionary<string, object> Parameters { get; set; }
    public int Priority { get; set; } = 0;
    public ObservableCollection<NavigationItem> Children { get; } = new();

    private string _badgeText;
    public string BadgeText { get => _badgeText; set => SetProperty(ref _badgeText, value); }

    private BadgeType _badgeType = BadgeType.None;
    public BadgeType BadgeType { get => _badgeType; set => SetProperty(ref _badgeType, value); }

    private bool _isEnabled = true;
    public bool IsEnabled { get => _isEnabled; set => SetProperty(ref _isEnabled, value); }

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }
}
