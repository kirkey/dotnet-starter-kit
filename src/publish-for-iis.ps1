# PowerShell Script to Publish API and Blazor Client for Windows IIS Deployment
# Run this script from your development machine with Administrator privileges (recommended)
# Usage: .\publish-for-iis.ps1 -Configuration Release -OutputPath "./deploy"

param(
    [string]$Configuration = "Release",
    [string]$OutputPath = "./deploy",
    [switch]$CreateWebConfig = $false,
    [switch]$SkipVerification = $false
)

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "FSH Starter Kit - IIS Deployment Builder" -ForegroundColor Cyan
Write-Host "Windows Server (IIS) Optimized" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Stop"
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootPath = $scriptPath

# Validate .NET is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host "✓ .NET found: $dotnetVersion" -ForegroundColor Green
}
catch {
    Write-Host "✗ .NET SDK not found. Please install .NET SDK." -ForegroundColor Red
    exit 1
}

# Define paths
$apiProjectPath = Join-Path $rootPath "api/server"
$blazorProjectPath = Join-Path $rootPath "apps/blazor/client"
$apiOutputPath = Join-Path $OutputPath "api"
$blazorOutputPath = Join-Path $OutputPath "blazor"

# Validate project paths
if (-not (Test-Path $apiProjectPath)) {
    Write-Host "✗ API project not found at: $apiProjectPath" -ForegroundColor Red
    exit 1
}
if (-not (Test-Path $blazorProjectPath)) {
    Write-Host "✗ Blazor project not found at: $blazorProjectPath" -ForegroundColor Red
    exit 1
}

# Create output directory
Write-Host ""
Write-Host "Creating output directories..." -ForegroundColor Yellow
if (Test-Path $OutputPath) {
    Write-Host "Removing existing output directory..." -ForegroundColor Gray
    Remove-Item -Path $OutputPath -Recurse -Force -ErrorAction SilentlyContinue
}
New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
New-Item -ItemType Directory -Path $apiOutputPath -Force | Out-Null
New-Item -ItemType Directory -Path $blazorOutputPath -Force | Out-Null
Write-Host "✓ Output directories created" -ForegroundColor Green

# ============================================================================
# PUBLISH API
# ============================================================================
Write-Host ""
Write-Host "╔════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║          PUBLISHING API SERVICE            ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "Configuration: $Configuration" -ForegroundColor Yellow
Write-Host "Project: $apiProjectPath" -ForegroundColor Gray
Write-Host "Output: $apiOutputPath" -ForegroundColor Gray
Write-Host ""

