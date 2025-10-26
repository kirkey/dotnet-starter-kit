# PowerShell Script to Publish API and Blazor Client
# Run this script from your development machine

param(
    [string]$Configuration = "Release",
    [string]$OutputPath = "./deploy"
)

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "FSH Starter Kit - Deployment Builder" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Stop"
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootPath = $scriptPath

# Define paths
$apiProjectPath = Join-Path $rootPath "api/server"
$blazorProjectPath = Join-Path $rootPath "apps/blazor/client"
$apiOutputPath = Join-Path $OutputPath "api"
$blazorOutputPath = Join-Path $OutputPath "blazor"

# Create output directory
Write-Host "Creating output directory at: $OutputPath" -ForegroundColor Yellow
if (Test-Path $OutputPath) {
    Remove-Item -Path $OutputPath -Recurse -Force
}
New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
New-Item -ItemType Directory -Path $apiOutputPath -Force | Out-Null
New-Item -ItemType Directory -Path $blazorOutputPath -Force | Out-Null

# Publish API
Write-Host ""
Write-Host "Publishing API..." -ForegroundColor Green
Write-Host "Project: $apiProjectPath" -ForegroundColor Gray
Write-Host "Output: $apiOutputPath" -ForegroundColor Gray

try {
    Push-Location $apiProjectPath
    dotnet publish -c $Configuration -o $apiOutputPath --no-self-contained /p:PublishProfile="" /p:EnableSdkContainerSupport=false
    if ($LASTEXITCODE -ne 0) {
        throw "API publish failed"
    }
    Write-Host "✓ API published successfully" -ForegroundColor Green
    Pop-Location
}
catch {
    Pop-Location
    Write-Host "✗ API publish failed: $_" -ForegroundColor Red
    exit 1
}

# Publish Blazor Client
Write-Host ""
Write-Host "Publishing Blazor Client..." -ForegroundColor Green
Write-Host "Project: $blazorProjectPath" -ForegroundColor Gray
Write-Host "Output: $blazorOutputPath" -ForegroundColor Gray

try {
    Push-Location $blazorProjectPath
    dotnet publish -c $Configuration -o "$blazorOutputPath/temp"
    if ($LASTEXITCODE -ne 0) {
        throw "Blazor publish failed"
    }

    # Copy wwwroot content to the output (for IIS, we only need the wwwroot folder content)
    $wwwrootSource = Join-Path "$blazorOutputPath/temp" "wwwroot"
    Copy-Item -Path "$wwwrootSource/*" -Destination $blazorOutputPath -Recurse -Force
    Remove-Item -Path "$blazorOutputPath/temp" -Recurse -Force

    Write-Host "✓ Blazor client published successfully" -ForegroundColor Green
    Pop-Location
}
catch {
    Pop-Location
    Write-Host "✗ Blazor publish failed: $_" -ForegroundColor Red
    exit 1
}

# Create deployment packages
Write-Host ""
Write-Host "Creating deployment packages..." -ForegroundColor Green

$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$apiZipName = "FSH.Starter.API_$timestamp.zip"
$blazorZipName = "FSH.Starter.Blazor_$timestamp.zip"

try {
    # Compress API
    Compress-Archive -Path "$apiOutputPath/*" -DestinationPath (Join-Path $OutputPath $apiZipName) -Force
    Write-Host "✓ API package created: $apiZipName" -ForegroundColor Green

    # Compress Blazor
    Compress-Archive -Path "$blazorOutputPath/*" -DestinationPath (Join-Path $OutputPath $blazorZipName) -Force
    Write-Host "✓ Blazor package created: $blazorZipName" -ForegroundColor Green
}
catch {
    Write-Host "✗ Package creation failed: $_" -ForegroundColor Red
    exit 1
}

# Summary
Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Deployment packages ready!" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Deployment packages location:" -ForegroundColor Yellow
Write-Host "  API Package: $(Join-Path $OutputPath $apiZipName)" -ForegroundColor White
Write-Host "  Blazor Package: $(Join-Path $OutputPath $blazorZipName)" -ForegroundColor White
Write-Host ""
Write-Host "Uncompressed folders:" -ForegroundColor Yellow
Write-Host "  API: $apiOutputPath" -ForegroundColor White
Write-Host "  Blazor: $blazorOutputPath" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Transfer the packages to your IIS server" -ForegroundColor White
Write-Host "2. Follow the IIS_DEPLOYMENT_GUIDE.md for deployment instructions" -ForegroundColor White
Write-Host "3. Update appsettings.json on the server with production settings" -ForegroundColor White
Write-Host ""

