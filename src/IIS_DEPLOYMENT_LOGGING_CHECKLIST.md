# IIS Deployment Checklist with Logging Setup

## Pre-Deployment Checklist

### 1. Build and Publish
- [ ] Build the solution in Release mode
- [ ] Run `dotnet publish -c Release` or use the publish scripts
- [ ] Verify all necessary files are in the publish directory

### 2. Server Prerequisites
- [ ] .NET 8.0 (or appropriate) Runtime installed on Windows Server
- [ ] IIS installed with ASP.NET Core Hosting Bundle
- [ ] SQL Server or PostgreSQL accessible from the server
- [ ] Required ports open (typically 80, 443)

## Deployment Steps

### 3. Copy Files to IIS
- [ ] Copy published files to IIS directory (e.g., `C:\inetpub\wwwroot\FSH.Starter.WebApi`)
- [ ] Verify `FSH.Starter.WebApi.Host.dll` is present
- [ ] Verify `web.config` is present

### 4. Configure Logging Directory Permissions

#### Option A: Using PowerShell Script (Recommended)
```powershell
# Run as Administrator
.\Setup-IISLogPermissions.ps1 -AppPath "C:\inetpub\wwwroot\FSH.Starter.WebApi" -AppPoolName "YourAppPoolName"
```

#### Option B: Manual Configuration
```powershell
# Create logs directory if it doesn't exist
New-Item -Path "C:\inetpub\wwwroot\FSH.Starter.WebApi\logs" -ItemType Directory -Force

# Grant permissions (replace YourAppPoolName with actual app pool name)
icacls "C:\inetpub\wwwroot\FSH.Starter.WebApi\logs" /grant "IIS AppPool\YourAppPoolName:(OI)(CI)F" /T
```

- [ ] Logs directory created
- [ ] Permissions granted to IIS App Pool identity
- [ ] Verify permissions with: `icacls "C:\inetpub\wwwroot\FSH.Starter.WebApi\logs"`

### 5. Configure IIS Application Pool
- [ ] Create new Application Pool or use existing
- [ ] Set .NET CLR Version to "No Managed Code"
- [ ] Set Managed Pipeline Mode to "Integrated"
- [ ] Configure Identity (ApplicationPoolIdentity recommended)
- [ ] Set Start Mode to "AlwaysRunning" (optional, for better startup)
- [ ] Set Idle Time-out to appropriate value (0 for always running)

### 6. Create IIS Website/Application
- [ ] Create new website or application in IIS Manager
- [ ] Set Physical Path to publish directory
- [ ] Assign Application Pool created in step 5
- [ ] Configure bindings (HTTP/HTTPS)
- [ ] Set appropriate authentication (typically Anonymous)

### 7. Verify web.config Settings
Check that `web.config` contains:
```xml
<aspNetCore processPath="dotnet" 
            arguments=".\FSH.Starter.WebApi.Host.dll" 
            stdoutLogEnabled="true" 
            stdoutLogFile=".\logs\stdout" 
            hostingModel="inprocess">
```
- [ ] stdoutLogEnabled is "true"
- [ ] stdoutLogFile points to ".\logs\stdout"
- [ ] processPath and arguments are correct

### 8. Configure appsettings.Production.json
- [ ] Database connection string configured
- [ ] CORS origins updated for production domains
- [ ] JWT settings configured with strong keys
- [ ] Mail settings configured
- [ ] Serilog file logging configured (should already be set)

### 9. Start Application and Verify Logs

#### Start the Application
- [ ] Start IIS Application Pool
- [ ] Browse to application URL
- [ ] Check for successful startup

#### Verify Log Files Created
Navigate to the logs directory and verify these files are created:
- [ ] `logs/app-YYYYMMDD.log` - Application logs
- [ ] `logs/startup-YYYYMMDD.log` - Startup logs  
- [ ] `logs/stdout-YYYYMMDD.log` - IIS module logs (if any output)