try {
    Push-Location $apiProjectPath
    
    Write-Host "Running: dotnet publish (IIS-optimized settings)..." -ForegroundColor Gray
    dotnet publish `
        -c $Configuration `
        -o $apiOutputPath `
        --no-self-contained `
        /p:PublishProfile="" `
        /p:EnableSdkContainerSupport=false `
        /p:PublishSingleFile=false `
        /p:PublishReadyToRun=false `
        /p:PublishTrimmed=false `
        /p:DebugType=none `
        /p:DebugSymbols=false `
        -v minimal
    
    if ($LASTEXITCODE -ne 0) {
        throw "API publish failed with exit code $LASTEXITCODE"
    }
    
    Write-Host ""
    Write-Host "✓ API published successfully" -ForegroundColor Green
    
    # Create web.config for IIS if requested
    if ($CreateWebConfig) {
        Write-Host ""
        Write-Host "Creating web.config for IIS..." -ForegroundColor Yellow
        
        $webConfig = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <!-- Enable compression for better performance -->
      <httpCompression directory="%SystemDrive%\inetpub\temp\IIS Temporary Compressed Files">
        <scheme name="gzip" dll="%Windir%\system32\inetsrv\gzip.dll" staticCompressionLevel="9" />
        <dynamicTypes>
          <add mimeType="text/*" enabled="true" />
          <add mimeType="message/*" enabled="true" />
          <add mimeType="application/javascript" enabled="true" />
          <add mimeType="application/json" enabled="true" />
          <add mimeType="*/*" enabled="false" />
        </dynamicTypes>
      </httpCompression>
      
      <!-- ASP.NET Core Module v2 -->
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      
      <aspNetCore processPath="dotnet" arguments=".\Server.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout">
        <environmentVariables>
          <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
        </environmentVariables>
      </aspNetCore>
      
      <!-- URL Rewrite for HTTPS -->
      <rewrite>
        <rules>
          <rule name="HTTP to HTTPS redirect" stopProcessing="true">
            <match url="(.*)" />
            <conditions>
              <add input="{HTTPS}" pattern="^OFF$" />
            </conditions>
            <action type="Redirect" url="https://{HTTP_HOST}{REQUEST_URI}" redirectType="Permanent" />
          </rule>
        </rules>
      </rewrite>
      
      <!-- Security headers -->
      <httpProtocol>
        <customHeaders>
          <add name="X-Content-Type-Options" value="nosniff" />
          <add name="X-Frame-Options" value="SAMEORIGIN" />
          <add name="X-XSS-Protection" value="1; mode=block" />
          <add name="Strict-Transport-Security" value="max-age=31536000; includeSubDomains" />
        </customHeaders>
      </httpProtocol>
    </system.webServer>
  </location>
</configuration>
"@
        Set-Content -Path (Join-Path $apiOutputPath "web.config") -Value $webConfig -Force
        Write-Host "✓ web.config created with IIS optimizations" -ForegroundColor Green
    }
    
    Pop-Location
}
catch {
    Pop-Location
    Write-Host ""
    Write-Host "✗ API publish failed: $_" -ForegroundColor Red
    exit 1
}

# ============================================================================
# PUBLISH BLAZOR CLIENT
# ============================================================================
Write-Host ""
Write-Host "╔════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║      PUBLISHING BLAZOR WEB ASSEMBLY        ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "Configuration: $Configuration" -ForegroundColor Yellow
Write-Host "Project: $blazorProjectPath" -ForegroundColor Gray
Write-Host "Output: $blazorOutputPath" -ForegroundColor Gray
Write-Host ""

try {
    Push-Location $blazorProjectPath
    
    Write-Host "Running: dotnet publish..." -ForegroundColor Gray
    dotnet publish `
        -c $Configuration `
        -o "$blazorOutputPath/temp" `
        -v minimal
    
    if ($LASTEXITCODE -ne 0) {
        throw "Blazor publish failed with exit code $LASTEXITCODE"
    }

    # Copy wwwroot content to the output (for IIS, we only need the wwwroot folder content)
    Write-Host ""
    Write-Host "Extracting wwwroot content for IIS..." -ForegroundColor Yellow
    $wwwrootSource = Join-Path "$blazorOutputPath/temp" "wwwroot"
    
    if (Test-Path $wwwrootSource) {
        Copy-Item -Path "$wwwrootSource/*" -Destination $blazorOutputPath -Recurse -Force -ErrorAction Stop
        Remove-Item -Path "$blazorOutputPath/temp" -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "✓ Blazor client published successfully" -ForegroundColor Green
    }
    else {
        throw "wwwroot folder not found in publish output"
    }
    
    # Create web.config for static files
    if ($CreateWebConfig) {
        Write-Host ""
        Write-Host "Creating web.config for Blazor static content..." -ForegroundColor Yellow
        
        $wwwrootWebConfig = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <!-- Enable gzip compression -->
    <httpCompression directory="%SystemDrive%\inetpub\temp\IIS Temporary Compressed Files">
      <scheme name="gzip" dll="%Windir%\system32\inetsrv\gzip.dll" staticCompressionLevel="9" />
      <dynamicTypes>
        <add mimeType="text/html" enabled="true" />
        <add mimeType="text/css" enabled="true" />
        <add mimeType="application/javascript" enabled="true" />
        <add mimeType="application/json" enabled="true" />
        <add mimeType="application/wasm" enabled="true" />
      </dynamicTypes>
    </httpCompression>
    
    <!-- Add MIME types for Blazor WebAssembly -->
    <staticContent>
      <mimeMap fileExtension=".wasm" mimeType="application/wasm" />
      <mimeMap fileExtension=".js" mimeType="application/javascript" />
      <mimeMap fileExtension=".json" mimeType="application/json" />
    </staticContent>
    
    <!-- Cache headers for optimal performance -->
    <clientCache cacheControlMode="UseMaxAge" cacheControlMaxAgeInSeconds="31536000" />
    
    <!-- URL Rewrite for SPA routing -->
    <rewrite>
      <rules>
        <rule name="SPA routing" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
"@
        Set-Content -Path (Join-Path $blazorOutputPath "web.config") -Value $wwwrootWebConfig -Force
        Write-Host "✓ web.config created for Blazor static content" -ForegroundColor Green
    }
    
    Pop-Location
}
catch {
    Pop-Location
    Write-Host ""
    Write-Host "✗ Blazor publish failed: $_" -ForegroundColor Red
    exit 1
}

# ============================================================================
# CREATE DEPLOYMENT PACKAGES
# ============================================================================
Write-Host ""
Write-Host "╔════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║      CREATING DEPLOYMENT PACKAGES          ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$apiZipName = "FSH.Starter.API_$timestamp.zip"
$blazorZipName = "FSH.Starter.Blazor_$timestamp.zip"
$combinedZipName = "FSH.Starter.Deployment_$timestamp.zip"

