# Logging Implementation Summary

## What Was Implemented

Comprehensive file logging has been configured for the FSH.Starter.WebApi.Host application to ensure all errors and diagnostic information are captured when running in IIS on Windows Server.

## Changes Made

### 1. Enhanced StaticLogger (`Infrastructure/Logging/Serilog/StaticLogger.cs`)
- ✅ Added file logging to startup logger
- ✅ Logs written to `logs/startup-YYYYMMDD.log`
- ✅ Ensures early startup errors are captured
- ✅ Added XML documentation
- ✅ Configured with 1-second flush interval for immediate error visibility

### 2. Improved Program.cs (`api/server/Program.cs`)
- ✅ Enhanced error handling with detailed logging
- ✅ Logs environment and directory information on startup
- ✅ Captures full exception details including inner exceptions
- ✅ Added interactive pause feature (waits for keypress on error when running in console)
- ✅ Ensures logs are flushed before application exit

### 3. Updated appsettings.json
- ✅ Added Serilog.Sinks.File configuration
- ✅ Configured daily log rotation
- ✅ Set 10 MB file size limit with rollover
- ✅ 30-day retention policy
- ✅ Added custom output template with timestamps
- ✅ Configured appropriate log levels for different components

### 4. Updated appsettings.Production.json
- ✅ Optimized log levels for production (Information level)
- ✅ Same file logging configuration as development
- ✅ Enhanced override settings for noisy components
- ✅ Added enrichment with machine name and thread ID

### 5. Helper Scripts Created

#### Setup-IISLogPermissions.ps1
- ✅ Automated script to configure log directory permissions
- ✅ Creates logs directory if it doesn't exist
- ✅ Grants full control to IIS App Pool identity
- ✅ Validates and displays current permissions
- ✅ Includes error handling and user-friendly output

#### View-Logs.ps1
- ✅ View log files with syntax highlighting
- ✅ Follow logs in real-time (-Follow parameter)
- ✅ View specific log types (app, startup, stdout)
- ✅ List all available log files
- ✅ Color-coded output based on log levels (ERROR=Red, WARNING=Yellow, etc.)

#### Test-WebApiHost.bat
- ✅ Test runner that pauses on error
- ✅ Captures and displays exit codes
- ✅ Guides user to check log files
- ✅ Perfect for testing before IIS deployment

### 6. Documentation Created

#### LOGGING_QUICK_START.md
- ✅ Quick reference guide for getting started
- ✅ Step-by-step setup instructions
- ✅ Common troubleshooting scenarios
- ✅ Helper script usage examples

#### LOGGING_IIS_SETUP.md
- ✅ Comprehensive logging documentation
- ✅ Log file descriptions and locations
- ✅ Permission configuration details
- ✅ Monitoring best practices
- ✅ Troubleshooting guide

#### IIS_DEPLOYMENT_LOGGING_CHECKLIST.md
- ✅ Complete deployment checklist
- ✅ Pre-deployment verification steps
- ✅ Post-deployment validation
- ✅ Maintenance procedures
- ✅ Quick reference commands

## Log Files Generated

### Location
All logs are written to: `[ApplicationRoot]/logs/`

Example: `C:\inetpub\wwwroot\FSH.Starter.WebApi\logs\`

### Files

1. **app-YYYYMMDD.log**
   - Main application logs
   - All runtime events, errors, and information
   - Configured via appsettings.json Serilog section

2. **startup-YYYYMMDD.log**
   - Early initialization logs
   - Captures errors before full configuration loads
   - Critical for diagnosing startup failures

3. **stdout-YYYYMMDD.log**
   - IIS ASP.NET Core Module output
   - Configured via web.config
   - Contains IIS-level diagnostics

## Key Features

### ✅ Automatic Log Rotation
- Daily rotation (new file each day)
- Size-based rotation (10 MB limit)
- Automatic cleanup (30-day retention)

### ✅ Error Capture
- All exceptions logged with full stack traces
- Inner exceptions logged separately
- Environment context included

### ✅ Interactive Error Display
- Console mode pauses on error
- Shows formatted error message
- Directs user to log files

### ✅ IIS Compatibility
- Shared log file access for multi-process scenarios
- Fast flush interval (1 second) for immediate visibility
- Proper permissions configuration

### ✅ Structured Logging
- Timestamp with timezone
- Log level indicators
- Contextual information (environment, directory, etc.)
- Machine name and thread ID enrichment

## Usage Examples

### Setting Up for First Time
```powershell
# Run as Administrator in application directory
.\Setup-IISLogPermissions.ps1 -AppPath "C:\inetpub\wwwroot\YourApp" -AppPoolName "YourAppPool"
```

### Viewing Logs
```powershell
# View latest logs
.\View-Logs.ps1

# Follow in real-time
.\View-Logs.ps1 -Follow

# View startup logs
.\View-Logs.ps1 -LogType startup
```

### Testing Before Deployment
```batch
Test-WebApiHost.bat
```

### Manual Testing
```powershell
dotnet FSH.Starter.WebApi.Host.dll
# Application will pause on error, showing details before exit
```

## Troubleshooting

### Logs Not Created
**Cause**: Missing permissions
**Solution**: Run `Setup-IISLogPermissions.ps1`

### Application Won't Start in IIS
**Check in order**:
1. `logs/startup-YYYYMMDD.log`
2. `logs/stdout-YYYYMMDD.log`
3. Windows Event Viewer > Application
4. Run `Test-WebApiHost.bat` manually

### Can't See Real-Time Logs
**Solution**: Use `.\View-Logs.ps1 -Follow`

## Benefits

1. **Complete Error Visibility**: No more mystery errors when running in IIS
2. **Easy Troubleshooting**: Clear, detailed log files with timestamps
3. **Automated Management**: Logs rotate and clean up automatically
4. **Developer Friendly**: Interactive mode pauses on error
5. **Production Ready**: Proper log levels and retention for production use
6. **IIS Optimized**: Shared access, fast flushing, proper permissions

## Testing Checklist

Before deployment, verify:
- [ ] Application starts successfully
- [ ] Log files are created in `logs` directory
- [ ] Startup messages appear in logs
- [ ] Errors are captured and logged
- [ ] Interactive pause works in console mode
- [ ] IIS App Pool identity has write permissions to logs directory

## Next Steps

1. Deploy application to IIS
2. Run `Setup-IISLogPermissions.ps1` on the server
3. Start the application
4. Verify logs are being created
5. Set up monitoring/alerts for ERROR and FATAL logs
6. Configure log archival if needed for compliance

## Support Files Reference

| File | Purpose | Location |
|------|---------|----------|
| StaticLogger.cs | Early logging | Infrastructure/Logging/Serilog/ |
| Program.cs | Main entry point | api/server/ |
| appsettings.json | Dev config | api/server/ |
| appsettings.Production.json | Prod config | api/server/ |
| web.config | IIS config | api/server/ |
| Setup-IISLogPermissions.ps1 | Permission setup | api/server/ |
| View-Logs.ps1 | Log viewer | api/server/ |
| Test-WebApiHost.bat | Manual tester | api/server/ |
| LOGGING_QUICK_START.md | Quick guide | api/server/ |
| LOGGING_IIS_SETUP.md | Full docs | api/server/ |
| IIS_DEPLOYMENT_LOGGING_CHECKLIST.md | Checklist | root/ |

---

**Implementation Date**: October 27, 2025
**Status**: ✅ Complete and Ready for Deployment

