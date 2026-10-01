namespace ClinicManager.Win.Configuration;

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
