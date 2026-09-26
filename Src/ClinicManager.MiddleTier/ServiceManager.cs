using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ClinicManager.MiddleTier
{
    /// <summary>
    /// Service Manager - Orchestrates all middleware operations
    /// Provides robust error handling, health monitoring, and graceful shutdown
    /// </summary>
    public class ServiceManager : IDisposable
    {
        private readonly List<IServiceComponent> _components;
        private Timer? _healthCheckTimer;
        private bool _isRunning;
        private bool _disposed;
        private readonly object _lockObject = new object();
        private const int HealthCheckInterval = 60000; // 60 seconds

        public ServiceManager()
        {
            _components = new List<IServiceComponent>();
            _isRunning = false;
            _disposed = false;

            // Initialize service components
            InitializeComponents();
        }

        /// <summary>
        /// Initialize all service components
        /// </summary>
        private void InitializeComponents()
        {
            try
            {
                // Add any middleware components here
                _components.Add(new HealthMonitorComponent());
                _components.Add(new ConfigurationComponent());
                _components.Add(new LoggerComponent());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing components: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Start all service components
        /// </summary>
        public void Start()
        {
            lock (_lockObject)
            {
                if (_isRunning)
                    throw new InvalidOperationException("Service is already running.");

                try
                {
                    foreach (var component in _components)
                    {
                        component.Initialize();
                        component.Start();
                        Debug.WriteLine($"Component started: {component.GetType().Name}");
                    }

                    // Start health check timer
                    _healthCheckTimer = new Timer(PerformHealthCheck, null, HealthCheckInterval, HealthCheckInterval);
                    _isRunning = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error starting service: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Stop all service components
        /// </summary>
        public void Stop()
        {
            lock (_lockObject)
            {
                if (!_isRunning)
                    return;

                try
                {
                    // Stop health check timer
                    _healthCheckTimer?.Dispose();
                    _healthCheckTimer = null;

                    // Stop all components in reverse order
                    for (int i = _components.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            _components[i].Stop();
                            Debug.WriteLine($"Component stopped: {_components[i].GetType().Name}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error stopping component {_components[i].GetType().Name}: {ex.Message}");
                        }
                    }

                    _isRunning = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error stopping service: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Perform periodic health check on all components
        /// </summary>
        private void PerformHealthCheck(object? state)
        {
            lock (_lockObject)
            {
                if (!_isRunning)
                    return;

                try
                {
                    foreach (var component in _components)
                    {
                        if (component is IHealthCheckable healthCheckable)
                        {
                            var health = healthCheckable.GetHealth();
                            if (health.Status != HealthStatus.Healthy)
                            {
                                Debug.WriteLine($"Warning: Component {component.GetType().Name} is {health.Status}. {health.Details}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during health check: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Get service status
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            try
            {
                Stop();
                _healthCheckTimer?.Dispose();
                _components.Clear();
                _disposed = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during dispose: {ex.Message}");
            }
        }
    }
}
