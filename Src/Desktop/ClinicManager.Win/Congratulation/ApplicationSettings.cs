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

public class HttpClientsSettings
{
    /// <summary>
    /// Primary API client configuration
    /// </summary>
    public HttpClientConfig PrimaryApi { get; set; } = new();

    /// <summary>
    /// Secondary/auxiliary API client configuration
    /// </summary>
    public HttpClientConfig SecondaryApi { get; set; } = new();
}

public class HttpClientConfig
{
    /// <summary>
    /// Base address for the HTTP client (e.g., https://api.example.com)
    /// </summary>
    public string BaseAddress { get; set; } = "https://localhost:5001";

    /// <summary>
    /// Request timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Maximum number of retry attempts for transient failures
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Initial delay in seconds before first retry (exponential backoff)
    /// </summary>
    public double InitialDelaySeconds { get; set; } = 1.0;
}

public class LoggingSettings
{
    /// <summary>
    /// Minimum log level (Verbose, Debug, Information, Warning, Error, Fatal)
    /// </summary>
    public string MinimumLevel { get; set; } = "Information";

    /// <summary>
    /// Log file path
    /// </summary>
    public string FilePath { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ClinicManager",
        "Logs",
        "clinicmanager-.txt");

    /// <summary>
    /// Rolling interval for log files (Day, Hour, Month, etc.)
    /// </summary>
    public string RollingInterval { get; set; } = "Day";

    /// <summary>
    /// Number of log files to retain before deletion
    /// </summary>
    public int RetainedFileCountLimit { get; set; } = 30;

    /// <summary>
    /// Enable console output for logs
    /// </summary>
    public bool EnableConsole { get; set; } = true;
}

public class ResilienceSettings
{
    /// <summary>
    /// Number of retry attempts for transient failures
    /// </summary>
    public int RetryAttempts { get; set; } = 3;

    /// <summary>
    /// Initial delay in seconds for exponential backoff (first retry)
    /// </summary>
    public double InitialDelaySeconds { get; set; } = 1.0;

    /// <summary>
    /// Maximum delay in seconds for exponential backoff
    /// </summary>
    public double MaxDelaySeconds { get; set; } = 30.0;

    /// <summary>
    /// Number of failures allowed before circuit breaks
    /// </summary>
    public int CircuitBreakerThreshold { get; set; } = 5;

    /// <summary>
    /// Duration in seconds to keep circuit open before attempting recovery
    /// </summary>
    public int CircuitBreakerTimeoutSeconds { get; set; } = 30;
}
