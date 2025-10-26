# FSH Starter Kit - IIS Deployment Quick Start

## Prerequisites Checklist

### On IIS Server:
- [ ] Windows Server installed
- [ ] IIS installed and running
- [ ] .NET 9.0 Hosting Bundle installed: https://dotnet.microsoft.com/download/dotnet/9.0
- [ ] URL Rewrite Module installed: https://www.iis.net/downloads/microsoft/url-rewrite
- [ ] SQL Server or database accessible
- [ ] SSL certificates ready (for production)

### Verify Installation:
```powershell
# Check .NET is installed
dotnet --list-runtimes

# Should show something like:
# Microsoft.AspNetCore.App 9.x.x [path]
# Microsoft.NETCore.App 9.x.x [path]

# After installing, restart IIS
iisreset
```

## Quick Deployment Steps

### Step 1: Build Deployment Packages

**On macOS/Linux:**
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
chmod +x publish-for-iis.sh
./publish-for-iis.sh
```

**On Windows:**
```powershell
cd \path\to\dotnet-starter-kit\src
.\publish-for-iis.ps1
```

This creates ZIP files in the `./deploy` folder.

### Step 2: Configure Production Settings

**Before deploying, update these files:**

1. **API Configuration** (`api/server/appsettings.json`):
```json
{
  "DatabaseSettings": {
    "ConnectionString": "YOUR_PRODUCTION_CONNECTION_STRING"
  },
  "CorsSettings": {
    "AllowedOrigins": ["https://your-blazor-domain.com"]
  }
}
```

2. **Blazor Configuration** (`apps/blazor/client/wwwroot/appsettings.json`):
```json
{
  "ApiBaseUrl": "https://your-api-domain.com"
}
```

### Step 3: Transfer to IIS Server

Transfer these files to your Windows Server:
- `FSH.Starter.API_[timestamp].zip`
- `FSH.Starter.Blazor_[timestamp].zip`
- `deploy-to-iis.ps1` (optional, for automated deployment)

### Step 4: Deploy to IIS

**Option A: Manual Deployment**

1. Create API application pool and website
2. Create Blazor application pool and website
3. Extract files to respective directories
4. Configure bindings and SSL

See `IIS_DEPLOYMENT_GUIDE.md` for detailed manual steps.

**Option B: Automated Deployment (on IIS Server)**

```powershell
# Run PowerShell as Administrator
cd C:\path\to\deployment\files

.\deploy-to-iis.ps1 `
    -ApiPackagePath ".\FSH.Starter.API_20251026_120000.zip" `
    -BlazorPackagePath ".\FSH.Starter.Blazor_20251026_120000.zip" `
    -ApiHostname "api.yourdomain.com" `
    -BlazorHostname "app.yourdomain.com" `
    -CertificateThumbprint "YOUR_CERT_THUMBPRINT"
```

### Step 5: Configure DNS

Add DNS records pointing to your IIS server:
- `api.yourdomain.com` → Your server IP
- `app.yourdomain.com` → Your server IP

### Step 6: Test

1. **Test API:**
   - Browse to: `https://api.yourdomain.com/swagger`
   - Check health endpoint

2. **Test Blazor:**
   - Browse to: `https://app.yourdomain.com`
   - Verify it loads and can connect to API

## Troubleshooting

### API won't start (500 errors)
```powershell
# Check logs
Get-Content C:\inetpub\wwwroot\FSH.Starter.API\logs\stdout_*.log

# Verify .NET runtime
dotnet --list-runtimes

# Check app pool is running
Get-IISAppPool | Where-Object {$_.Name -like "*FSH*"}

# Restart app pool
Restart-WebAppPool -Name "FSH.Starter.API.Pool"
```

### Blazor won't load
1. Check browser console for errors
2. Verify `appsettings.json` has correct API URL
3. Check CORS settings in API
4. Verify web.config has URL rewrite rules

### Database connection issues
1. Check connection string in `appsettings.json`
2. Verify app pool identity has database access
3. Test connection from server:
```powershell
Test-NetConnection -ComputerName your-db-server -Port 1433
```

### CORS errors
Update API `appsettings.json`:
```json
{
  "CorsSettings": {
    "AllowedOrigins": [
      "https://app.yourdomain.com",
      "https://localhost:5001"
    ],
    "AllowCredentials": true
  }
}
```

## Common Commands

```powershell
# Restart IIS completely
iisreset

# Restart specific app pool
Restart-WebAppPool -Name "FSH.Starter.API.Pool"

# View running sites
Get-IISSite

# View app pools
Get-IISAppPool

# Check logs
Get-Content C:\inetpub\logs\LogFiles\W3SVC*\*.log -Tail 50

# Grant permissions to folder
icacls "C:\inetpub\wwwroot\FSH.Starter.API" /grant "IIS_IUSRS:(OI)(CI)F" /T
```

## File Locations on IIS Server

| Component | Default Location |
|-----------|------------------|
| API Files | `C:\inetpub\wwwroot\FSH.Starter.API` |
| Blazor Files | `C:\inetpub\wwwroot\FSH.Starter.Blazor` |
| API Logs | `C:\inetpub\wwwroot\FSH.Starter.API\logs` |
| IIS Logs | `C:\inetpub\logs\LogFiles` |
| API Config | `C:\inetpub\wwwroot\FSH.Starter.API\appsettings.json` |
| Blazor Config | `C:\inetpub\wwwroot\FSH.Starter.Blazor\appsettings.json` |

## SSL Certificate Setup

