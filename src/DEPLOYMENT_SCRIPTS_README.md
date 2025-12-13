# Deployment Scripts for FSH Starter Kit

This directory contains scripts to automate the deployment process for the FSH Starter Kit to IIS servers.

## Files

- **publish-for-iis.sh** - Bash script for macOS/Linux users
- **publish-for-iis.bat** - Batch script for Windows users  
- **IIS_DEPLOYMENT_GUIDE.md** - Comprehensive IIS deployment instructions

## Quick Start

### For macOS/Linux Users

```bash
cd /path/to/dotnet-starter-kit/src
chmod +x publish-for-iis.sh
./publish-for-iis.sh
```

### For Windows Users

```cmd
cd C:\path\to\dotnet-starter-kit\src
publish-for-iis.bat
```

## What Gets Generated

The scripts produce:
1. **API Package** - Self-contained .NET 9 Windows executable with all dependencies
2. **Blazor Client Package** - Pre-compiled static Blazor WebAssembly files
3. **Deployment ZIP files** - Ready to transfer to your IIS server

## Key Changes in These Scripts

### ✅ Correctly Configured for IIS

- **Self-contained deployment** (`--self-contained`) - Includes .NET runtime
- **Windows 64-bit target** (`-r win-x64`) - Optimized for Windows servers
- **Generates .exe executable** - Can be run directly by IIS
- **Optimized size** - Debug symbols removed (`/p:DebugType=none`)

### ❌ What NOT To Do

- ❌ DO NOT use `--no-self-contained` - Requires .NET installed on server
- ❌ DO NOT publish for Linux/ARM - This is for Windows IIS only
- ❌ DO NOT skip the Windows hosting bundle installation on the server

## Output Structure

```
deploy/
├── FSH.Starter.API_[timestamp].zip
│   ├── FSH.Starter.WebApi.Host.exe      ← Main executable
│   ├── appsettings.json                 ← Configuration file
│   ├── logs/                            ← Log directory
│   └── ... (all runtime files)
│
└── FSH.Starter.Blazor_[timestamp].zip
    ├── index.html
    ├── app.css
    ├── _framework/                      ← Blazor WebAssembly files
    └── ... (static web files)
```

## Next Steps

1. **Run the publish script** from your development machine
2. **Follow IIS_DEPLOYMENT_GUIDE.md** for detailed IIS configuration
3. **Transfer the ZIP files** to your Windows IIS server
4. **Extract and configure** using the guide's PowerShell commands

## Requirements

### Development Machine
- .NET 9 SDK
- bash (macOS/Linux) or cmd/PowerShell (Windows)
- Approximately 2-3 GB free disk space for build output

### IIS Server
- Windows Server 2012 R2 or later
- IIS 8.5 or higher
- .NET 9 ASP.NET Core Hosting Bundle installed
  - Download: https://dotnet.microsoft.com/download/dotnet/9.0
- SQL Server or compatible database

## Troubleshooting

### Script Won't Run
```bash
# Ensure script is executable (macOS/Linux)
chmod +x publish-for-iis.sh
```

### No .exe in output
This means you're likely running without the `--self-contained` flag. The scripts have been updated to include this. Make sure you're using the latest version.

### Build fails
- Ensure .NET 9 SDK is installed: `dotnet --version`
- Clean build: `dotnet clean` in the src/api/server directory
- Check that all project references exist

## Performance Notes

First-time publish takes 2-5 minutes (larger due to .NET runtime embedding). Subsequent publishes are faster as files are cached.

## Support

For detailed deployment instructions, see **IIS_DEPLOYMENT_GUIDE.md**

