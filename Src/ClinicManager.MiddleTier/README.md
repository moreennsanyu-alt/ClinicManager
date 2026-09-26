# ClinicManager.MiddleTier

## Overview

**ClinicManager.MiddleTier** is a robust Windows Service providing middleware functionality for the ClinicManager system. It handles centralized processing, monitoring, health checks, and request management.

### Key Features

- **Windows Service Host**: Runs as a Windows service for continuous availability
- **Component-Based Architecture**: Modular, extensible design with pluggable components
- **Health Monitoring**: Built-in performance tracking (CPU, memory, uptime)
- **Automatic Logging**: File-based logging with configurable retention
- **Graceful Shutdown**: Properly stops all components on shutdown
- **Event Log Integration**: Windows Event Viewer integration for monitoring
- **.NET Framework 4.8**: Built on stable .NET Framework 4.8 runtime

## Project Structure

```
ClinicManager.MiddleTier/
├── ClinicManager.MiddleTier.csproj    # Project file (.NET Framework 4.8)
├── Program.cs                          # Service entry point with debug support
├── ClinicManagerMiddleTierService.cs   # Windows Service implementation
├── ServiceManager.cs                   # Core service orchestration
├── ServiceComponents.cs                # Built-in service components
├── ServiceInterfaces.cs                # Interface definitions
├── ProjectInstaller.cs                 # Service installer
├── App.config                          # Configuration file
└── README.md                           # This file
```

## Architecture

### Service Layers

```
┌─────────────────────────────────────────────────────┐
│  Windows Service Host                               │
│  └─ ClinicManagerMiddleTierService                 │
│     └─ ServiceManager                              │
│        ├─ HealthMonitorComponent                   │
│        │  └─ CPU/Memory/Uptime Tracking            │
│        ├─ ConfigurationComponent                   │
│        │  └─ App Settings Management               │
│        └─ LoggerComponent                          │
│           └─ File-Based Logging                    │
└─────────────────────────────────────────────────────┘
```

## Installation

### Prerequisites

1. **Operating System**: Windows Server or Windows Desktop with .NET Framework 4.8 installed
2. **Permissions**: Administrative privileges for service installation
3. **Network**: Appropriate firewall rules (if service requires network access)

### Install the Service

#### Option 1: Using InstallUtil.exe (Recommended)

```powershell
# Open Command Prompt as Administrator
# Navigate to .NET Framework directory
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319

# Install the service
InstallUtil.exe "C:\path\to\ClinicManager.MiddleTier.exe"
```

#### Option 2: Using sc.exe

```powershell
# Open Command Prompt as Administrator
sc create ClinicManagerMiddleTier binPath= "C:\path\to\ClinicManager.MiddleTier.exe" start= auto displayName= "ClinicManager Middle Tier Service"
```

### Start the Service

```powershell
# Using net command
net start ClinicManagerMiddleTier

# Or using sc command
sc start ClinicManagerMiddleTier

# Or through Services GUI
# 1. Press Win+R
# 2. Type 'services.msc'
# 3. Find "ClinicManager Middle Tier Service"
# 4. Right-click and select "Start"
```

### Stop the Service

```powershell
# Using net command
net stop ClinicManagerMiddleTier

# Or using sc command
sc stop ClinicManagerMiddleTier
```

### Uninstall the Service

```powershell
# Using InstallUtil.exe
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
InstallUtil.exe /u "C:\path\to\ClinicManager.MiddleTier.exe"

# Or using sc.exe
sc delete ClinicManagerMiddleTier
```

## Configuration

### App.config

Edit `App.config` to configure the service:

```xml
<appSettings>
  <add key="Environment" value="Production" />
  <add key="EnableDetailedLogging" value="false" />
  <add key="LogRetentionDays" value="30" />
</appSettings>
```

## Running as Console Application (Debug Mode)

To debug the service, temporarily run it as a console application:

1. Build in Debug configuration
2. Run the executable directly (not as a service)
3. The service will output to the console
4. Press Enter to stop

Example output:
```
=== ClinicManager Middle Tier Service Started (DEBUG MODE) ===
Press Enter to stop the service...
```

## Monitoring and Logging

### Log Files

Logs are stored in the `Logs` directory relative to the service executable:

```
ClinicManager.MiddleTier.exe
Logs/
  └── service-2024-01-15.log
```

