# IIS Deployment - Troubleshooting Container Build Error

## Problem

When running the `publish-for-iis` scripts, you may encounter this error:

```
error : Cannot find docker/podman executable.
error CONTAINER1012: The local registry is not available, but pushing to a local registry was requested.
```

This happens because the Server.csproj has container publishing configured, but Docker is not installed or available.

## Solution Options

### Option 1: Use the Updated Scripts (Recommended)

The `publish-for-iis.ps1` and `publish-for-iis.sh` scripts have been updated to disable container publishing.

They now use: `/p:PublishProfile="" /p:EnableSdkContainerSupport=false`

### Option 2: Manual Publish Commands

If the scripts still have issues, use these commands directly:

**For API:**
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/server
dotnet publish -c Release -o ../../deploy/api --no-self-contained /p:PublishProfile="" /p:EnableSdkContainerSupport=false
```

**For Blazor:**
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/client
dotnet publish -c Release -o ../../deploy/blazor-temp

# Then copy just the wwwroot content
cp -r ../../deploy/blazor-temp/wwwroot/* ../../deploy/blazor/
rm -rf ../../deploy/blazor-temp
```

**Create ZIP files:**
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/deploy

# API
cd api
zip -r ../FSH.Starter.API_$(date +%Y%m%d_%H%M%S).zip .
cd ..

# Blazor
cd blazor
zip -r ../FSH.Starter.Blazor_$(date +%Y%m%d_%H%M%S).zip .
cd ..
```

### Option 3: Temporarily Modify Server.csproj

You can comment out the container properties in `Server.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <RootNamespace>FSH.Starter.WebApi.Host</RootNamespace>
    <AssemblyName>FSH.Starter.WebApi.Host</AssemblyName>
    <!-- Temporarily comment out for IIS deployment -->
    <!--
    <ContainerUser>root</ContainerUser>
  </PropertyGroup>
  <PropertyGroup>
    <ContainerRepository>webapi</ContainerRepository>
    <PublishProfile>DefaultContainer</PublishProfile>
    -->
  </PropertyGroup>
  <!-- rest of file... -->
</Project>
```

**Remember to uncomment these when you need container builds again!**

### Option 4: Create Deployment Directory Structure Manually

If all else fails, create the structure manually:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src

# Create directories
mkdir -p deploy/api
mkdir -p deploy/blazor

# Publish API (without container)
cd api/server
dotnet publish -c Release -o ../../deploy/api --no-self-contained

# Publish Blazor
cd ../../apps/blazor/client
dotnet publish -c Release -o ../../deploy/blazor-temp
cp -r ../../deploy/blazor-temp/wwwroot/* ../../deploy/blazor/
rm -rf ../../deploy/blazor-temp

# Go back to root
cd ../../

# Create packages
cd deploy
tar -czf FSH.Starter.API_$(date +%Y%m%d_%H%M%S).tar.gz -C api .
tar -czf FSH.Starter.Blazor_$(date +%Y%m%d_%H%M%S).tar.gz -C blazor .
```

## Step-by-Step Manual Deployment

If you want to skip the automated scripts entirely:

### 1. Publish the API

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src
mkdir -p deploy/api

cd api/server
dotnet publish -c Release -o ../../deploy/api --no-self-contained \
  /p:PublishProfile="" \
  /p:EnableSdkContainerSupport=false \
  /p:ContainerRepository="" \
  /p:ContainerImageName=""

cd ../..
```

### 2. Verify API Files

```bash
ls -la deploy/api/FSH.Starter.WebApi.Host.dll
ls -la deploy/api/web.config
ls -la deploy/api/appsettings.json
```

You should see the main DLL, web.config, and appsettings files.

### 3. Publish the Blazor Client

```bash
mkdir -p deploy/blazor

cd apps/blazor/client
dotnet publish -c Release -o ../../../deploy/blazor-temp

# Copy only wwwroot content
cp -r ../../../deploy/blazor-temp/wwwroot/* ../../../deploy/blazor/
rm -rf ../../../deploy/blazor-temp

cd ../../..
```

### 4. Verify Blazor Files

```bash
ls -la deploy/blazor/index.html
ls -la deploy/blazor/_framework/
ls -la deploy/blazor/appsettings.json
```

### 5. Create ZIP Archives

```bash
cd deploy

# API
cd api
zip -r ../FSH.Starter.API_manual.zip .
cd ..

# Blazor
cd blazor
zip -r ../FSH.Starter.Blazor_manual.zip .
cd ..

echo "✓ Packages created:"
ls -lh *.zip
```

### 6. Update Configuration Files

Before transferring to IIS:

**API - Update `deploy/api/appsettings.json`:**
```json
{
  "DatabaseSettings": {
    "ConnectionString": "YOUR_PRODUCTION_CONNECTION_STRING"
  },
  "CorsSettings": {
    "AllowedOrigins": ["https://your-blazor-domain.com"]
  },
  "JwtSettings": {
    "Key": "YOUR_STRONG_SECRET_KEY_32_CHARS_MINIMUM"
  }
}
```

**Blazor - Update `deploy/blazor/appsettings.json`:**
```json
{
  "ApiBaseUrl": "https://your-api-domain.com"
}
```

### 7. Transfer to IIS Server

Copy these files to your Windows Server:
- `deploy/FSH.Starter.API_manual.zip`
- `deploy/FSH.Starter.Blazor_manual.zip`

Then follow the **IIS_DEPLOYMENT_GUIDE.md** for IIS setup.

## Quick Test

To verify your publish worked locally before transferring:

```bash
# Check API
ls -la deploy/api/*.dll | wc -l
# Should show many DLL files

# Check Blazor
ls -la deploy/blazor/_framework/*.dll | wc -l
# Should show many DLL files

# Check sizes
du -sh deploy/api
du -sh deploy/blazor
```

## Common Issues

### Issue: "Cannot find executable"
**Solution:** Use the `/p:EnableSdkContainerSupport=false` flag

### Issue: "Access denied" when creating directories
**Solution:** Ensure you have write permissions in the src directory

### Issue: Build warnings about CA1515
**Solution:** These are code analysis warnings and won't prevent deployment. They can be ignored for now.

### Issue: Very slow build times
**Solution:** Use `-c Release` instead of Debug, and ensure no virus scanner is scanning the build output

## Next Steps

Once you have the `deploy` folder with `api` and `blazor` subdirectories:

1. Zip them up (manually or with commands above)
2. Transfer to your Windows Server
3. Follow **IIS_DEPLOYMENT_GUIDE.md** starting from "Deploy to IIS" section
4. Deploy using **deploy-to-iis.ps1** script or manually through IIS Manager

## Alternative: Use PowerShell on Windows

If you have access to a Windows machine, you can transfer your code there and use the PowerShell script (`publish-for-iis.ps1`) which generally works better on Windows.

## Support

If you continue to have issues:
1. Check that .NET 9 SDK is properly installed: `dotnet --version`
2. Try restoring packages first: `dotnet restore`
3. Build before publishing: `dotnet build -c Release`
4. Then publish with the commands above

---

**Remember:** The goal is to get two folders:
- `deploy/api` with your ASP.NET Core API files
- `deploy/blazor` with your Blazor WebAssembly files

Once you have these, you can deploy to IIS!

