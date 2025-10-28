# 📋 Blazor IIS Deployment Quick Checklist

**Print this page and check off each step as you complete it!**

---

## 🖥️ ON YOUR MAC (Development Machine)

### Prepare Deployment Package

- [ ] Navigate to project: `cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src`
- [ ] Run: `make publish-blazor`
- [ ] Run: `make create-deployment-packages`
- [ ] Verify ZIP created: `ls -lh publishfsh9/FSH.Starter.Blazor.zip`
- [ ] Note: Should be ~15-20 MB compressed

### Optional: Update API URL Before Transfer

- [ ] Edit: `publishfsh9/blazor/appsettings.json`
- [ ] Change `ApiBaseUrl` to production API URL
- [ ] Re-create ZIP if changed

### Transfer to Windows Server

- [ ] Transfer `publishfsh9/FSH.Starter.Blazor.zip` to Windows Server
- [ ] Methods: RDP, FTP, OneDrive, Network Share
- [ ] Recommended location on server: `C:\Temp\`

---

## 🖥️ ON WINDOWS SERVER

### Part 1: Server Prerequisites (One-Time Setup)

#### Install IIS
- [ ] Open PowerShell as Administrator
- [ ] Run: `Install-WindowsFeature -Name Web-Server -IncludeManagementTools`
- [ ] Wait for installation to complete
- [ ] Verify: Open browser, go to `http://localhost` (should see IIS default page)

#### Install URL Rewrite Module (REQUIRED!)
- [ ] Download from: https://www.iis.net/downloads/microsoft/url-rewrite
- [ ] Or use: https://download.microsoft.com/download/1/2/8/128E2E22-C1B9-44A4-BE2A-5859ED1D4592/rewrite_amd64_en-US.msi
- [ ] Run installer, accept defaults
- [ ] Run: `iisreset` in PowerShell

#### Optional: Install .NET 9.0 Hosting Bundle
- [ ] Download from: https://dotnet.microsoft.com/download/dotnet/9.0
- [ ] Select "Hosting Bundle"
- [ ] Run installer
- [ ] Run: `iisreset` in PowerShell

---

### Part 2: Extract and Configure Files

