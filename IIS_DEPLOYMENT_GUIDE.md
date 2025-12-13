# Windows Server IIS Deployment Guide

## Overview

This guide provides comprehensive instructions for deploying the FSH Starter Kit (API and Blazor Client) to Windows Server with IIS.

**Requirements:**
- Windows Server 2022 or later (Windows 11/2019 also supported)
- .NET 9 Runtime and Hosting Bundle
- IIS 10.0 or higher
- IIS URL Rewrite Module
- SSL/TLS Certificate (for HTTPS)

---

## Pre-Deployment Setup

### 1. Install .NET 9 Hosting Bundle

1. Download the **.NET 9 Hosting Bundle** from [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2. Run the installer with Administrator privileges
3. **Restart IIS** after installation:
   ```powershell
   iisreset /restart
   ```

### 2. Install IIS URL Rewrite Module

1. Download from: [IIS URL Rewrite Module](https://www.iis.net/downloads/microsoft/url-rewrite)
2. Run installer with Administrator privileges
3. Restart IIS

### 3. Enable Required IIS Features

1. Open **Add Roles and Features** (Server Manager)
2. Navigate to **Web Server (IIS)** → **Application Development**
3. Ensure these are **enabled**:
   - [x] .NET Extensibility 4.8
   - [x] Application Initialization
   - [x] ASP.NET 4.8
   - [x] ISAPI Extensions
   - [x] ISAPI Filters
4. Under **Web Server**, enable:
   - [x] URL Rewrite
   - [x] Static Content
   - [x] HTTP Compression
5. Click **Install** and restart server if prompted

---

## Publishing the Application

### Using PowerShell Script (Recommended)

From your development machine:

```powershell
# Basic usage
.\src\publish-for-iis.ps1

# With custom output directory
.\src\publish-for-iis.ps1 -OutputPath "C:\deployment"

# With automatic web.config generation
.\src\publish-for-iis.ps1 -CreateWebConfig

# Skip verification checks
.\src\publish-for-iis.ps1 -SkipVerification
```

**Output files:**
- `FSH.Starter.API_YYYYMMDD_HHMMSS.zip` - API package
- `FSH.Starter.Blazor_YYYYMMDD_HHMMSS.zip` - Blazor package
- `FSH.Starter.Deployment_YYYYMMDD_HHMMSS.zip` - Combined package

### Using Make (Linux/macOS)

```bash
make publish-api          # API only
make publish-blazor       # Blazor only
make publish-all          # Both with ZIP packages
make clean-publish        # Clean output directory
```

---

## Deployment Steps

### Step 1: Transfer Files to Server

1. Copy the ZIP packages to your Windows Server
2. Extract to appropriate locations:
   - **API**: `C:\inetpub\wwwroot\api`
   - **Blazor**: `C:\inetpub\wwwroot\blazor`

```powershell
# Extract API
Expand-Archive -Path "FSH.Starter.API_*.zip" -DestinationPath "C:\inetpub\wwwroot\api"

# Extract Blazor
Expand-Archive -Path "FSH.Starter.Blazor_*.zip" -DestinationPath "C:\inetpub\wwwroot\blazor"
```

### Step 2: Create Application Pools

1. Open **IIS Manager** → **Application Pools**
2. Create new Application Pool for API:
   - **Name**: `FSHApiPool`
   - **.NET CLR Version**: `No Managed Code`
   - **Managed Pipeline Mode**: `Integrated`
3. Create new Application Pool for Blazor:
   - **Name**: `FSHBlazerPool`
   - **.NET CLR Version**: `No Managed Code`
   - **Managed Pipeline Mode**: `Integrated`

### Step 3: Create IIS Sites

#### API Site

1. Right-click **Sites** → **Add Website**
2. Configure:
   - **Site name**: `FSH API`
   - **Application pool**: `FSHApiPool`
   - **Physical path**: `C:\inetpub\wwwroot\api`
   - **Binding**:
     - **Type**: `https`
     - **IP**: `All Unassigned`
     - **Port**: `443`
     - **Host name**: `api.yourdomain.com`
     - **SSL Certificate**: Select your certificate
3. Click **OK**

#### Blazor Site

1. Right-click **Sites** → **Add Website**
2. Configure:
   - **Site name**: `FSH Blazor`
   - **Application pool**: `FSHBlazerPool`
   - **Physical path**: `C:\inetpub\wwwroot\blazor`
   - **Binding**:
     - **Type**: `https`
     - **IP**: `All Unassigned`
     - **Port**: `443`
     - **Host name**: `yourdomain.com`
     - **SSL Certificate**: Select your certificate
3. Click **OK**

### Step 4: Configure File Permissions

1. Open **File Explorer**
2. Navigate to `C:\inetpub\wwwroot\api`
3. Right-click → **Properties** → **Security** → **Edit**
4. Click **Add** and add:
   - `IIS AppPool\FSHApiPool` - **Full Control**
   - `IIS AppPool\FSHBlazerPool` - **Read & Execute** (for Blazor)
5. Repeat for Blazor directory with appropriate permissions

### Step 5: Configure Application Settings

#### API Configuration

Edit: `C:\inetpub\wwwroot\api\appsettings.Production.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_DB_SERVER;Database=FSHStarterKit;User Id=sa;Password=YOUR_PASSWORD;Encrypt=true;TrustServerCertificate=true;"
  },
  "Jwt": {
    "Key": "your-very-long-secret-key-min-32-chars",
    "Issuer": "https://api.yourdomain.com",
    "Audience": "https://yourdomain.com"
  },
  "Cors": {
    "AllowedOrigins": ["https://yourdomain.com", "https://www.yourdomain.com"]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    },
    "Serilog": {
      "MinimumLevel": "Information",
      "WriteTo": [
        {
          "Name": "File",
          "Args": {
            "path": "C:\\inetpub\\logs\\api\\log-.txt",
            "rollingInterval": "Day",
            "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
          }
        }
      ]
    }
  }
}
```

**Create logs directory:**
```powershell
New-Item -Path "C:\inetpub\logs\api" -ItemType Directory -Force
```

#### Blazor Configuration

Edit: `C:\inetpub\wwwroot\blazor\appsettings.json`

```json
{
  "ApiBaseUrl": "https://api.yourdomain.com",
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Step 6: Enable Compression

For optimal performance, enable HTTP compression in IIS:

1. Open **IIS Manager** → Select **Server**
2. Double-click **Compression** (under Management section)
3. Check both:
   - [x] Enable dynamic content compression
   - [x] Enable static content compression
4. Click **Apply** in the right panel

### Step 7: Configure SSL/TLS Certificate

#### Using Let's Encrypt (Recommended)

1. Install **Win-ACME** from [github.com/win-acme/win-acme](https://github.com/win-acme/win-acme)
2. Run with Administrator privileges
3. Choose `N - Create certificate (new certificate)`
4. Select your domain(s)
5. Choose IIS binding option
6. Auto-renewal is configured

#### Using Existing Certificate

1. Open **IIS Manager** → **Server Certificates**
2. Click **Import** (right panel)
3. Select your .pfx/.p7b file
4. For each site:
   - Right-click → **Edit Bindings**
   - Select HTTPS binding
   - Choose certificate from dropdown

### Step 8: Final Configuration

#### Create web.config Files (if not auto-generated)

**API web.config** at `C:\inetpub\wwwroot\api\web.config`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\Server.dll" stdoutLogEnabled="false">
        <environmentVariables>
          <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
        </environmentVariables>
      </aspNetCore>
      <rewrite>
        <rules>
          <rule name="HTTP to HTTPS redirect" stopProcessing="true">
            <match url="(.*)" />
            <conditions>
              <add input="{HTTPS}" pattern="^OFF$" />
            </conditions>
            <action type="Redirect" url="https://{HTTP_HOST}{REQUEST_URI}" />
          </rule>
        </rules>
      </rewrite>
    </system.webServer>
  </location>
</configuration>
```

#### Restart IIS

```powershell
iisreset /restart
```

---

## Verification & Testing

### 1. Check Application Pools

1. Open **IIS Manager** → **Application Pools**
2. Verify both pools are **Started** (Status: Started)
3. If not started, right-click → **Start**

### 2. Check Sites

1. Open **IIS Manager** → **Sites**
2. Verify both sites are **Started**
3. Check bindings are correct

### 3. Test API

Open a browser and navigate to:
```
https://api.yourdomain.com/api/health
```

Expected response:
```json
{
  "status": "Healthy"
}
```

### 4. Test Blazor Client

Navigate to:
```
https://yourdomain.com
```

Should load the Blazor WebAssembly application.

### 5. Check Event Logs

1. Open **Event Viewer**
2. Navigate to **Windows Logs** → **Application**
3. Look for entries from `dotnet` application
4. Check for any errors or warnings

### 6. View IIS Logs

Check logs at:
- **API logs**: `C:\inetpub\logs\api\`
- **IIS logs**: `C:\inetpub\logs\LogFiles\`

---

## Troubleshooting

### 502 Bad Gateway

**Cause**: Application pool not running or ASP.NET Core Module not found

**Solution**:
```powershell
# Check pool status
Get-IISAppPool -Name "FSHApiPool"

# Start pool if stopped
Start-IISAppPool -Name "FSHApiPool"

# Verify .NET installation
dotnet --version

# Restart IIS
iisreset /restart
```

### 404 Errors

**Cause**: Physical path incorrect or site not started

**Solution**:
1. Verify physical path exists: `C:\inetpub\wwwroot\api`
2. Check Web.config present
3. Restart site in IIS Manager

### CORS Errors

**Cause**: Blazor can't reach API

**Solution**:
1. Verify API base URL in Blazor appsettings.json
2. Check CORS configuration in API appsettings.Production.json
3. Ensure both sites use HTTPS
4. Check firewall rules

### SSL Certificate Errors

**Cause**: Certificate not valid for domain

**Solution**:
1. Verify certificate is bound to correct site
2. Check certificate expiration: `certutil -v -dump`
3. Ensure certificate CN/SAN matches domain

---

## Monitoring & Maintenance

### Enable Failed Request Tracing

1. In **IIS Manager**, select API site
2. Open **Failed Request Tracing**
3. Click **Enable** (right panel)
4. Configure what to trace (status codes, etc.)
5. Check logs in `%SystemDrive%\inetpub\logs\FailedReqLogFiles\`

### Regular Updates

```powershell
# Check for .NET updates
dotnet --list-runtimes
dotnet --list-sdks

# Update if newer version available
# Download from https://dotnet.microsoft.com/download
```

### Backup Strategy

Regularly backup:
```powershell
# Configuration
Copy-Item -Path "C:\inetpub\wwwroot\*" -Destination "\\backup\server\wwwroot" -Recurse

# Application configuration
Copy-Item -Path "C:\Windows\System32\inetsrv\config\*" -Destination "\\backup\server\iis-config" -Recurse
```

---

## Performance Optimization

### Application Pool Recycling

1. In IIS Manager, right-click **FSHApiPool** → **Recycling Conditions**
2. Configure:
   - Private Memory: 512 MB
   - Regular time interval: 1440 minutes (daily)
   - Time of day: 2:00 AM

### Enable HTTP/2

1. Windows Server 2016+: HTTP/2 is built-in
2. No additional configuration needed for HTTPS sites

### Output Caching

For Blazor static content:
1. Select site → **Output Caching**
2. Click **Enable Caching** (right panel)
3. Add rules for file types (.js, .wasm, etc.)

---

## Disaster Recovery

### Automated Backups

```powershell
# Create backup script
$date = Get-Date -Format "yyyyMMdd_HHmmss"
$backupPath = "C:\backups\FSH_$date"

New-Item -Path $backupPath -ItemType Directory -Force
Copy-Item -Path "C:\inetpub\wwwroot\api" -Destination "$backupPath\api" -Recurse
Copy-Item -Path "C:\inetpub\wwwroot\blazor" -Destination "$backupPath\blazor" -Recurse

# Run daily via Task Scheduler
```

### Quick Restore

```powershell
# Stop sites
Stop-Website -Name "FSH API"
Stop-Website -Name "FSH Blazor"

# Restore files
Remove-Item -Path "C:\inetpub\wwwroot\api" -Recurse -Force
Remove-Item -Path "C:\inetpub\wwwroot\blazor" -Recurse -Force
Copy-Item -Path "\\backup\latest\*" -Destination "C:\inetpub\wwwroot" -Recurse

# Start sites
Start-Website -Name "FSH API"
Start-Website -Name "FSH Blazor"

# Restart IIS
iisreset
```

---

## Security Best Practices

### 1. Enable Windows Defender Firewall Rules

```powershell
# Allow HTTPS
New-NetFirewallRule -DisplayName "HTTPS" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 443

# Allow HTTP redirect (optional)
New-NetFirewallRule -DisplayName "HTTP" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 80
```

### 2. Application Pool Identity

- Use **NetworkService** or custom service account
- Avoid using ApplicationPoolIdentity in production
- Apply principle of least privilege

### 3. Disable Unused Features

```powershell
# Disable WebDAV if not needed
Disable-WindowsOptionalFeature -Online -FeatureName IIS-WebDAV

# Disable FTP if not used
Uninstall-WindowsFeature Web-Ftp-Server
```

### 4. Security Headers (configured in web.config)

```xml
<httpProtocol>
  <customHeaders>
    <add name="X-Content-Type-Options" value="nosniff" />
    <add name="X-Frame-Options" value="SAMEORIGIN" />
    <add name="X-XSS-Protection" value="1; mode=block" />
    <add name="Strict-Transport-Security" value="max-age=31536000" />
  </customHeaders>
</httpProtocol>
```

---

## Support & Resources

- [Microsoft Docs - Host ASP.NET Core on Windows with IIS](https://learn.microsoft.com/aspnet/core/host-and-deploy/iis/)
- [IIS Documentation](https://learn.microsoft.com/iis/)
- [.NET Runtime Downloads](https://dotnet.microsoft.com/download)
- [Let's Encrypt - Free SSL Certificates](https://letsencrypt.org)

---

**Last Updated**: December 2024
**Tested on**: Windows Server 2022, .NET 9.0

