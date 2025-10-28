# Logging Quick Start Guide for IIS

## Overview
The FSH.Starter.WebApi.Host now has comprehensive file logging configured for IIS deployments. All errors and diagnostic information will be written to log files.

## Log Files Location
All logs are written to the `logs` directory in your application root:
```
C:\inetpub\wwwroot\YourApp\logs\
```

## Log Files Created

| Log File | Purpose | When Used |
|----------|---------|-----------|
| `app-YYYYMMDD.log` | Main application logs | Always |
| `startup-YYYYMMDD.log` | Early startup logs | Before full config loads |
| `stdout-YYYYMMDD.log` | IIS module logs | When IIS captures stdout |

## Quick Setup for IIS

### 1. Set Log Permissions (Required!)
```powershell
# Run as Administrator
cd C:\inetpub\wwwroot\YourApp
.\Setup-IISLogPermissions.ps1 -AppPath "C:\inetpub\wwwroot\YourApp" -AppPoolName "YourAppPool"
```

### 2. Verify web.config
Ensure this line exists in `web.config`:
```xml
stdoutLogEnabled="true" stdoutLogFile=".\logs\stdout"
```

### 3. Start Application
Start your IIS app pool and application.

### 4. Check Logs
```powershell
# View latest logs
.\View-Logs.ps1

# Follow logs in real-time
.\View-Logs.ps1 -Follow

# View specific log type
.\View-Logs.ps1 -LogType app
.\View-Logs.ps1 -LogType startup
.\View-Logs.ps1 -LogType stdout

# List all log files
.\View-Logs.ps1 -LogType all
```

## Testing Before IIS Deployment

Run the application manually to test logging:
```batch
Test-WebApiHost.bat
```

Or with PowerShell:
```powershell
dotnet FSH.Starter.WebApi.Host.dll
```

The application will pause on errors and show the error message before exiting.

## Troubleshooting Startup Errors

### Application Won't Start in IIS?
1. Check `logs/startup-YYYYMMDD.log` first
2. Check `logs/stdout-YYYYMMDD.log` for IIS errors  
3. Check Windows Event Viewer > Application logs
4. Verify log directory permissions
5. Test manually: Run `Test-WebApiHost.bat`

### No Log Files Created?
1. Check directory permissions (most common issue)
2. Ensure logs directory exists
3. Verify IIS App Pool identity has write access
4. Check Windows Event Viewer for access denied errors

### Common Permission Fix
```powershell
# Grant full control to App Pool (run as admin)
icacls "C:\inetpub\wwwroot\YourApp\logs" /grant "IIS AppPool\YourAppPool:(OI)(CI)F" /T
```

## What Gets Logged?

### Startup
- Environment information
- Current directory
- Configuration loading
- Middleware initialization
- Server startup status

### Runtime
- HTTP requests (configurable level)
- Application errors
- Database queries (in development)
- Authentication events
- Custom application logs

### Shutdown
- Graceful shutdown messages
- Final flush of log buffer

## Configuration

### Development (appsettings.json)
- Log Level: Debug
- Retention: 30 days
- File size: 10 MB per file

### Production (appsettings.Production.json)  
- Log Level: Information
- Retention: 30 days
- File size: 10 MB per file

## Best Practices

1. **Always set permissions** before first run
2. **Check logs after deployment** to verify everything works
3. **Monitor log disk space** - set up cleanup if needed
4. **Review ERROR logs daily** in production
5. **Use -Follow mode** when actively troubleshooting

## Helper Scripts Included

| Script | Purpose |
|--------|---------|
| `Setup-IISLogPermissions.ps1` | Configure log directory permissions |
| `View-Logs.ps1` | View and monitor log files |
| `Test-WebApiHost.bat` | Test app manually with pause on error |

## Example: Complete First-Time Setup

```powershell
# 1. Navigate to application directory
cd C:\inetpub\wwwroot\FSH.Starter.WebApi

# 2. Set permissions (as Administrator)
.\Setup-IISLogPermissions.ps1 -AppPath "C:\inetpub\wwwroot\FSH.Starter.WebApi" -AppPoolName "MyAppPool"

# 3. Start IIS App Pool
Restart-WebAppPool -Name "MyAppPool"

# 4. Test the application
Test-WebApiHost.bat

# 5. View logs
.\View-Logs.ps1 -LogType latest

# 6. If issues, follow logs in real-time
.\View-Logs.ps1 -Follow
```

## Support

For detailed documentation, see:
- `LOGGING_IIS_SETUP.md` - Complete logging documentation
- `IIS_DEPLOYMENT_LOGGING_CHECKLIST.md` - Full deployment checklist

## Log Retention

Logs are automatically rotated:
- **Daily rotation**: New file each day
- **Size limit**: 10 MB per file (creates new file if exceeded)
- **Retention**: 30 days (older files auto-deleted)

To change retention settings, edit `appsettings.json` or `appsettings.Production.json`:
```json
"Args": {
  "retainedFileCountLimit": 30
}
```

## Emergency Log Cleanup

If logs grow too large:
```powershell
# Delete logs older than 7 days
Get-ChildItem ".\logs\*.log" | 
  Where-Object { $_.LastWriteTime -lt (Get-Date).AddDays(-7) } | 
  Remove-Item -Force
```

