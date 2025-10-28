# Setup-IISLogPermissions.ps1
# This script configures the necessary permissions for IIS to write log files

param(
    [Parameter(Mandatory=$true)]
    [string]$AppPath,
    
    [Parameter(Mandatory=$false)]
    [string]$AppPoolName = "DefaultAppPool"
)

Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "IIS Log Directory Permissions Setup" -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

# Check if running as Administrator
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "ERROR: This script must be run as Administrator!" -ForegroundColor Red
    Write-Host "Please right-click PowerShell and select 'Run as Administrator'" -ForegroundColor Yellow
    exit 1
}

# Verify app path exists
if (-not (Test-Path $AppPath)) {
    Write-Host "ERROR: Application path does not exist: $AppPath" -ForegroundColor Red
    exit 1
}

Write-Host "Application Path: $AppPath" -ForegroundColor Green
Write-Host "App Pool Name: $AppPoolName" -ForegroundColor Green
Write-Host ""

# Create logs directory if it doesn't exist
$logsPath = Join-Path $AppPath "logs"
if (-not (Test-Path $logsPath)) {
    Write-Host "Creating logs directory..." -ForegroundColor Yellow
    New-Item -Path $logsPath -ItemType Directory -Force | Out-Null
    Write-Host "Logs directory created: $logsPath" -ForegroundColor Green
} else {
    Write-Host "Logs directory already exists: $logsPath" -ForegroundColor Green
}

# Set permissions for IIS App Pool
$appPoolIdentity = "IIS AppPool\$AppPoolName"
Write-Host ""
Write-Host "Setting permissions for: $appPoolIdentity" -ForegroundColor Yellow

try {
    # Grant full control to the logs directory
    icacls $logsPath /grant "${appPoolIdentity}:(OI)(CI)F" /T
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Permissions set successfully!" -ForegroundColor Green
    } else {
        Write-Host "Warning: icacls returned exit code $LASTEXITCODE" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "Current permissions for logs directory:" -ForegroundColor Cyan
    icacls $logsPath
    
} catch {
    Write-Host "ERROR: Failed to set permissions" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "Setup Complete!" -ForegroundColor Green
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Ensure your IIS Application Pool is named: $AppPoolName" -ForegroundColor White
Write-Host "2. Restart the IIS Application Pool" -ForegroundColor White
Write-Host "3. Start your application and check for log files in:" -ForegroundColor White
Write-Host "   $logsPath" -ForegroundColor Cyan
Write-Host ""
Write-Host "To restart the App Pool, run:" -ForegroundColor Yellow
Write-Host "   Restart-WebAppPool -Name '$AppPoolName'" -ForegroundColor White
Write-Host ""

