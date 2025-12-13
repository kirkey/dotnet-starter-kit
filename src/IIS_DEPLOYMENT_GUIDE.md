# IIS Deployment Guide for FSH Starter Kit

## Overview

This guide explains how to deploy the FSH Starter Kit API and Blazor Client to Windows IIS servers.

## Prerequisites

- Windows Server with IIS 8.5 or higher installed
- .NET 9 ASP.NET Core Hosting Bundle (or Windows Hosting Bundle) installed on the server
  - Download: https://dotnet.microsoft.com/download/dotnet
- Local machine with .NET 9 SDK for publishing (macOS/Linux/Windows)

## Publishing for Deployment

### Option 1: From macOS/Linux

```bash
cd /path/to/dotnet-starter-kit/src
chmod +x publish-for-iis.sh
./publish-for-iis.sh
```

### Option 2: From Windows

```cmd
cd C:\path\to\dotnet-starter-kit\src
publish-for-iis.bat
```

This will generate:
- `deploy/FSH.Starter.API_[timestamp].zip` - API deployment package
- `deploy/FSH.Starter.Blazor_[timestamp].zip` - Blazor Client deployment package

## API Deployment to IIS

### 1. Prepare the Server

```powershell
# On your IIS server, create application directory
New-Item -ItemType Directory -Path "C:\inetpub\fsh-api" -Force
New-Item -ItemType Directory -Path "C:\inetpub\fsh-blazor" -Force
```

### 2. Extract and Copy Files

1. Extract `FSH.Starter.API_[timestamp].zip` to `C:\inetpub\fsh-api`
2. Extract `FSH.Starter.Blazor_[timestamp].zip` to `C:\inetpub\fsh-blazor`

### 3. Configure IIS for API

#### Create Application Pool

```powershell
# PowerShell as Administrator
$AppPoolName = "FSH-API-Pool"
New-WebAppPool -Name $AppPoolName
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "processModel.identityType" -Value "ApplicationPoolIdentity"
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "managedPipelineMode" -Value "Integrated"
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "managedRuntimeVersion" -Value ""  # For .NET Core
```

#### Create Website/Application

```powershell
$SiteName = "FSH-API"
$PhysicalPath = "C:\inetpub\fsh-api"
$AppPoolName = "FSH-API-Pool"

# Create website
New-Website -Name $SiteName -PhysicalPath $PhysicalPath -ApplicationPool $AppPoolName -Port 5000
```

### 4. Configure appsettings.json

Edit `C:\inetpub\fsh-api\appsettings.json`:

```json
{
  "DatabaseOptions": {
    "ConnectionString": "Server=YOUR_DB_SERVER;Database=FSH_Starter;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=true;",
    "Provider": "mssql"
  },
  "SecurityOptions": {
    "JwtOptions": {
      "Key": "your-long-secret-key-min-32-characters",
      "DurationInMinutes": 60
    }
  },
  "Serilog": {
    "MinimumLevel": "Warning",
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "C:\\inetpub\\fsh-api\\logs\\log-.txt",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

### 5. Set File Permissions

```powershell
# Grant write permissions for logs and temp files
$Acl = Get-Acl "C:\inetpub\fsh-api"
$Rule = New-Object System.Security.AccessControl.FileSystemAccessRule(
    "IIS AppPool\FSH-API-Pool",
    "Modify",
    "ContainerInherit,ObjectInherit",
    "None",
    "Allow"
)
$Acl.SetAccessRule($Rule)
Set-Acl "C:\inetpub\fsh-api" $Acl
```

### 6. Test the Deployment

```powershell
# Start the application pool and website
Start-WebAppPool -Name "FSH-API-Pool"
Start-Website -Name "FSH-API"

# Test the API
$response = Invoke-WebRequest -Uri "http://localhost:5000/api/health" -ErrorAction SilentlyContinue
if ($response.StatusCode -eq 200) {
    Write-Host "API is running successfully!"
} else {
    Write-Host "API failed to respond correctly"
}
```

## Blazor Client Deployment to IIS

### 1. Create Application Pool and Website

```powershell
$BlazorAppPoolName = "FSH-Blazor-Pool"
New-WebAppPool -Name $BlazorAppPoolName
Set-ItemProperty "IIS:\AppPools\$BlazorAppPoolName" -Name "processModel.identityType" -Value "ApplicationPoolIdentity"