### Get certificate thumbprint:
```powershell
Get-ChildItem -Path Cert:\LocalMachine\My | Format-List Subject, Thumbprint
```

### Bind certificate to site:
```powershell
$cert = Get-ChildItem -Path Cert:\LocalMachine\My | Where-Object {$_.Thumbprint -eq "YOUR_THUMBPRINT"}
$binding = Get-WebBinding -Name "FSH.Starter.API" -Protocol "https"
$binding.AddSslCertificate($cert.Thumbprint, "my")
```

## Update Deployment

To update an existing deployment:

1. Build new packages with `publish-for-iis.ps1` or `.sh`
2. Stop the app pool:
   ```powershell
   Stop-WebAppPool -Name "FSH.Starter.API.Pool"
   ```
3. Extract new files over existing ones
4. Start the app pool:
   ```powershell
   Start-WebAppPool -Name "FSH.Starter.API.Pool"
   ```

## Security Checklist

- [ ] SSL/TLS certificates configured
- [ ] HTTP to HTTPS redirect enabled
- [ ] CORS properly configured (not allowing *)
- [ ] Database connection uses secure credentials
- [ ] Application secrets encrypted or in Azure Key Vault
- [ ] File upload size limits configured
- [ ] Rate limiting enabled
- [ ] Security headers configured
- [ ] Windows Firewall rules configured

## Performance Tips

1. **Enable Response Compression** in API
2. **Enable Output Caching** where appropriate
3. **Use HTTP/2** in IIS
4. **Configure CDN** for Blazor static assets
5. **Enable Brotli Compression**
6. **Configure app pool recycling** appropriately

## Support

For detailed instructions, see: `IIS_DEPLOYMENT_GUIDE.md`

For issues:
1. Check application logs
2. Check IIS logs
3. Check Windows Event Viewer
4. Enable detailed errors temporarily (remove in production!)
#!/bin/bash
# Bash Script to Publish API and Blazor Client
# Run this script from your development machine (macOS/Linux)

set -e

CONFIGURATION="Release"
OUTPUT_PATH="./deploy"

echo "====================================="
echo "FSH Starter Kit - Deployment Builder"
echo "====================================="
echo ""

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_PATH="$SCRIPT_DIR"

# Define paths
API_PROJECT_PATH="$ROOT_PATH/api/server"
BLAZOR_PROJECT_PATH="$ROOT_PATH/apps/blazor/client"
API_OUTPUT_PATH="$OUTPUT_PATH/api"
BLAZOR_OUTPUT_PATH="$OUTPUT_PATH/blazor"

# Create output directory
echo "Creating output directory at: $OUTPUT_PATH"
rm -rf "$OUTPUT_PATH"
mkdir -p "$OUTPUT_PATH"
mkdir -p "$API_OUTPUT_PATH"
mkdir -p "$BLAZOR_OUTPUT_PATH"

# Publish API
echo ""
echo "Publishing API..."
echo "Project: $API_PROJECT_PATH"
echo "Output: $API_OUTPUT_PATH"

cd "$API_PROJECT_PATH"
dotnet publish -c $CONFIGURATION -o "$API_OUTPUT_PATH" --no-self-contained
if [ $? -eq 0 ]; then
    echo "✓ API published successfully"
else
    echo "✗ API publish failed"
    exit 1
fi
cd "$ROOT_PATH"

# Publish Blazor Client
echo ""
echo "Publishing Blazor Client..."
echo "Project: $BLAZOR_PROJECT_PATH"
echo "Output: $BLAZOR_OUTPUT_PATH"

cd "$BLAZOR_PROJECT_PATH"
BLAZOR_TEMP="$BLAZOR_OUTPUT_PATH/temp"
dotnet publish -c $CONFIGURATION -o "$BLAZOR_TEMP"
if [ $? -eq 0 ]; then
    # Copy wwwroot content to the output
    cp -r "$BLAZOR_TEMP/wwwroot/"* "$BLAZOR_OUTPUT_PATH/"
    rm -rf "$BLAZOR_TEMP"
    echo "✓ Blazor client published successfully"
else
    echo "✗ Blazor publish failed"
    exit 1
fi
cd "$ROOT_PATH"

# Create deployment packages
echo ""
echo "Creating deployment packages..."

TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
API_ZIP_NAME="FSH.Starter.API_$TIMESTAMP.zip"
BLAZOR_ZIP_NAME="FSH.Starter.Blazor_$TIMESTAMP.zip"

cd "$OUTPUT_PATH"

# Compress API
cd api
zip -r "../$API_ZIP_NAME" .
cd ..
echo "✓ API package created: $API_ZIP_NAME"

# Compress Blazor
cd blazor
zip -r "../$BLAZOR_ZIP_NAME" .
cd ..
echo "✓ Blazor package created: $BLAZOR_ZIP_NAME"

cd "$ROOT_PATH"

# Summary
echo ""
echo "====================================="
echo "Deployment packages ready!"
echo "====================================="
echo ""
echo "Deployment packages location:"
echo "  API Package: $OUTPUT_PATH/$API_ZIP_NAME"
echo "  Blazor Package: $OUTPUT_PATH/$BLAZOR_ZIP_NAME"
echo ""
echo "Uncompressed folders:"
echo "  API: $API_OUTPUT_PATH"
echo "  Blazor: $BLAZOR_OUTPUT_PATH"
echo ""
echo "Next Steps:"
echo "1. Transfer the packages to your IIS server"
echo "2. Follow the IIS_DEPLOYMENT_GUIDE.md for deployment instructions"
echo "3. Update appsettings.json on the server with production settings"
echo ""

