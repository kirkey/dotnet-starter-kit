# 🚀 Blazor WebAssembly Deployment Guide for Windows Server IIS

**Complete Step-by-Step Guide**

---

## 📋 Table of Contents

1. [Prerequisites](#prerequisites)
2. [Prepare Your Blazor Application](#prepare-your-blazor-application)
3. [Windows Server Preparation](#windows-server-preparation)
4. [Deploy to IIS](#deploy-to-iis)
5. [Configure IIS](#configure-iis)
6. [Testing & Troubleshooting](#testing--troubleshooting)
7. [Production Checklist](#production-checklist)

---

## 1. Prerequisites

### On Your Development Machine (macOS)
- ✅ Already have: Published Blazor app in `publishfsh9/blazor/`
- ✅ 390 files, 42 MB, ready to deploy

### On Windows Server
- Windows Server 2016 or later (2019/2022 recommended)
- IIS 10 or later installed
- Administrator access
- Internet connection (for downloading components)

---

## 2. Prepare Your Blazor Application

### Step 2.1: Create Deployment Package

On your Mac, run:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
make create-deployment-packages
```

This creates:
- ✅ `publishfsh9/FSH.Starter.Blazor.zip` (ready to upload)

**OR** manually create ZIP:

```bash
cd publishfsh9/blazor
zip -r ../FSH.Starter.Blazor.zip .
```

### Step 2.2: Configure API URL

**CRITICAL**: Before transferring to Windows Server, update the API URL.

**Option A - Update Before Zipping:**
```bash
cd publishfsh9/blazor
nano appsettings.json
```

Change:
```json
{
  "ApiBaseUrl": "https://localhost:7000/"
}
```

To:
```json
{
  "ApiBaseUrl": "https://your-api-server.com/"
}
```

**Option B - Update After Extracting on Server** (recommended for flexibility)

### Step 2.3: Transfer Files to Windows Server

**Methods:**
- **RDP**: Copy/paste the ZIP file via Remote Desktop
- **FTP/SFTP**: Upload using FileZilla or WinSCP
- **Cloud Storage**: Upload to OneDrive/Dropbox, download on server
- **Network Share**: Copy to shared folder

Transfer `FSH.Starter.Blazor.zip` to the Windows Server (e.g., to `C:\Temp\`)

---

## 3. Windows Server Preparation

### Step 3.1: Install IIS (if not already installed)

Open **PowerShell as Administrator**:

```powershell
# Install IIS with all required features
Install-WindowsFeature -Name Web-Server -IncludeManagementTools

# Install additional features for static content
Install-WindowsFeature -Name Web-Static-Content
Install-WindowsFeature -Name Web-Default-Doc
Install-WindowsFeature -Name Web-Dir-Browsing
Install-WindowsFeature -Name Web-Http-Errors
Install-WindowsFeature -Name Web-Http-Logging
```

### Step 3.2: Install URL Rewrite Module (REQUIRED!)

**This is CRITICAL for Blazor routing to work!**

1. Download URL Rewrite Module:
   - Go to: https://www.iis.net/downloads/microsoft/url-rewrite
   - Or direct link: https://download.microsoft.com/download/1/2/8/128E2E22-C1B9-44A4-BE2A-5859ED1D4592/rewrite_amd64_en-US.msi

2. Run the installer (double-click `rewrite_amd64_en-US.msi`)

3. Follow the installation wizard (defaults are fine)

4. Restart IIS:
   ```powershell
   iisreset
   ```

### Step 3.3: Install .NET Hosting Bundle (Optional but Recommended)

**Note**: Not strictly required for Blazor WebAssembly, but useful if you also host the API on same server.

1. Download .NET 9.0 Hosting Bundle:
   - Go to: https://dotnet.microsoft.com/download/dotnet/9.0
   - Download "Hosting Bundle" (not Runtime)

2. Run the installer

3. Restart IIS:
   ```powershell
   iisreset
   ```

---

## 4. Deploy to IIS

### Step 4.1: Extract Blazor Files

1. Open **File Explorer** on Windows Server

2. Create deployment folder:
   ```
   C:\inetpub\wwwroot\fsh-blazor\
   ```

3. Right-click `FSH.Starter.Blazor.zip` → **Extract All...**

4. Extract to: `C:\inetpub\wwwroot\fsh-blazor\`

**Your folder structure should look like:**
```
C:\inetpub\wwwroot\fsh-blazor\
├── index.html
├── appsettings.json
├── web.config
├── favicon.png
├── _framework\
│   ├── blazor.boot.json
│   ├── blazor.webassembly.js
│   ├── dotnet.native.*.wasm
│   └── [105 .wasm files]
├── _content\
├── css\
└── js\
```

### Step 4.2: Update API Configuration

Open `C:\inetpub\wwwroot\fsh-blazor\appsettings.json` in Notepad:

```json
{
  "ApiBaseUrl": "https://your-production-api.com/"
}
```

**Examples:**
- If API is on same server: `"https://api.yourdomain.com/"`
- If API is separate: `"https://api.external.com/"`
- If using IP: `"https://192.168.1.100:5000/"`

**Save the file!**

---

## 5. Configure IIS

### Step 5.1: Create Application Pool

1. Open **IIS Manager** (Run: `inetmgr`)

2. In left pane, expand server name → Click **Application Pools**

3. Right-click in middle pane → **Add Application Pool...**

4. Configure:
   - **Name**: `FSH.Blazor.AppPool`
   - **.NET CLR Version**: **No Managed Code** (IMPORTANT!)
   - **Managed Pipeline Mode**: Integrated
   - Click **OK**

5. Right-click the new pool → **Advanced Settings...**
   - **Start Mode**: AlwaysRunning (optional, for better performance)
   - Click **OK**

### Step 5.2: Create IIS Website

1. In **IIS Manager**, expand server → Right-click **Sites** → **Add Website...**

2. Configure:
   ```
   Site name:          FSH Blazor Client
   Application pool:   FSH.Blazor.AppPool (select from dropdown)
   Physical path:      C:\inetpub\wwwroot\fsh-blazor
   
   Binding:
   Type:               http (or https if you have certificate)
   IP address:         All Unassigned (or specific IP)
   Port:               80 (or 8080, or any available port)
   Host name:          (leave blank or enter: blazor.yourdomain.com)
   ```

3. Click **OK**

### Step 5.3: Verify web.config

Your `web.config` should already exist from the publish. Verify it contains:

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
      <mimeMap fileExtension=".woff" mimeType="font/woff" />
      <mimeMap fileExtension=".woff2" mimeType="font/woff2" />
    </staticContent>
    <rewrite>
      <rules>
        <rule name="Serve subdir">
          <match url=".*" />
          <action type="Rewrite" url="wwwroot\{R:0}" />
        </rule>
        <rule name="SPA fallback routing" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
          </conditions>
          <action type="Rewrite" url="wwwroot\index.html" />
        </rule>
      </rules>
    </rewrite>
    <httpCompression>
      <dynamicTypes>
        <remove mimeType="*/*" />
        <add mimeType="*/*" enabled="true" />
      </dynamicTypes>
      <staticTypes>
        <remove mimeType="*/*" />
        <add mimeType="*/*" enabled="true" />
      </staticTypes>
    </httpCompression>
  </system.webServer>
</configuration>
```

**Note**: The URL Rewrite rules ensure that:
- All routes (e.g., `/products`, `/login`) redirect to `index.html`
- This allows Blazor client-side routing to work properly

### Step 5.4: Set Folder Permissions

1. Right-click `C:\inetpub\wwwroot\fsh-blazor` → **Properties**

2. Go to **Security** tab

3. Click **Edit...** → **Add...**

4. Add these accounts with **Read & Execute** permissions:
   - `IIS_IUSRS`
   - `IUSR`

5. Click **OK** → **Apply** → **OK**

### Step 5.5: Enable Compression (Optional but Recommended)

1. In **IIS Manager**, click your site name

2. Double-click **Compression**

3. Check:
   - ✅ **Enable dynamic content compression**
   - ✅ **Enable static content compression**

4. Click **Apply**

---

## 6. Testing & Troubleshooting

### Step 6.1: Start the Website

1. In **IIS Manager**, click your site

2. In right pane, click **Browse *:80 (http)**

**Expected Result**: Your Blazor app should open in the browser!

### Step 6.2: Check Browser Console

Press **F12** in browser to open Developer Tools → **Console** tab

**Good signs:**
```
✅ No errors
✅ See: "Loading Blazor WebAssembly..."
✅ See: Successful fetch of _framework files
```

**Common errors and fixes:**

#### Error: "Failed to fetch _framework/blazor.boot.json"
**Fix**: URL Rewrite module not installed
- Install URL Rewrite Module (Step 3.2)
- Restart IIS: `iisreset`

#### Error: "404 Not Found" on .wasm files
**Fix**: MIME types not configured
- Verify `web.config` has MIME type mappings
- Or add manually in IIS Manager → MIME Types

#### Error: "Failed to load API" or CORS errors
**Fix**: API URL incorrect or CORS not configured
- Verify `appsettings.json` has correct API URL
- Configure CORS on API to allow your Blazor domain

#### Error: "Routing doesn't work" (404 on refresh)
**Fix**: URL Rewrite rules not working
- Install URL Rewrite Module
- Verify `web.config` has rewrite rules
- Restart IIS

### Step 6.3: Test Routing

1. Navigate to different pages in your app (e.g., `/products`, `/login`)

2. **Press F5 to refresh** the page

3. **Expected**: Page should reload correctly (not 404)

If you get 404 on refresh:
- URL Rewrite module is not installed or not working
- Check `web.config` rewrite rules

### Step 6.4: Check Performance

1. Open Developer Tools → **Network** tab

2. Refresh the page

3. Look for:
   - ✅ `.wasm.br` files being served (Brotli compressed)
   - ✅ Or `.wasm.gz` files (Gzip compressed)
   - ✅ Size should be ~40% smaller than original

**If serving uncompressed .wasm files:**
- Enable static compression in IIS (Step 5.5)
- Ensure browser supports compression (modern browsers do)

---

## 7. Production Checklist

### Security

- [ ] **HTTPS Configured**: Install SSL certificate and use HTTPS binding
- [ ] **HTTP → HTTPS Redirect**: Force all traffic to HTTPS
- [ ] **Security Headers**: Add security headers in web.config
- [ ] **CORS Configured**: API allows only your Blazor domain
- [ ] **API Authentication**: JWT/Cookie authentication working

### Performance

- [ ] **Compression Enabled**: Both static and dynamic compression
- [ ] **Caching Configured**: Browser caching for static assets
- [ ] **CDN** (optional): Use CDN for static files
- [ ] **HTTP/2 Enabled**: Better performance for many small files

### Monitoring

- [ ] **IIS Logs**: Check `C:\inetpub\logs\LogFiles\`
- [ ] **Application Insights** (optional): Monitor performance
- [ ] **Health Check**: Set up endpoint monitoring

### Backup

- [ ] **Backup Files**: Keep a copy of `C:\inetpub\wwwroot\fsh-blazor\`
- [ ] **Configuration Backup**: Save IIS configuration
- [ ] **Deployment Package**: Keep `FSH.Starter.Blazor.zip`

---

## 🔧 Advanced Configuration

### Adding HTTPS

1. Obtain SSL certificate (Let's Encrypt, commercial CA, or self-signed)

2. In IIS Manager → Your site → **Bindings...**

3. Click **Add...**
   - Type: **https**
   - Port: **443**
   - SSL certificate: Select your certificate

4. Add HTTP → HTTPS redirect in `web.config`:

```xml
<system.webServer>
  <rewrite>
    <rules>
      <rule name="HTTP to HTTPS redirect" stopProcessing="true">
        <match url="(.*)" />
        <conditions>
          <add input="{HTTPS}" pattern="off" ignoreCase="true" />
        </conditions>
        <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" redirectType="Permanent" />
      </rule>
      <!-- ...existing rules... -->
    </rules>
  </rewrite>
</system.webServer>
```

### Custom Domain

1. Update DNS records:
   ```
   Type: A
   Name: blazor (or @)
   Value: [Your server IP]
   ```

2. In IIS binding, add host name: `blazor.yourdomain.com`

3. Update API CORS to allow: `https://blazor.yourdomain.com`

### Multiple Environments

Create separate folders and sites:
```
C:\inetpub\wwwroot\fsh-blazor-dev\
C:\inetpub\wwwroot\fsh-blazor-staging\
C:\inetpub\wwwroot\fsh-blazor-prod\
```

Each with different `appsettings.json` pointing to respective APIs.

---

## 📊 File Size Reference

Your deployed Blazor app:
```
Total Size:          42 MB
Uncompressed:        ~42 MB
Brotli Compressed:   ~17 MB (60% reduction)
Gzip Compressed:     ~20 MB (52% reduction)

Files:               390 total
- WASM assemblies:   105 files
- Compressed:        210 .br and .gz files
- Static assets:     75 files
```

**First Load**: ~17-20 MB (with compression)
**Cached Load**: < 1 KB (only checks for updates)

---

## 🆘 Common Issues & Solutions

### Issue: White screen, no errors
**Solution**: 
- Check browser console for errors
- Verify all files extracted properly
- Check `appsettings.json` API URL

### Issue: API calls fail
**Solution**:
- Verify API is running and accessible
- Check CORS configuration on API
- Verify `appsettings.json` has correct API URL
- Check network/firewall rules

### Issue: 500.19 Error
**Solution**:
- Install URL Rewrite Module
- Check `web.config` syntax
- Restart IIS

### Issue: Slow loading
**Solution**:
- Enable compression in IIS
- Check if `.br` or `.gz` files are being served
- Consider using CDN

### Issue: Works on localhost, fails on server
**Solution**:
- Check Windows Firewall (open port 80/443)
- Verify DNS settings
- Check IIS bindings

---

## 📚 Additional Resources

- [Blazor Hosting Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly)
- [IIS URL Rewrite Module](https://www.iis.net/downloads/microsoft/url-rewrite)
- [IIS Compression](https://learn.microsoft.com/en-us/iis/configuration/system.webserver/httpcompression/)

---

## 🎯 Quick Reference Commands

### On Development Machine (Mac)
```bash
# Create deployment package
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
make publish-blazor
make create-deployment-packages

# The result:
# publishfsh9/FSH.Starter.Blazor.zip → Transfer to Windows Server
```

### On Windows Server (PowerShell)
```powershell
# Install IIS
Install-WindowsFeature -Name Web-Server -IncludeManagementTools

# Restart IIS
iisreset

# Stop site
Stop-Website -Name "FSH Blazor Client"

# Start site
Start-Website -Name "FSH Blazor Client"

# View IIS logs
Get-Content "C:\inetpub\logs\LogFiles\W3SVC1\*.log" -Tail 50

# Test site locally
Invoke-WebRequest -Uri "http://localhost" -UseBasicParsing
```

---

## ✅ Success Criteria

Your deployment is successful when:

1. ✅ Website loads without errors
2. ✅ All pages/routes work correctly
3. ✅ Page refresh doesn't cause 404 errors
4. ✅ API calls work (check Network tab)
5. ✅ Login/authentication works
6. ✅ Files are compressed (check Network tab)
7. ✅ HTTPS is working (if configured)
8. ✅ No console errors in browser
9. ✅ Performance is acceptable
10. ✅ Can access from other computers

---

**Deployment Date**: October 28, 2025
**Application**: FSH Starter Blazor WebAssembly
**Files**: 390 files, 42 MB
**Status**: ✅ Ready for Production

**Need help?** Check the troubleshooting section or Windows Server Event Viewer.

Good luck with your deployment! 🚀

