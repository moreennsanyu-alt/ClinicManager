using System;
using System.Diagnostics;

namespace ClinicManager.MiddleTier
{
    /// <summary>
    /// Interface for service components
    /// </summary>
    public interface IServiceComponent
    {
        void Initialize();
        void Start();
        void Stop();
    }

    /// <summary>
    /// Interface for components that support health checking
    /// </summary>
    public interface IHealthCheckable
    {
        HealthStatus GetHealth();
    }

    /// <summary>
    /// Health status information
    /// </summary>
    public class HealthStatus
    {
        public enum Status
        {
            Healthy,
            Degraded,
            Unhealthy
        }

        public Status StatusValue { get; set; }
        public string Details { get; set; }

        public static HealthStatus Healthy(string? details = null) => new HealthStatus 
        { 
            StatusValue = Status.Healthy, 
            Details = details ?? "Component is healthy" 
        };

        public static HealthStatus Degraded(string details) => new HealthStatus 
        { 
            StatusValue = Status.Degraded, 
            Details = details 
        };

        public static HealthStatus Unhealthy(string details) => new HealthStatus 
        { 
            StatusValue = Status.Unhealthy, 
            Details = details 
        };
    }
}
