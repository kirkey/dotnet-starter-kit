# IIS Deployment Package

This folder contains everything you need to deploy the FSH Starter Kit API and Blazor Client to an IIS server.

## 📁 Files Overview

| File | Description |
|------|-------------|
| `IIS_DEPLOYMENT_GUIDE.md` | **Comprehensive deployment guide** - Detailed step-by-step instructions |
| `IIS_QUICK_START.md` | **Quick reference guide** - Common commands and quick troubleshooting |
| `publish-for-iis.ps1` | PowerShell script to build deployment packages (Windows) |
| `publish-for-iis.sh` | Bash script to build deployment packages (macOS/Linux) |
| `deploy-to-iis.ps1` | PowerShell script for automated IIS deployment (run on server) |
| `api/server/web.config` | IIS configuration for the API |
| `apps/blazor/client/wwwroot/web.config` | IIS configuration for the Blazor client |
| `api/server/appsettings.Production.json` | Production settings template for API |
| `apps/blazor/client/wwwroot/appsettings.Production.json` | Production settings template for Blazor |

## 🚀 Quick Start

### 1. Build Deployment Packages

**On your Mac:**
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
./publish-for-iis.sh
```

**On Windows:**
```powershell
cd \path\to\dotnet-starter-kit\src
.\publish-for-iis.ps1
```

This creates a `./deploy` folder with:
- `FSH.Starter.API_[timestamp].zip` - API package
- `FSH.Starter.Blazor_[timestamp].zip` - Blazor package
- `api/` - Uncompressed API files
- `blazor/` - Uncompressed Blazor files

### 2. Configure Production Settings

**Update API settings** before deploying:
Edit `api/server/appsettings.Production.json`:
- Database connection string
- JWT secret key
- CORS allowed origins
- Mail settings
- Disable Swagger for production

**Update Blazor settings**:
Edit `apps/blazor/client/wwwroot/appsettings.Production.json`:
- Set `ApiBaseUrl` to your API domain

### 3. Transfer to IIS Server

Copy these files to your Windows Server:
- The generated ZIP files from step 1
- `deploy-to-iis.ps1` (optional, for automated deployment)
- Your updated configuration files

### 4. Deploy on IIS Server

**Prerequisites on the server:**
1. Install [.NET 9.0 Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/9.0)
2. Install [URL Rewrite Module](https://www.iis.net/downloads/microsoft/url-rewrite)
3. Restart IIS: `iisreset`

**Deploy using the script** (run as Administrator):
```powershell
.\deploy-to-iis.ps1 `
    -ApiPackagePath ".\FSH.Starter.API_20251026_120000.zip" `
    -BlazorPackagePath ".\FSH.Starter.Blazor_20251026_120000.zip" `
    -ApiHostname "api.yourdomain.com" `
    -BlazorHostname "app.yourdomain.com" `
    -CertificateThumbprint "YOUR_SSL_CERT_THUMBPRINT"
```

**Or deploy manually** following `IIS_DEPLOYMENT_GUIDE.md`

### 5. Test Your Deployment

- API: `https://api.yourdomain.com/swagger` (if enabled)
- Blazor: `https://app.yourdomain.com`

## 📚 Documentation

- **New to IIS deployment?** Start with `IIS_DEPLOYMENT_GUIDE.md`
- **Need quick reference?** Check `IIS_QUICK_START.md`
- **Having issues?** See troubleshooting sections in both guides

## 🔧 Common Deployment Scenarios

### Scenario 1: First Time Deployment
1. Follow the complete guide in `IIS_DEPLOYMENT_GUIDE.md`
2. Don't skip the prerequisites section
3. Test thoroughly after deployment

### Scenario 2: Update Existing Deployment
1. Build new packages with the publish script
2. Stop the application pool
3. Replace files in `C:\inetpub\wwwroot\FSH.Starter.API` (or your path)
4. Start the application pool
5. Test the update

### Scenario 3: Deploy Behind Reverse Proxy
If deploying behind nginx, Apache, or another reverse proxy:
- Configure appropriate forwarded headers
- Update CORS settings
- Ensure WebSocket support if needed

## 🔐 Security Checklist

Before going to production:
- [ ] SSL/TLS certificates installed and configured
- [ ] Strong JWT secret key configured
- [ ] Database credentials secured
- [ ] CORS restricted to your domain only
- [ ] Swagger disabled in production
- [ ] File upload limits configured
- [ ] Application pool running with least privileges
- [ ] Windows Firewall configured
- [ ] Rate limiting configured
- [ ] Security headers configured

## 📊 Monitoring

After deployment, monitor:
- **Application Logs**: `C:\inetpub\wwwroot\FSH.Starter.API\logs`
- **IIS Logs**: `C:\inetpub\logs\LogFiles`
- **Windows Event Viewer**: Application and System logs
- **Performance Counters**: CPU, Memory, Request rate

## 🆘 Getting Help

If you encounter issues:

1. **Check the logs first**:
   ```powershell
   Get-Content C:\inetpub\wwwroot\FSH.Starter.API\logs\stdout_*.log -Tail 50
   ```

2. **Verify prerequisites**:
   ```powershell
   dotnet --list-runtimes
   ```

3. **Check IIS status**:
   ```powershell
   Get-IISAppPool
   Get-IISSite
   ```

4. **Review troubleshooting sections** in the guides

5. **Common fixes**:
   - Restart IIS: `iisreset`
   - Restart app pool: `Restart-WebAppPool -Name "FSH.Starter.API.Pool"`
   - Check permissions: Grant IIS_IUSRS full control to application folder
   - Verify database connection: Test connection string
   - Check CORS: Update allowed origins in appsettings.json

## 📝 Notes

- The API requires .NET 9.0 Runtime
- The Blazor client is a WebAssembly app (static files)
- Both need separate IIS sites or applications
- SSL is highly recommended for production
- The `web.config` files are pre-configured for IIS
- Application pool must be set to "No Managed Code"

## 🔄 CI/CD Integration

For automated deployments, integrate the publish scripts into your CI/CD pipeline:

**GitHub Actions example**:
```yaml
- name: Publish API
  run: dotnet publish api/server/Server.csproj -c Release -o ./deploy/api

- name: Publish Blazor
  run: dotnet publish apps/blazor/client/Client.csproj -c Release -o ./deploy/blazor/temp
```

**Azure DevOps example**:
```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'publish'
    projects: 'api/server/Server.csproj'
    arguments: '-c Release -o $(Build.ArtifactStagingDirectory)/api'
```

## 📞 Support Contacts

- Technical Issues: Check documentation first
- Server Access: Contact your IT department
- SSL Certificates: Contact your certificate provider
- DNS Configuration: Contact your domain registrar

---

**Ready to deploy?** Start with `IIS_QUICK_START.md` for a streamlined process or `IIS_DEPLOYMENT_GUIDE.md` for comprehensive instructions.

Good luck with your deployment! 🚀