$BlazorSiteName = "FSH-Blazor"
$BlazorPath = "C:\inetpub\fsh-blazor"

New-Website -Name $BlazorSiteName -PhysicalPath $BlazorPath -ApplicationPool $BlazorAppPoolName -Port 5100
```

### 2. Configure Blazor Settings

Edit `C:\inetpub\fsh-blazor\appsettings.json`:

```json
{
  "ApiUrl": "http://YOUR_API_SERVER:5000",
  "ApiBaseAddress": "http://YOUR_API_SERVER:5000/api",
  "ApplicationUrl": "http://YOUR_BLAZOR_SERVER:5100"
}
```

### 3. Add IIS Rewrite Rules (if needed)

For proper routing in IIS, add `web.config`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="Handle History" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="index.html" />
        </rule>
      </rules>
    </rewrite>
    <staticContent>
      <mimeMap fileExtension=".json" mimeType="application/json" />
    </staticContent>
  </system.webServer>
</configuration>
```

## Troubleshooting

### Issue: 500.19 or 500.30 Errors

**Solution:** Check that .NET Hosting Bundle is installed and IIS is properly configured.

```powershell
# Check if ASP.NET Core Module is installed
Get-Item "C:\Windows\System32\inetsrv\aspnetcore.dll"
```

### Issue: Application Pool Crashes

**Check event logs:**

```powershell
Get-EventLog -LogName Application -Source ".NET Runtime" -Newest 10
```

### Issue: API Cannot Connect to Database

**Verify connection string** in `appsettings.json` and test database connectivity:

```powershell
# Test from the IIS server
$connString = "Server=YOUR_DB_SERVER;Database=FSH_Starter;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=true;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
Write-Host "Database connection successful!"
$conn.Close()
```

### Issue: CORS Errors

**Configure CORS in API** - Edit `C:\inetpub\fsh-api\appsettings.json`:

```json
{
  "CorsOptions": {
    "AllowedOrigins": [
      "http://your-blazor-server:5100",
      "https://your-blazor-server.com"
    ]
  }
}
```

## SSL/HTTPS Configuration

### 1. Install SSL Certificate

In IIS Manager:
1. Select your website
2. In Features View, click "Bindings"
3. Add HTTPS binding with your SSL certificate

### 2. Update Configuration

Update appsettings.json CORS and API URLs to use `https://`

### 3. Enforce HTTPS

```json
{
  "ForceHttps": true
}
```

## Performance Tuning

### Application Pool Settings

```powershell
$AppPoolName = "FSH-API-Pool"

# Set recycling to 4 hours instead of 20 minutes
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "recyclingConfig.periodicRestart.time" -Value 240

# Enable 64-bit applications
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "enable32BitAppCompat" -Value $false

# Set max connections
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "queueLength" -Value 5000
```

## Monitoring and Maintenance

### Log Locations

- **API Logs:** `C:\inetpub\fsh-api\logs\`
- **IIS Logs:** `C:\inetpub\logs\LogFiles\W3SVC\`
- **Application Event Log:** Windows Event Viewer > Windows Logs > Application

### Backup Strategy

```powershell
# Regular backups
Backup-WebConfiguration -Force
Get-ChildItem -Path "C:\inetpub\fsh-api" | Compress-Archive -DestinationPath "C:\backups\fsh-api-$(Get-Date -Format 'yyyyMMdd').zip"
```

## Rollback Procedure

```powershell
# In case of issues, quickly restore previous version
$backupZip = "C:\backups\fsh-api-previous.zip"
Stop-Website -Name "FSH-API"
Stop-WebAppPool -Name "FSH-API-Pool"

Remove-Item "C:\inetpub\fsh-api" -Recurse
Expand-Archive -Path $backupZip -DestinationPath "C:\inetpub"

Start-WebAppPool -Name "FSH-API-Pool"
Start-Website -Name "FSH-API"
```

## Additional Resources

- [Microsoft IIS Documentation](https://docs.microsoft.com/en-us/iis/)
- [ASP.NET Core on IIS](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/)
- [.NET 9 Downloads](https://dotnet.microsoft.com/download/dotnet/9.0)

