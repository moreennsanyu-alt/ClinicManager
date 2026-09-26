using System;
using System.Configuration;
using System.ServiceProcess;
using System.Diagnostics;
using System.Collections.Generic;

namespace ClinicManager.MiddleTier
{
    /// <summary>
    /// ClinicManager Middle Tier Windows Service
    /// 
    /// A robust Windows Service implementation providing middleware functionality
    /// for ClinicManager applications with comprehensive logging and monitoring.
    /// </summary>
    public partial class ClinicManagerMiddleTierService : ServiceBase
    {
        private EventLog? _eventLog;
        private ServiceManager? _serviceManager;
        private const string ServiceName = "ClinicManagerMiddleTier";
        private const string DisplayName = "ClinicManager Middle Tier Service";
        private const string EventLogSource = "ClinicManagerMiddleTier";

        public ClinicManagerMiddleTierService()
        {
            ServiceName = ServiceName;
            DisplayName = DisplayName;
            CanStop = true;
            CanPauseAndContinue = false;
            AutoLog = true;
            CanShutdown = true;

            // Initialize Event Log
            if (!EventLog.SourceExists(EventLogSource))
            {
                try
                {
                    EventLog.CreateEventSource(EventLogSource, "Application");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to create Event Log source: {ex.Message}");
                }
            }

            _eventLog = new EventLog()
            {
                Source = EventLogSource,
                Log = "Application"
            };
        }

        /// <summary>
        /// Start the Windows Service and initialize the service manager
        /// </summary>
        protected override void OnStart(string[] args)
        {
            try
            {
                LogMessage($"{DisplayName} is starting...", EventLogEntryType.Information);

                // Initialize the service manager
                _serviceManager = new ServiceManager();
                _serviceManager.Start();

                LogMessage($"{DisplayName} started successfully on {DateTime.Now}.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                LogMessage(
                    $"{DisplayName} failed to start: {ex.Message}{Environment.NewLine}StackTrace: {ex.StackTrace}",
                    EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Stop the Windows Service and dispose of the service manager
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                LogMessage($"{DisplayName} is stopping...", EventLogEntryType.Information);

                if (_serviceManager != null)
                {
                    _serviceManager.Stop();
                    _serviceManager.Dispose();
                    _serviceManager = null;
                }

                LogMessage($"{DisplayName} stopped successfully on {DateTime.Now}.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                LogMessage(
                    $"{DisplayName} error during stop: {ex.Message}{Environment.NewLine}StackTrace: {ex.StackTrace}",
                    EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Handle system shutdown
        /// </summary>
        protected override void OnShutdown()
        {
            try
            {
                LogMessage($"{DisplayName} received system shutdown signal.", EventLogEntryType.Information);
                OnStop();
            }
            catch (Exception ex)
            {
                LogMessage(
                    $"{DisplayName} error during shutdown: {ex.Message}",
                    EventLogEntryType.Error);
            }
            base.OnShutdown();
        }

        /// <summary>
        /// Helper method to log messages to the Event Log and console
        /// </summary>
        private void LogMessage(string message, EventLogEntryType entryType)
        {
            try
            {
                _eventLog?.WriteEntry(message, entryType);
            }
            catch
            {
                // If event log fails, try console output (useful for debugging)
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{entryType}] {message}");
            }
        }
    }
}
