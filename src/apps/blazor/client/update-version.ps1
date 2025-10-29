# Version Update Script (PowerShell)
# Updates the version.json file with a new version number and build timestamp

param(
    [Parameter(Mandatory=$true, HelpMessage="Version number (e.g., 1.0.0)")]
    [string]$Version,
    
    [Parameter(Mandatory=$false)]
    [string]$File = "wwwroot\version.json",
    
    [Parameter(Mandatory=$false)]
    [string]$Description = "Application version information",
    
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

# Display help
if ($Help) {
    Write-Host @"
Version Update Script

SYNOPSIS
    Updates the version.json file with a new version number and build timestamp.

SYNTAX
    .\update-version.ps1 -Version <version> [-File <path>] [-Description <text>]

PARAMETERS
    -Version <string>
        The version number (e.g., 1.0.0). Required.
    
    -File <string>
        Path to version.json file (default: wwwroot\version.json). Optional.
    
    -Description <string>
        Version description (default: 'Application version information'). Optional.
    
    -Help
        Display this help message.

EXAMPLES
    .\update-version.ps1 -Version 1.2.3
    
    .\update-version.ps1 -Version 2.0.0 -Description "Major release with new features"
    
    .\update-version.ps1 -Version 1.0.1 -File ".\Client\wwwroot\version.json"

"@
    exit 0
}

# Validate version format
$semverPattern = '^[0-9]+\.[0-9]+\.[0-9]+(-[a-zA-Z0-9]+)?$'
if ($Version -notmatch $semverPattern) {
    Write-Host "Warning: Version '$Version' does not follow semantic versioning (MAJOR.MINOR.PATCH)" -ForegroundColor Yellow
    $continue = Read-Host "Continue anyway? (y/N)"
    if ($continue -ne 'y' -and $continue -ne 'Y') {
        exit 1
    }
}

# Get current timestamp in ISO 8601 format
$buildDate = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")

# Create version object
$versionObject = @{
    version = $Version
    buildDate = $buildDate
    description = $Description
} | ConvertTo-Json -Depth 10

# Create directory if it doesn't exist
$versionDir = Split-Path -Parent $File
if (!(Test-Path $versionDir)) {
    Write-Host "Creating directory: $versionDir" -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $versionDir -Force | Out-Null
}

# Backup existing version file if it exists
if (Test-Path $File) {
    $backupFile = "$File.backup"
    Write-Host "Backing up existing version file to: $backupFile" -ForegroundColor Yellow
    Copy-Item $File $backupFile -Force
}

# Write new version file
try {
    $versionObject | Out-File -FilePath $File -Encoding UTF8 -NoNewline
    
    Write-Host "`n✓ Version file updated successfully!" -ForegroundColor Green
    Write-Host "`nVersion Details:"
    Write-Host "  Version: $Version"
    Write-Host "  Build Date: $buildDate"
    Write-Host "  File: $File"
    Write-Host "`nFile Contents:"
    Get-Content $File | Write-Host
}
catch {
    Write-Host "`n✗ Failed to create version file" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Red
    exit 1
}

