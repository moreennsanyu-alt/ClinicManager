namespace ClinicManager.Win.Configuration;

/// <summary>
/// Application configuration settings with defaults and overridable values.
/// Can be populated from appsettings.json, environment variables, or command line arguments.
/// </summary>
public class ApplicationSettings
{
    /// <summary>
    /// HTTP client configurations
    /// </summary>
    public HttpClientsSettings HttpClients { get; set; } = new();

    /// <summary>
    /// Logging configuration
    /// </summary>
    public LoggingSettings Logging { get; set; } = new();

    /// <summary>
    /// Resilience policy settings (retries, timeouts, circuit breaker)
    /// </summary>
    public ResilienceSettings Resilience { get; set; } = new();
}
