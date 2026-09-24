using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;

namespace ClinicManager.Win.Features.Infrastructure.Shell;

public class ShellViewModel : BindableBase
{
    private readonly IApiClient _api;
    private readonly IRequestLogService _log;

    private string _title = "Prism + DryIoc + Microsoft.Extensions.DependencyInjection";
    private string _relativeUrl = "objects";
    private string _response = string.Empty;
    private bool _isBusy;

    // IApiClient is a typed HttpClient registered through Microsoft DI (AddHttpClient<,>);
    // IRequestLogService is registered through Prism. Both are constructor-injected here.
    public ShellViewModel(IApiClient api, IRequestLogService log)
    {
        _api = api;
        _log = log;

        SendCommand = new DelegateCommand(async () => await SendAsync(), () => !IsBusy)
            .ObservesProperty(() => IsBusy);
        ClearLogCommand = new DelegateCommand(_log.Clear);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string RelativeUrl
    {
        get => _relativeUrl;
        set => SetProperty(ref _relativeUrl, value);
    }

    public string Response
    {
        get => _response;
        private set => SetProperty(ref _response, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set => SetProperty(ref _isBusy, value);
    }

    public ObservableCollection<RequestLogEntry> LogEntries => _log.Entries;

    public DelegateCommand SendCommand { get; }
    public DelegateCommand ClearLogCommand { get; }

    private async Task SendAsync()
    {
        IsBusy = true;
        try
        {
            Response = await _api.GetStringAsync(RelativeUrl);
        }
        catch (Exception ex)
        {
            Response = $"Request failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
