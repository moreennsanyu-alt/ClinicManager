namespace ClinicManager.Win.Configuration;


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