try {
    # Compress API
    Write-Host "Compressing API files..." -ForegroundColor Yellow
    Compress-Archive -Path "$apiOutputPath/*" -DestinationPath (Join-Path $OutputPath $apiZipName) -Force
    $apiZipSize = (Get-Item (Join-Path $OutputPath $apiZipName)).Length / 1MB
    Write-Host "✓ API package created: $apiZipName ($([Math]::Round($apiZipSize, 2)) MB)" -ForegroundColor Green

    # Compress Blazor
    Write-Host "Compressing Blazor files..." -ForegroundColor Yellow
    Compress-Archive -Path "$blazorOutputPath/*" -DestinationPath (Join-Path $OutputPath $blazorZipName) -Force
    $blazorZipSize = (Get-Item (Join-Path $OutputPath $blazorZipName)).Length / 1MB
    Write-Host "✓ Blazor package created: $blazorZipName ($([Math]::Round($blazorZipSize, 2)) MB)" -ForegroundColor Green
    
    # Create combined archive
    Write-Host "Creating combined deployment package..." -ForegroundColor Yellow
    $tempCombined = Join-Path $OutputPath "_temp_combined"
    New-Item -ItemType Directory -Path $tempCombined -Force | Out-Null
    Copy-Item -Path (Join-Path $OutputPath $apiZipName) -Destination $tempCombined -Force
    Copy-Item -Path (Join-Path $OutputPath $blazorZipName) -Destination $tempCombined -Force
    Compress-Archive -Path "$tempCombined/*" -DestinationPath (Join-Path $OutputPath $combinedZipName) -Force
    Remove-Item -Path $tempCombined -Recurse -Force
    Write-Host "✓ Combined package created: $combinedZipName" -ForegroundColor Green
}
catch {
    Write-Host ""
    Write-Host "✗ Package creation failed: $_" -ForegroundColor Red
    exit 1
}

# ============================================================================
# DEPLOYMENT VERIFICATION (if not skipped)
# ============================================================================
if (-not $SkipVerification) {
    Write-Host ""
    Write-Host "╔════════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "║        VERIFYING DEPLOYMENT FILES          ║" -ForegroundColor Cyan
    Write-Host "╚════════════════════════════════════════════╝" -ForegroundColor Cyan
    Write-Host ""
    
    # Check API files
    Write-Host "Checking API files..." -ForegroundColor Yellow
    $apiDllCount = (Get-ChildItem -Path $apiOutputPath -Filter "*.dll" -Recurse).Count
    $apiJsonCount = (Get-ChildItem -Path $apiOutputPath -Filter "appsettings*.json").Count
    Write-Host "  - DLL files: $apiDllCount" -ForegroundColor Gray
    Write-Host "  - appsettings files: $apiJsonCount" -ForegroundColor Gray
    
    if ($apiJsonCount -eq 0) {
        Write-Host "  ⚠️  Warning: No appsettings.json files found" -ForegroundColor Yellow
    }
    
    # Check Blazor files
    Write-Host "Checking Blazor files..." -ForegroundColor Yellow
    $blazorIndexCount = (Get-ChildItem -Path $blazorOutputPath -Filter "index.html").Count
    $blazorWasmCount = (Get-ChildItem -Path $blazorOutputPath -Filter "*.wasm" -Recurse).Count
    Write-Host "  - index.html: $blazorIndexCount" -ForegroundColor Gray
    Write-Host "  - WASM files: $blazorWasmCount" -ForegroundColor Gray
    
    if ($blazorIndexCount -eq 0 -or $blazorWasmCount -eq 0) {
        Write-Host "  ⚠️  Warning: Blazor static files may be incomplete" -ForegroundColor Yellow
    }
    
    Write-Host "✓ Verification complete" -ForegroundColor Green
}

