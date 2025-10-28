# Logging Configuration for IIS Deployment

## Overview
The FSH.Starter.WebApi.Host is configured with comprehensive file logging for IIS deployments. This ensures all application events, errors, and diagnostics are captured to log files.

## Log File Locations

When deployed to IIS, the application creates log files in the `logs` directory within the application root:

### 1. Application Logs
- **Location**: `logs/app-YYYYMMDD.log`
- **Content**: All application logs including info, warnings, and errors
- **Rolling**: Daily (new file each day)
- **Retention**: 30 days
- **Size Limit**: 10 MB per file (rolls over when exceeded)

### 2. Startup Logs
- **Location**: `logs/startup-YYYYMMDD.log`
- **Content**: Early initialization logs before full Serilog configuration loads
- **Rolling**: Daily
- **Retention**: 7 days
- **Purpose**: Captures startup errors that occur before the app is fully configured

### 3. IIS ASP.NET Core Module Logs
- **Location**: `logs/stdout-YYYYMMDD.log`
- **Content**: stdout/stderr from the ASP.NET Core module
- **Configuration**: Defined in `web.config`
- **Purpose**: IIS-level diagnostics and hosting errors

## Log Levels by Environment

### Development (appsettings.json)
- Default: Debug
- Microsoft: Warning
- EntityFrameworkCore: Warning

### Production (appsettings.Production.json)
- Default: Information
- Microsoft: Warning
- System: Warning
- EntityFrameworkCore: Warning
- Hangfire: Warning

## Log File Permissions

When deploying to IIS, ensure the IIS Application Pool identity has write permissions to the `logs` directory:

```powershell
# Grant permissions to the logs directory
icacls "C:\inetpub\wwwroot\YourApp\logs" /grant "IIS AppPool\YourAppPoolName:(OI)(CI)F" /T
```

Or for the default application pool:
```powershell
icacls "C:\inetpub\wwwroot\YourApp\logs" /grant "IIS AppPool\DefaultAppPool:(OI)(CI)F" /T
```

## Troubleshooting

### Logs Not Being Created
1. **Check Directory Permissions**: Ensure IIS App Pool has write access to the logs directory
2. **Check Directory Exists**: The app will create the logs directory, but ensure parent directory is writable
3. **Check web.config**: Verify `stdoutLogEnabled="true"` is set
4. **Event Viewer**: Check Windows Event Viewer > Application logs for IIS/ASP.NET Core errors

### Finding Startup Errors
When the application fails to start in IIS:
1. Check `logs/startup-YYYYMMDD.log` first
2. Check `logs/stdout-YYYYMMDD.log` for IIS module errors
3. Check Windows Event Viewer

### Log Files Growing Too Large
Adjust retention and size limits in `appsettings.json` or `appsettings.Production.json`:

```json
"Serilog": {
  "WriteTo": [
    {
      "Name": "File",
      "Args": {
        "retainedFileCountLimit": 30,  // Reduce this number
        "fileSizeLimitBytes": 10485760  // Reduce file size (10MB default)
      }
    }
  ]
}
```

## Log Output Format

Logs are formatted with the following template:
```
{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message}{NewLine}{Exception}
```

Example:
```
2025-10-27 14:30:15.123 +08:00 [INF] Server booting up...
2025-10-27 14:30:15.456 +08:00 [INF] Environment: Production
2025-10-27 14:30:16.789 +08:00 [ERR] Database connection failed
System.InvalidOperationException: Cannot open database
   at ...
```

## Monitoring Best Practices

1. **Set up log rotation monitoring**: Ensure old logs are cleaned up automatically
2. **Monitor disk space**: Large applications can generate significant log volume
3. **Use log analysis tools**: Consider tools like Seq, Splunk, or ELK stack for log aggregation
4. **Configure alerts**: Set up alerts for ERROR and FATAL level logs

## Interactive Console Mode

When running the application interactively (e.g., from command prompt), the application will:
- Display errors in the console with color coding
- Pause and wait for key press before exit (so you can read the error)
- Still write all logs to files

## Configuration in appsettings.json

The logging configuration is controlled via the `Serilog` section:

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Information"
    },
    "WriteTo": [
      { "Name": "Console" },
      { 
        "Name": "File",
        "Args": {
          "path": "logs/app-.log",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

## Additional Resources

- [Serilog Documentation](https://serilog.net/)
- [ASP.NET Core IIS Troubleshooting](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/troubleshoot)
- [Enable stdout logging in web.config](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/aspnet-core-module)

