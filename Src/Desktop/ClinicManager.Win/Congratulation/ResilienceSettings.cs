namespace ClinicManager.Win.Configuration;

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
    public int CircuitBreakerTimeoutSeconds { 
get; set; } = 30;
}