# ============================================================================
# SUMMARY
# ============================================================================
Write-Host ""
Write-Host "╔════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║       ✅ DEPLOYMENT READY FOR IIS!         ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "📦 DEPLOYMENT ARTIFACTS:" -ForegroundColor Yellow
Write-Host ""
Write-Host "Individual packages:" -ForegroundColor White
Write-Host "  API:            $(Join-Path $OutputPath $apiZipName)" -ForegroundColor Green
Write-Host "  Blazor:         $(Join-Path $OutputPath $blazorZipName)" -ForegroundColor Green
Write-Host ""
Write-Host "Combined package:" -ForegroundColor White
Write-Host "  All-in-One:     $(Join-Path $OutputPath $combinedZipName)" -ForegroundColor Cyan
Write-Host ""
Write-Host "Uncompressed files (for inspection):" -ForegroundColor White
Write-Host "  API:            $apiOutputPath" -ForegroundColor Gray
Write-Host "  Blazor:         $blazorOutputPath" -ForegroundColor Gray
Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host ""
Write-Host "🚀 NEXT STEPS FOR IIS DEPLOYMENT:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1️⃣  PRE-DEPLOYMENT (Windows Server):" -ForegroundColor Cyan
Write-Host "   ✓ Install .NET 9 Hosting Bundle from https://dotnet.microsoft.com/download" -ForegroundColor White
Write-Host "   ✓ Install IIS URL Rewrite Module" -ForegroundColor White
Write-Host "   ✓ Ensure IIS Application Development is enabled" -ForegroundColor White
Write-Host ""
Write-Host "2️⃣  TRANSFER PACKAGES:" -ForegroundColor Cyan
Write-Host "   ✓ Copy ZIP files to Windows Server" -ForegroundColor White
Write-Host "   ✓ Extract API ZIP to: C:\inetpub\wwwroot\api" -ForegroundColor White
Write-Host "   ✓ Extract Blazor ZIP to: C:\inetpub\wwwroot\blazor" -ForegroundColor White
Write-Host ""
Write-Host "3️⃣  CONFIGURE APPLICATION POOL:" -ForegroundColor Cyan
Write-Host "   ✓ Create new App Pool in IIS Manager" -ForegroundColor White
Write-Host "   ✓ Set .NET CLR version: No Managed Code" -ForegroundColor White
Write-Host "   ✓ Set Managed Pipeline Mode: Integrated" -ForegroundColor White
Write-Host ""
Write-Host "4️⃣  CONFIGURE IIS SITES:" -ForegroundColor Cyan
Write-Host "   ✓ Create API Site" -ForegroundColor White
Write-Host "     - Physical path: C:\inetpub\wwwroot\api" -ForegroundColor White
Write-Host "     - App Pool: Your created pool" -ForegroundColor White
Write-Host "     - Binding: https://api.yourdomain.com" -ForegroundColor White
Write-Host "   ✓ Create Blazor Site" -ForegroundColor White
Write-Host "     - Physical path: C:\inetpub\wwwroot\blazor" -ForegroundColor White
Write-Host "     - Binding: https://yourdomain.com" -ForegroundColor White
Write-Host ""
Write-Host "5️⃣  CONFIGURATION (CRITICAL):" -ForegroundColor Cyan
Write-Host "   ✓ Edit: C:\inetpub\wwwroot\api\appsettings.Production.json" -ForegroundColor White
Write-Host "     - Update database connection string" -ForegroundColor White
Write-Host "     - Set CORS origins to Blazor URL" -ForegroundColor White
Write-Host "     - Configure JWT secrets" -ForegroundColor White
Write-Host "   ✓ Edit: C:\inetpub\wwwroot\blazor\appsettings.json" -ForegroundColor White
Write-Host "     - Update API base URL" -ForegroundColor White
Write-Host ""
Write-Host "6️⃣  FILE PERMISSIONS:" -ForegroundColor Cyan
Write-Host "   ✓ Right-click folder → Properties → Security" -ForegroundColor White
Write-Host "   ✓ Add: IIS AppPool\YourPoolName → Full Control" -ForegroundColor White
Write-Host "   ✓ Add logs folder if logging is enabled" -ForegroundColor White
Write-Host ""
Write-Host "7️⃣  SSL/TLS CERTIFICATE:" -ForegroundColor Cyan
Write-Host "   ✓ Bind valid HTTPS certificate to both sites" -ForegroundColor White
Write-Host "   ✓ Recommended: Use Let's Encrypt with IIS extension" -ForegroundColor White
Write-Host ""
Write-Host "8️⃣  FINAL STEPS:" -ForegroundColor Cyan
Write-Host "   ✓ Run in PowerShell (as Administrator):" -ForegroundColor White
Write-Host "     iisreset /restart" -ForegroundColor Gray
Write-Host "   ✓ Test API: https://api.yourdomain.com/api/health" -ForegroundColor White
Write-Host "   ✓ Test Blazor: https://yourdomain.com" -ForegroundColor White
Write-Host "   ✓ Check IIS Event Logs for errors" -ForegroundColor White
Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host ""
Write-Host "📚 DOCUMENTATION:" -ForegroundColor Yellow
Write-Host "  - IIS Setup Guide: See LOGGING_IIS_SETUP.md" -ForegroundColor White
Write-Host "  - Framework Docs: https://learn.microsoft.com/dotnet/core/deployment/native-aot/" -ForegroundColor White
Write-Host ""
Write-Host "✅ Deployment packages are ready!" -ForegroundColor Green
Write-Host ""

