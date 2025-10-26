# IIS Deployment Guide for .NET API and Blazor WebAssembly

This guide will walk you through deploying your .NET API (Server) and Blazor WebAssembly client to an IIS server.

## Prerequisites

### On the IIS Server:
1. **Windows Server** (2016 or later recommended)
2. **IIS** (Internet Information Services) installed
3. **.NET 9.0 Hosting Bundle** - [Download from Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
   - This includes ASP.NET Core Runtime and IIS support
4. **URL Rewrite Module** for IIS - [Download here](https://www.iis.net/downloads/microsoft/url-rewrite)
5. **SQL Server** or access to your database server (for the API)
6. **Administrator access** to the IIS server

### On Your Development Machine:
1. **.NET 9.0 SDK** installed
2. Access to publish the applications

## Part 1: Prepare the API for Deployment

### Step 1.1: Configure Production Settings

1. **Update `appsettings.json`** in `/api/server/`:
   - Set the production database connection string
   - Configure CORS to allow your Blazor client domain
   - Set appropriate logging levels
   - Configure authentication/authorization settings

Example CORS configuration in your API:
```json
{
  "CorsSettings": {
    "AllowedOrigins": ["https://your-blazor-domain.com", "http://localhost"],
    "AllowCredentials": true
  }
}
```

### Step 1.2: Publish the API

Run the following command from your development machine:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/server
dotnet publish -c Release -o ./publish --no-self-contained
```

This will create a `publish` folder with all necessary files.

### Step 1.3: Package the API

Create a ZIP file of the `publish` folder or use a file transfer method to copy it to your IIS server.

## Part 2: Prepare the Blazor Client for Deployment

### Step 2.1: Configure API Endpoint

1. **Update `wwwroot/appsettings.json`** in `/apps/blazor/client/`:

```json
{
  "ApiBaseUrl": "https://your-api-domain.com"
}
```

Replace `https://your-api-domain.com` with your actual API URL.

### Step 2.2: Publish the Blazor Client

Run the following command:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/client
dotnet publish -c Release -o ./publish
```

This will create a `publish/wwwroot` folder with your Blazor WebAssembly application.

### Step 2.3: Package the Blazor Client

Create a ZIP file of the `publish/wwwroot` folder content.

## Part 3: Deploy to IIS

### Step 3.1: Install Prerequisites on IIS Server

1. **Install .NET 9.0 Hosting Bundle:**
   - Download from [https://dotnet.microsoft.com/en-us/download/dotnet/9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
   - Run the installer
   - **Restart IIS** after installation: `iisreset`

2. **Install URL Rewrite Module:**
   - Download from [https://www.iis.net/downloads/microsoft/url-rewrite](https://www.iis.net/downloads/microsoft/url-rewrite)
   - Run the installer

3. **Verify Installation:**
   ```powershell
   dotnet --list-runtimes
   ```
   You should see ASP.NET Core Runtime 9.x.x listed.

### Step 3.2: Deploy the API to IIS

1. **Create Application Pool for API:**
   - Open IIS Manager
   - Right-click "Application Pools" → "Add Application Pool"
   - Name: `FSH.Starter.API`
   - .NET CLR Version: **No Managed Code**
   - Managed Pipeline Mode: Integrated
   - Click "OK"

2. **Configure Application Pool:**
   - Select the new pool → "Advanced Settings"
   - Identity: Choose appropriate identity (ApplicationPoolIdentity or custom account with DB access)
   - Start Mode: AlwaysRunning (optional, for better performance)
   - Idle Time-out: 0 (if you want to keep it always running)

3. **Create Website or Application:**
   
   **Option A: Create a new Website:**
   - Right-click "Sites" → "Add Website"
   - Site name: `FSH.Starter.API`
   - Application pool: Select `FSH.Starter.API`
   - Physical path: `C:\inetpub\wwwroot\FSH.Starter.API` (or your preferred location)
   - Binding:
     - Type: https (recommended)
     - IP: All Unassigned
     - Port: 443
     - Host name: `api.yourdomain.com`
     - SSL Certificate: Select your SSL certificate

   **Option B: Create as an Application under existing site:**
   - Right-click Default Web Site → "Add Application"
   - Alias: `api`
   - Application pool: Select `FSH.Starter.API`
   - Physical path: `C:\inetpub\wwwroot\api`

4. **Copy Published Files:**
   - Transfer the API publish folder content to the physical path
   - Ensure the folder has appropriate permissions:
     ```powershell
     icacls "C:\inetpub\wwwroot\FSH.Starter.API" /grant "IIS_IUSRS:(OI)(CI)F" /T
     ```

5. **Configure web.config (usually auto-generated):**
   The publish process should create a `web.config`. Verify it looks like this:
   
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <location path="." inheritInChildApplications="false">
       <system.webServer>
         <handlers>
           <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
         </handlers>
         <aspNetCore processPath="dotnet" 
                     arguments=".\FSH.Starter.WebApi.Host.dll" 
                     stdoutLogEnabled="true" 
                     stdoutLogFile=".\logs\stdout" 
                     hostingModel="inprocess" />
       </system.webServer>
     </location>
   </configuration>
   ```

6. **Create Logs Folder:**
   ```powershell
   mkdir C:\inetpub\wwwroot\FSH.Starter.API\logs
   ```

7. **Test the API:**
   - Browse to `https://api.yourdomain.com/swagger` or your API health endpoint
   - Check logs folder if there are issues

### Step 3.3: Deploy the Blazor Client to IIS

1. **Create Application Pool for Blazor:**
   - Open IIS Manager
   - Right-click "Application Pools" → "Add Application Pool"
   - Name: `FSH.Starter.Blazor`
   - .NET CLR Version: **No Managed Code**
   - Click "OK"

2. **Create Website:**
   - Right-click "Sites" → "Add Website"
   - Site name: `FSH.Starter.Blazor`
   - Application pool: Select `FSH.Starter.Blazor`
   - Physical path: `C:\inetpub\wwwroot\FSH.Starter.Blazor`
   - Binding:
     - Type: https
     - Port: 443
     - Host name: `app.yourdomain.com` (or your domain)
     - SSL Certificate: Select your certificate

3. **Copy Published Files:**
   - Transfer the content of `publish/wwwroot` folder to `C:\inetpub\wwwroot\FSH.Starter.Blazor`

4. **Create/Update web.config for Blazor:**
   Create a `web.config` file in the Blazor root directory:

   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <system.webServer>
       <staticContent>
         <remove fileExtension=".blat" />
         <remove fileExtension=".dat" />
         <remove fileExtension=".dll" />
         <remove fileExtension=".json" />
         <remove fileExtension=".wasm" />
         <remove fileExtension=".woff" />
         <remove fileExtension=".woff2" />
         <mimeMap fileExtension=".blat" mimeType="application/octet-stream" />
         <mimeMap fileExtension=".dll" mimeType="application/octet-stream" />
         <mimeMap fileExtension=".dat" mimeType="application/octet-stream" />
         <mimeMap fileExtension=".json" mimeType="application/json" />
         <mimeMap fileExtension=".wasm" mimeType="application/wasm" />
         <mimeMap fileExtension=".woff" mimeType="application/font-woff" />
         <mimeMap fileExtension=".woff2" mimeType="application/font-woff2" />
       </staticContent>
       <rewrite>
         <rules>
           <rule name="Serve service-worker" stopProcessing="true">
             <match url="^service-worker\.js$" />
             <action type="Rewrite" url="service-worker.published.js" />
           </rule>
           <rule name="SPA Routes" stopProcessing="true">
             <match url=".*" />
             <conditions logicalGrouping="MatchAll">
               <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
               <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
             </conditions>
             <action type="Rewrite" url="/index.html" />
           </rule>
         </rules>
       </rewrite>
       <httpCompression>
         <dynamicTypes>
           <add mimeType="application/wasm" enabled="true" />
           <add mimeType="application/octet-stream" enabled="true" />
         </dynamicTypes>
       </httpCompression>
     </system.webServer>
   </configuration>
   ```

5. **Test the Blazor App:**
   - Browse to `https://app.yourdomain.com`
   - The Blazor app should load and communicate with your API

## Part 4: Post-Deployment Configuration

### Step 4.1: Configure SSL/TLS Certificates

1. **For Production:**
   - Obtain SSL certificates from a Certificate Authority (Let's Encrypt, DigiCert, etc.)
   - Install certificates in IIS
   - Bind certificates to your websites
   - Enable HTTPS redirect

2. **Update Bindings:**
   - In IIS Manager, select your site
   - Click "Bindings" → "Add"
   - Add HTTP binding on port 80 (for redirect)
   - Add HTTPS binding on port 443 with your certificate

### Step 4.2: Configure Firewall

Open necessary ports on Windows Firewall:
```powershell
# HTTP
New-NetFirewallRule -DisplayName "Allow HTTP" -Direction Inbound -Protocol TCP -LocalPort 80 -Action Allow

# HTTPS
New-NetFirewallRule -DisplayName "Allow HTTPS" -Direction Inbound -Protocol TCP -LocalPort 443 -Action Allow
```

### Step 4.3: Database Configuration

1. Ensure the API application pool identity has access to the database
2. Update connection strings in `appsettings.json` on the server
3. Run database migrations if needed:
   ```powershell
   cd C:\inetpub\wwwroot\FSH.Starter.API
   dotnet FSH.Starter.WebApi.Host.dll --migrate
   ```

### Step 4.4: Set Up Monitoring and Logging

1. **Configure Windows Event Log integration**
2. **Set up Application Insights** (optional)
3. **Monitor IIS logs** in `C:\inetpub\logs\LogFiles`
4. **Monitor Application logs** in your configured log directory

## Part 5: Troubleshooting

### Common Issues:

1. **500.19 Error - Configuration Error:**
   - Install URL Rewrite Module
   - Check web.config syntax

2. **500.30 Error - ANCM In-Process Start Failure:**
   - .NET Hosting Bundle not installed or IIS not restarted
   - Check application pool settings (No Managed Code)
   - Check logs in the logs folder

3. **502.5 Error - Process Failure:**
   - Check application pool identity has proper permissions
   - Verify .NET runtime is installed
   - Check database connection strings

4. **Blazor App Not Loading:**
   - Check console for CORS errors
   - Verify ApiBaseUrl in appsettings.json
   - Check network tab for 404 errors on .dll or .wasm files
   - Ensure MIME types are configured correctly

5. **Database Connection Issues:**
   - Verify connection string
   - Check firewall rules
   - Grant database access to application pool identity

### Useful Commands:

```powershell
# Restart IIS
iisreset

# Check .NET runtimes installed
dotnet --list-runtimes

# View application pool status
Get-IISAppPool

# View site bindings
Get-IISSite

# Grant permissions to folder
icacls "C:\inetpub\wwwroot\FSH.Starter.API" /grant "IIS_IUSRS:(OI)(CI)F" /T
```

### Enable Detailed Errors (for debugging only):

In the API's `web.config`, temporarily add:
```xml
<aspNetCore processPath="dotnet" 
            arguments=".\FSH.Starter.WebApi.Host.dll" 
            stdoutLogEnabled="true" 
            stdoutLogFile=".\logs\stdout" 
            hostingModel="inprocess">
  <environmentVariables>
    <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Development" />
  </environmentVariables>
</aspNetCore>
```

**Remember to remove this in production!**

## Part 6: Security Checklist

- [ ] SSL/TLS certificates installed and configured
- [ ] HTTP to HTTPS redirect enabled
- [ ] CORS properly configured (not allowing *)
- [ ] Authentication and authorization configured
- [ ] Database connection uses secure credentials
- [ ] Application pool running with least privilege account
- [ ] Sensitive data encrypted in configuration
- [ ] Regular security updates applied
- [ ] Rate limiting configured
- [ ] File upload restrictions in place

## Part 7: Performance Optimization

1. **Enable Response Compression** in your API
2. **Configure Output Caching** where appropriate
3. **Enable HTTP/2** in IIS
4. **Configure CDN** for Blazor static assets (optional)
5. **Enable Brotli Compression** for better performance
6. **Set up Application Pool recycling** schedules

## Additional Resources

- [Host ASP.NET Core on Windows with IIS](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/)
- [Host and deploy ASP.NET Core Blazor WebAssembly](https://docs.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly)
- [.NET Downloads](https://dotnet.microsoft.com/download)

## Quick Reference

### API Deployment Checklist:
1. ✓ Publish API in Release mode
2. ✓ Install .NET Hosting Bundle on server
3. ✓ Create Application Pool (No Managed Code)
4. ✓ Create Website/Application
5. ✓ Copy files to server
6. ✓ Configure database connection
7. ✓ Set proper permissions
8. ✓ Configure SSL
9. ✓ Test endpoints

### Blazor Deployment Checklist:
1. ✓ Configure ApiBaseUrl in appsettings.json
2. ✓ Publish Blazor app in Release mode
3. ✓ Create Application Pool
4. ✓ Create Website
5. ✓ Copy wwwroot content to server
6. ✓ Add web.config with URL rewrite rules
7. ✓ Configure MIME types
8. ✓ Configure SSL
9. ✓ Test application

---

**Need Help?** If you encounter issues:
1. Check the logs in `C:\inetpub\wwwroot\[your-app]\logs`
2. Check Windows Event Viewer
3. Enable detailed errors temporarily
4. Verify all prerequisites are installed
5. Check IIS application pool is running

