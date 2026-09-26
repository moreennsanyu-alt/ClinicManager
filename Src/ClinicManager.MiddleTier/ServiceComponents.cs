using System;
using System.Diagnostics;
using System.Configuration;

namespace ClinicManager.MiddleTier
{
    /// <summary>
    /// Health Monitor Component - Tracks system and service health
    /// </summary>
    public class HealthMonitorComponent : IServiceComponent, IHealthCheckable
    {
        private PerformanceCounter? _cpuCounter;
        private PerformanceCounter? _memoryCounter;
        private DateTime _startTime;
        private int _requestCount;

        public void Initialize()
        {
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
                _memoryCounter = new PerformanceCounter("Memory", "Available MBytes", null, true);
                _startTime = DateTime.Now;
                _requestCount = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing HealthMonitorComponent: {ex.Message}");
            }
        }

        public void Start()
        {
            Debug.WriteLine("HealthMonitorComponent started.");
        }

        public void Stop()
        {
            _cpuCounter?.Dispose();
            _memoryCounter?.Dispose();
            Debug.WriteLine("HealthMonitorComponent stopped.");
        }

        public HealthStatus GetHealth()
        {
            try
            {
                float cpu = _cpuCounter?.NextValue() ?? 0;
                float memory = _memoryCounter?.NextValue() ?? 0;
                TimeSpan uptime = DateTime.Now - _startTime;

                if (cpu > 90 || memory < 100)
                {
                    return HealthStatus.Degraded($"CPU: {cpu:F1}%, Memory Available: {memory:F0} MB");
                }

                return HealthStatus.Healthy($"CPU: {cpu:F1}%, Memory Available: {memory:F0} MB, Uptime: {uptime.TotalHours:F1}h");
            }
            catch (Exception ex)
            {
                return HealthStatus.Unhealthy($"Error reading performance counters: {ex.Message}");
            }
        }

        public void IncrementRequestCount() => _requestCount++;
        public int GetRequestCount() => _requestCount;
    }

    /// <summary>
    /// Configuration Component - Manages service configuration
    /// </summary>
    public class ConfigurationComponent : IServiceComponent
    {
        private Dictionary<string, string> _config = new();

        public void Initialize()
        {
            try
            {
                // Load configuration from App.config
                foreach (string key in ConfigurationManager.AppSettings)
                {
                    _config[key] = ConfigurationManager.AppSettings[key] ?? string.Empty;
                }
                Debug.WriteLine($"ConfigurationComponent initialized with {_config.Count} settings.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing ConfigurationComponent: {ex.Message}");
            }
        }

        public void Start()
        {
            Debug.WriteLine("ConfigurationComponent started.");
        }

        public void Stop()
        {
            Debug.WriteLine("ConfigurationComponent stopped.");
        }

        public string? GetSetting(string key)
        {
            _config.TryGetValue(key, out var value);
            return value;
        }
    }

    /// <summary>
    /// Logger Component - Handles logging operations
    /// </summary>
    public class LoggerComponent : IServiceComponent
    {
        private string? _logFilePath;

        public void Initialize()
        {
            try
            {
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                _logFilePath = Path.Combine(logDirectory, $"service-{DateTime.Now:yyyy-MM-dd}.log");
                Debug.WriteLine($"LoggerComponent initialized. Log file: {_logFilePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing LoggerComponent: {ex.Message}");
            }
        }

        public void Start()
        {
            WriteLog("LoggerComponent started.");
        }

        public void Stop()
        {
            WriteLog("LoggerComponent stopped.");
        }

        public void WriteLog(string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(_logFilePath))
                {
                    File.AppendAllText(_logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing to log: {ex.Message}");
            }
        }
    }
}