Log file format:
```
[2024-01-15 10:30:45] ClinicManager Middle Tier Service is starting...
[2024-01-15 10:30:45] HealthMonitorComponent started.
[2024-01-15 10:30:45] ConfigurationComponent started.
[2024-01-15 10:30:45] LoggerComponent started.
[2024-01-15 10:30:45] ClinicManager Middle Tier Service started successfully.
```

### Event Viewer

View service logs in Windows Event Viewer:

1. Open Event Viewer: Press `Win+R`, type `eventvwr.msc`
2. Navigate to: `Windows Logs > Application`
3. Look for entries with Source: "ClinicManagerMiddleTier"

### Health Monitoring

The service performs periodic health checks every 60 seconds:

- **CPU Usage**: Percentage of processor time
- **Memory**: Available RAM in MB
- **Uptime**: Service run duration
- **Request Count**: Number of processed requests (if applicable)

Health status is logged and can be monitored via performance counters.

## Extending the Service

### Adding Custom Components

To add new functionality, create a component implementing `IServiceComponent`:

```csharp
public class MyCustomComponent : IServiceComponent
{
    public void Initialize()
    {
        // Initialize resources
    }

    public void Start()
    {
        // Start processing
    }

    public void Stop()
    {
        // Clean up resources
    }
}
```

Register in `ServiceManager.InitializeComponents()`:

```csharp
private void InitializeComponents()
{
    _components.Add(new HealthMonitorComponent());
    _components.Add(new ConfigurationComponent());
    _components.Add(new LoggerComponent());
    _components.Add(new MyCustomComponent());  // Add your component
}
```

### Health Check Implementation

For components supporting health monitoring, implement `IHealthCheckable`:

```csharp
public class MyCustomComponent : IServiceComponent, IHealthCheckable
{
    public HealthStatus GetHealth()
    {
        if (IsHealthy)
            return HealthStatus.Healthy("All systems operational");
        else
            return HealthStatus.Unhealthy("Critical error detected");
    }
}
```

## Troubleshooting

### Service won't start

1. Check Event Viewer for error messages:
   - `Event Viewer > Windows Logs > Application`
   - Look for Source: "ClinicManagerMiddleTier"

2. Check log files in the `Logs` directory

3. Run in Debug mode to see console output:
   ```powershell
   ClinicManager.MiddleTier.exe
   ```

4. Verify permissions:
   - Local System account has necessary file/registry access
   - Consider using a dedicated service account

### High CPU Usage

1. Check health monitor logs for CPU percentage
2. Review log files for error patterns
3. Verify no infinite loops in custom components
4. Check database connectivity if applicable

### Service stops unexpectedly

1. Check Event Log for crash information
2. Review log files for stack traces
3. Verify Windows hasn't scheduled automatic updates/restarts
4. Check service account permissions and expiration

## Performance Considerations

### Resource Usage

- **Base Memory**: ~50-80 MB
- **CPU**: Minimal when idle (health check every 60 seconds)
- **Disk**: Log rotation every 30 days (configurable)

### Optimization Tips

1. Set appropriate log retention period in App.config
2. Disable detailed logging in production
3. Monitor performance counters to detect resource issues early
4. Use `LocalService` account instead of `LocalSystem` if possible
5. Configure automatic service restart on failure via Services properties

## Security Considerations

### Service Account

- Default: **Local System** (full system privileges)
- For production, consider using a dedicated service account with minimal required permissions
- Ensure the service account has appropriate file/registry access

### Windows Firewall

- Add inbound rules if the service listens on network ports
- Use Windows Firewall with Advanced Security for granular control

### Logging

- Store logs on a secure, backed-up partition
- Implement log rotation to prevent disk space issues
- Protect sensitive information in logs

## Automatic Restart on Failure

Configure via Services GUI or sc.exe:

```powershell
# Configure service to restart on failure
sc failure ClinicManagerMiddleTier reset= 86400 actions= restart/5000
```

This will:
- Restart the service after 5 seconds if it fails
- Reset the failure count after 24 hours (86400 seconds)

## Dependencies

### .NET Framework

- .NET Framework 4.8
- System.ServiceProcess
- System.Diagnostics
- System.Configuration

## License

See the main ClinicManager repository for licensing information.

## Support

For issues related to:

- **Windows Services**: Refer to [Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/)
- **ClinicManager**: See main repository documentation
- **Service Issues**: Check Event Viewer logs and service log files
