# Makefile IIS Deployment Guide

## Overview
The Makefile has been updated to properly build and package your .NET application for Windows Server IIS deployment.

## Key Updates

### 1. **Removed Runtime Identifiers**
- Removed `PUBLISH_RUNTIME` variable
- API now uses framework-dependent deployment (requires .NET 9 Hosting Bundle on IIS)
- Blazor WebAssembly uses `browser-wasm` runtime automatically (handled by SDK)

### 2. **Proper Blazor Publishing**
- Publishes to temp folder first
- Extracts only `wwwroot` content for IIS static file hosting
- Removes temporary files after extraction

### 3. **New Targets**

#### `make publish-all`
Publishes both API and Blazor client, then creates ZIP deployment packages.

#### `make publish-api`
Publishes the API server to `./publishfsh9/api`

#### `make publish-blazor`
Publishes the Blazor WebAssembly client to `./publishfsh9/blazor`

#### `make create-deployment-packages`
Creates ZIP files from published output:
- `FSH.Starter.API.zip`
- `FSH.Starter.Blazor.zip`

#### `make clean-publish`
Removes the publish output folder

## Usage

### Full Deployment
```bash
make publish-all
```

This will:
1. Publish API (framework-dependent, no self-contained)
2. Publish Blazor client (wwwroot only)
3. Create ZIP packages for easy transfer to Windows Server
4. Display deployment instructions

### Individual Publishing

Publish only API:
```bash
make publish-api
```

Publish only Blazor:
```bash
make publish-blazor
```

### Customization

Change output directory:
```bash
make publish-all PUBLISH_OUTPUT=./my-custom-output
```

Use Debug configuration:
```bash
make publish-all PUBLISH_CONFIG=Debug
```

## Published Structure

### API Output (`./publishfsh9/api/`)
```
api/
├── appsettings.json
├── appsettings.Production.json
├── appsettings.Development.json
├── FSH.Starter.WebApi.Host.dll
├── FSH.Starter.WebApi.Host.exe  (startup executable)
├── web.config  (IIS configuration)
├── [all dependencies DLLs]
└── wwwroot/
```

### Blazor Output (`./publishfsh9/blazor/`)
```
blazor/
├── index.html
├── appsettings.json  (IMPORTANT: Configure API URL here)
├── appsettings.Production.json
├── favicon.png
├── service-worker.js
├── manifest.webmanifest
├── web.config  (URL rewrite rules for SPA routing)
├── css/
├── js/
├── _framework/
│   ├── blazor.boot.json
│   ├── blazor.webassembly.js
│   ├── dotnet.*.wasm
│   ├── [all .dll files compressed as .wasm]
│   └── [all dependencies]
└── _content/
```

## IIS Deployment Checklist

### Prerequisites on Windows Server
- [ ] Windows Server 2016 or later
- [ ] IIS installed
- [ ] .NET 9.0 Hosting Bundle installed
- [ ] URL Rewrite Module for IIS installed

### API Deployment
1. Extract `FSH.Starter.API.zip` to IIS folder (e.g., `C:\inetpub\wwwroot\fsh-api`)
2. Update `appsettings.Production.json`:
   - Database connection string
   - JWT settings
   - CORS settings (allow Blazor client URL)
3. Create IIS Application Pool:
   - .NET CLR Version: **No Managed Code**
   - Managed Pipeline Mode: Integrated
4. Create IIS Site:
   - Physical path: API folder
   - Application Pool: The one created above
   - Binding: HTTP/HTTPS with appropriate port

### Blazor Deployment
1. Extract `FSH.Starter.Blazor.zip` to IIS folder (e.g., `C:\inetpub\wwwroot\fsh-blazor`)
2. **CRITICAL**: Update `appsettings.json`:
   ```json
   {
     "ApiBaseUrl": "https://your-api-domain.com"
   }
   ```
3. Ensure `web.config` has URL rewrite rules for SPA routing
4. Create IIS Application Pool:
   - .NET CLR Version: **No Managed Code**
   - Managed Pipeline Mode: Integrated
5. Create IIS Site:
   - Physical path: Blazor folder
   - Application Pool: The one created above
   - Binding: HTTP/HTTPS with appropriate port

### Post-Deployment
1. Restart IIS: `iisreset`
2. Test API: `https://your-api-domain.com/swagger`
3. Test Blazor: `https://your-blazor-domain.com`

## Troubleshooting

### Blazor doesn't load
- Check browser console for errors
- Verify `appsettings.json` has correct API URL
- Check that all files extracted properly from ZIP
- Verify URL Rewrite module is installed in IIS

### API doesn't start
- Check Event Viewer > Windows Logs > Application
- Verify .NET 9 Hosting Bundle is installed
- Check `appsettings.Production.json` for correct configuration
- Verify Application Pool is set to "No Managed Code"

### 502.5 Error
- .NET Hosting Bundle not installed
- Wrong Application Pool settings
- Check `web.config` for correct configuration

## File Counts

A complete Blazor publish typically has:
- 200+ files in `_framework/` directory
- Both `.dll` and `.wasm` files
- Compressed versions (`.br`, `.gz`) of most files

If your `_framework` folder has significantly fewer files, the publish may be incomplete.

## Notes

- **Framework-Dependent Deployment**: API requires .NET 9 Runtime on server
- **Blazor WebAssembly**: Client-side only, no server runtime needed
- **Compression**: All static files include Brotli and Gzip versions
- **Service Worker**: PWA support enabled by default
- **appsettings.json**: Must be configured AFTER extraction on server

## References

- [IIS_DEPLOYMENT_GUIDE.md](IIS_DEPLOYMENT_GUIDE.md) - Detailed deployment steps
- [publish-for-iis.ps1](publish-for-iis.ps1) - PowerShell equivalent script
- [web.config](api/server/web.config) - IIS configuration reference