#### Check Log Contents
- [ ] Open latest `app-*.log` file
- [ ] Verify "Server booting up..." message
- [ ] Verify "Application configured successfully" message
- [ ] Check for any ERROR or FATAL entries

### 10. Troubleshooting Failed Startup

If the application fails to start:

1. **Check logs in order:**
   - [ ] `logs/startup-YYYYMMDD.log` - First place to check
   - [ ] `logs/stdout-YYYYMMDD.log` - IIS-level errors
   - [ ] Windows Event Viewer > Application logs
   - [ ] Windows Event Viewer > System logs

2. **Common Issues:**
   - [ ] Missing .NET runtime
   - [ ] Incorrect file permissions
   - [ ] Database connection issues
   - [ ] Missing configuration values
   - [ ] Port conflicts

3. **Test Manually:**
   ```powershell
   cd C:\inetpub\wwwroot\FSH.Starter.WebApi
   dotnet FSH.Starter.WebApi.Host.dll
   ```
   Or use the provided batch file:
   ```batch
   Test-WebApiHost.bat
   ```

### 11. Configure SSL/TLS (Production)
- [ ] Install SSL certificate
- [ ] Configure HTTPS binding in IIS
- [ ] Update CORS settings for HTTPS URLs
- [ ] Test HTTPS access

### 12. Performance and Security
- [ ] Configure response compression
- [ ] Enable HTTP/2 if supported
- [ ] Configure request limits in web.config
- [ ] Set up health check endpoint monitoring
- [ ] Configure firewall rules
- [ ] Review security headers in web.config

### 13. Monitoring Setup
- [ ] Set up log rotation/cleanup job
- [ ] Configure disk space monitoring for logs directory
- [ ] Set up application performance monitoring
- [ ] Configure email alerts for critical errors
- [ ] Test error notification system

### 14. Backup and Recovery
- [ ] Document application configuration
- [ ] Back up appsettings files
- [ ] Document database connection details
- [ ] Create rollback plan

## Post-Deployment Verification

### Verify Logging is Working
1. [ ] Browse to a non-existent URL to generate 404 error
2. [ ] Check `logs/app-*.log` for the 404 entry
3. [ ] Restart Application Pool
4. [ ] Verify new startup entry in logs
5. [ ] Check log file timestamps are current

### Verify Application Functionality  
- [ ] Test user authentication
- [ ] Test API endpoints
- [ ] Test database connectivity
- [ ] Test email functionality
- [ ] Review application logs for errors

### Performance Check
- [ ] Monitor CPU usage
- [ ] Monitor memory usage
- [ ] Check response times
- [ ] Verify no memory leaks (monitor over time)

## Maintenance

### Daily
- [ ] Check latest log files for errors
- [ ] Monitor disk space in logs directory

### Weekly
- [ ] Review log files for patterns or recurring issues
- [ ] Check application pool recycling events
- [ ] Verify old logs are being cleaned up

### Monthly
- [ ] Review and archive old logs if needed
- [ ] Check for application updates
- [ ] Review security patches
- [ ] Performance analysis

## Quick Reference Commands

### Restart Application Pool
```powershell
Restart-WebAppPool -Name "YourAppPoolName"
```

### View Latest Logs
```powershell
Get-Content "C:\inetpub\wwwroot\FSH.Starter.WebApi\logs\app-*.log" -Tail 50
```

### Check Disk Space
```powershell
Get-ChildItem "C:\inetpub\wwwroot\FSH.Starter.WebApi\logs" -Recurse | Measure-Object -Property Length -Sum
```

### Clean Old Logs (older than 30 days)
```powershell
Get-ChildItem "C:\inetpub\wwwroot\FSH.Starter.WebApi\logs\*.log" | 
    Where-Object { $_.LastWriteTime -lt (Get-Date).AddDays(-30) } | 
    Remove-Item -Force
```

## Support Resources
- See `LOGGING_IIS_SETUP.md` for detailed logging documentation
- Check Windows Event Viewer for system-level issues
- Review ASP.NET Core logs in logs directory
- Consult application documentation for specific features