#### Extract Files
- [ ] Create folder: `C:\inetpub\wwwroot\fsh-blazor\`
- [ ] Right-click `FSH.Starter.Blazor.zip` → Extract All
- [ ] Extract to: `C:\inetpub\wwwroot\fsh-blazor\`
- [ ] Verify files exist:
  - [ ] `index.html`
  - [ ] `appsettings.json`
  - [ ] `web.config`
  - [ ] `_framework\` folder with 300+ files

#### Configure API URL
- [ ] Open: `C:\inetpub\wwwroot\fsh-blazor\appsettings.json`
- [ ] Change `ApiBaseUrl` to your production API
- [ ] Example: `"ApiBaseUrl": "https://api.yourdomain.com/"`
- [ ] Save file

#### Set Permissions
- [ ] Right-click `C:\inetpub\wwwroot\fsh-blazor\` → Properties
- [ ] Security tab → Edit → Add
- [ ] Add `IIS_IUSRS` with Read & Execute
- [ ] Add `IUSR` with Read & Execute
- [ ] Apply → OK

---

### Part 3: Configure IIS

#### Create Application Pool
- [ ] Open IIS Manager (Run: `inetmgr`)
- [ ] Application Pools → Right-click → Add Application Pool
- [ ] Name: `FSH.Blazor.AppPool`
- [ ] .NET CLR Version: **No Managed Code** ⚠️ IMPORTANT!
- [ ] Managed Pipeline Mode: Integrated
- [ ] Click OK

#### Create Website
- [ ] In IIS Manager, expand server name
- [ ] Right-click Sites → Add Website
- [ ] Configuration:
  ```
  Site name:          FSH Blazor Client
  Application pool:   FSH.Blazor.AppPool
  Physical path:      C:\inetpub\wwwroot\fsh-blazor
  Binding Type:       http
  Port:               80 (or 8080)
  Host name:          (leave blank or enter domain)
  ```
- [ ] Click OK

#### Enable Compression
- [ ] Click your site in IIS Manager
- [ ] Double-click "Compression"
- [ ] Check both:
  - [ ] Enable dynamic content compression
  - [ ] Enable static content compression
- [ ] Click Apply

---

### Part 4: Testing

#### Start Website
- [ ] In IIS Manager, select your site
- [ ] Click "Browse *:80 (http)" in right pane
- [ ] Browser should open with your Blazor app

#### Check for Errors
- [ ] Press F12 in browser (Developer Tools)
- [ ] Go to Console tab
- [ ] Verify no red errors
- [ ] Should see: "Loading Blazor WebAssembly..."

#### Test Routing
- [ ] Navigate to different pages in your app
- [ ] Press F5 to refresh page
- [ ] Should reload correctly (not 404)
- [ ] If 404: URL Rewrite module not installed

#### Test API Connection
- [ ] Try logging in (if auth is implemented)
- [ ] Try fetching data from API
- [ ] Check Network tab for API calls
- [ ] If fails: Check CORS on API, verify API URL

---

## 🔥 Troubleshooting Quick Fixes

### White Screen / No Load
- [ ] Check browser console (F12)
- [ ] Verify all files extracted
- [ ] Check `appsettings.json` API URL

### 404 on Routes (Page Refresh)
- [ ] Install URL Rewrite Module
- [ ] Verify `web.config` exists
- [ ] Run: `iisreset`

### API Calls Fail
- [ ] Verify API is running
- [ ] Check API CORS settings
- [ ] Verify `appsettings.json` API URL
- [ ] Check firewall rules

### Slow Loading
- [ ] Enable compression in IIS
- [ ] Check Network tab: should see `.br` or `.gz` files
- [ ] Verify HTTP/2 enabled

### 500.19 Error
- [ ] Install URL Rewrite Module
- [ ] Check `web.config` syntax
- [ ] Run: `iisreset`

---

## 🎯 Success Criteria

Your deployment is complete when ALL are checked:

- [ ] ✅ Website loads without errors
- [ ] ✅ All pages work (Products, Login, etc.)
- [ ] ✅ Page refresh works (no 404)
- [ ] ✅ API calls successful
- [ ] ✅ Authentication works
- [ ] ✅ No browser console errors
- [ ] ✅ Files are compressed (check Network tab)
- [ ] ✅ Accessible from other computers
- [ ] ✅ Performance is acceptable
- [ ] ✅ HTTPS configured (production only)

---

## 📞 Need Help?

### Check These:
1. Browser Console (F12) for JavaScript errors
2. IIS Manager → Your Site → Failed Request Tracing
3. Event Viewer → Windows Logs → Application
4. `C:\inetpub\logs\LogFiles\` for IIS logs

### Common Commands:
```powershell
# Restart IIS
iisreset

# Stop site
Stop-Website -Name "FSH Blazor Client"

# Start site
Start-Website -Name "FSH Blazor Client"

# View recent logs
Get-Content "C:\inetpub\logs\LogFiles\W3SVC1\*.log" -Tail 50
```

---

## 📚 Documentation Files

- `BLAZOR_IIS_DEPLOYMENT_GUIDE.md` - Full detailed guide
- `BLAZOR_PUBLISH_VERIFICATION.md` - Verify your publish is complete
- `BLAZOR_WASM_DLL_EXPLANATION.md` - Understanding .wasm files
- `MAKEFILE_IIS_DEPLOYMENT.md` - Makefile usage guide

---

**Deployment Date**: _______________
**Server IP**: _______________
**Domain**: _______________
**API URL**: _______________

**Deployed By**: _______________
**Notes**: 
```
_______________________________________________
_______________________________________________
_______________________________________________
```

---

✅ **DEPLOYMENT COMPLETE!** 🚀

Print Date: October 28, 2025

