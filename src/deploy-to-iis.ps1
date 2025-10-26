# PowerShell Script to Deploy to IIS Server
# Run this script ON THE IIS SERVER with Administrator privileges

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiPackagePath,
    
    [Parameter(Mandatory=$true)]
    [string]$BlazorPackagePath,
    
    [string]$ApiSiteName = "FSH.Starter.API",
    [string]$BlazorSiteName = "FSH.Starter.Blazor",
    
    [string]$ApiPath = "C:\inetpub\wwwroot\FSH.Starter.API",
    [string]$BlazorPath = "C:\inetpub\wwwroot\FSH.Starter.Blazor",
    
    [string]$ApiAppPoolName = "FSH.Starter.API.Pool",
    [string]$BlazorAppPoolName = "FSH.Starter.Blazor.Pool",
    
    [string]$ApiHostname = "api.yourdomain.com",
    [string]$BlazorHostname = "app.yourdomain.com",
    
    [int]$ApiPort = 443,
    [int]$BlazorPort = 443,
    
    [string]$CertificateThumbprint = ""
)

# Check if running as administrator
$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
$isAdmin = $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Write-Host "ERROR: This script must be run as Administrator!" -ForegroundColor Red
    exit 1
}

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "FSH Starter Kit - IIS Deployment" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

Import-Module WebAdministration

# Function to create or update application pool
function New-OrUpdate-AppPool {
    param(
        [string]$Name
    )
    
    if (Test-Path "IIS:\AppPools\$Name") {
        Write-Host "Application pool '$Name' already exists, stopping it..." -ForegroundColor Yellow
        Stop-WebAppPool -Name $Name
        Start-Sleep -Seconds 2
    } else {
        Write-Host "Creating application pool '$Name'..." -ForegroundColor Green
        New-WebAppPool -Name $Name
    }
    
    Set-ItemProperty "IIS:\AppPools\$Name" -Name "managedRuntimeVersion" -Value ""
    Set-ItemProperty "IIS:\AppPools\$Name" -Name "managedPipelineMode" -Value "Integrated"
    
    Write-Host "✓ Application pool '$Name' configured" -ForegroundColor Green
}

# Function to deploy files
function Deploy-Files {
    param(
        [string]$ZipPath,
        [string]$DestinationPath,
        [string]$Type
    )
    
    Write-Host ""
    Write-Host "Deploying $Type..." -ForegroundColor Green
    Write-Host "Source: $ZipPath" -ForegroundColor Gray
    Write-Host "Destination: $DestinationPath" -ForegroundColor Gray
    
    # Create destination directory
    if (Test-Path $DestinationPath) {
        Write-Host "Removing existing files..." -ForegroundColor Yellow
        Remove-Item -Path $DestinationPath -Recurse -Force
    }
    New-Item -ItemType Directory -Path $DestinationPath -Force | Out-Null
    
    # Extract files
    Write-Host "Extracting files..." -ForegroundColor Yellow
    Expand-Archive -Path $ZipPath -DestinationPath $DestinationPath -Force
    
    # Set permissions
    Write-Host "Setting permissions..." -ForegroundColor Yellow
    $acl = Get-Acl $DestinationPath
    $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule("IIS_IUSRS", "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")
    $acl.AddAccessRule($accessRule)
    Set-Acl $DestinationPath $acl
    
    Write-Host "✓ $Type deployed successfully" -ForegroundColor Green
}

# Function to create or update website
function New-OrUpdate-Website {
    param(
        [string]$Name,
        [string]$PhysicalPath,
        [string]$AppPoolName,
        [string]$Hostname,
        [int]$Port,
        [string]$Protocol = "https"
    )
    
    # Check if site exists
    if (Test-Path "IIS:\Sites\$Name") {
        Write-Host "Website '$Name' already exists, updating..." -ForegroundColor Yellow
        Remove-Website -Name $Name
    }
    
    Write-Host "Creating website '$Name'..." -ForegroundColor Green
    
    # Create site
    if ($Protocol -eq "https" -and $CertificateThumbprint) {
        New-Website -Name $Name `
                    -PhysicalPath $PhysicalPath `
                    -ApplicationPool $AppPoolName `
                    -Port $Port `
                    -Protocol $Protocol `
                    -HostHeader $Hostname `
                    -Ssl
        
        # Bind certificate
        $binding = Get-WebBinding -Name $Name -Protocol "https"
        $binding.AddSslCertificate($CertificateThumbprint, "my")
    } else {
        New-Website -Name $Name `
                    -PhysicalPath $PhysicalPath `
                    -ApplicationPool $AppPoolName `
                    -Port $Port `
                    -Protocol "http" `
                    -HostHeader $Hostname
    }
    
    Write-Host "✓ Website '$Name' created" -ForegroundColor Green
}

# Deploy API
Write-Host ""
Write-Host "=== Deploying API ===" -ForegroundColor Cyan
New-OrUpdate-AppPool -Name $ApiAppPoolName
Deploy-Files -ZipPath $ApiPackagePath -DestinationPath $ApiPath -Type "API"

# Create logs directory for API
$apiLogsPath = Join-Path $ApiPath "logs"
if (-not (Test-Path $apiLogsPath)) {
    New-Item -ItemType Directory -Path $apiLogsPath -Force | Out-Null
}

New-OrUpdate-Website -Name $ApiSiteName `
                     -PhysicalPath $ApiPath `
                     -AppPoolName $ApiAppPoolName `
                     -Hostname $ApiHostname `
                     -Port $ApiPort `
                     -Protocol "https"

# Deploy Blazor Client
Write-Host ""
Write-Host "=== Deploying Blazor Client ===" -ForegroundColor Cyan
New-OrUpdate-AppPool -Name $BlazorAppPoolName
Deploy-Files -ZipPath $BlazorPackagePath -DestinationPath $BlazorPath -Type "Blazor Client"

New-OrUpdate-Website -Name $BlazorSiteName `
                     -PhysicalPath $BlazorPath `
                     -AppPoolName $BlazorAppPoolName `
                     -Hostname $BlazorHostname `
                     -Port $BlazorPort `
                     -Protocol "https"

# Start application pools
Write-Host ""
Write-Host "Starting application pools..." -ForegroundColor Green
Start-WebAppPool -Name $ApiAppPoolName
Start-WebAppPool -Name $BlazorAppPoolName

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Deployment Complete!" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Deployed Sites:" -ForegroundColor Yellow
Write-Host "  API: https://$ApiHostname" -ForegroundColor White
Write-Host "  Blazor: https://$BlazorHostname" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Update appsettings.json in $ApiPath with production settings" -ForegroundColor White
Write-Host "2. Update appsettings.json in $BlazorPath with correct API URL" -ForegroundColor White
Write-Host "3. Configure SSL certificates if not already done" -ForegroundColor White
Write-Host "4. Test the applications in a browser" -ForegroundColor White
Write-Host "5. Check logs if there are any issues:" -ForegroundColor White
Write-Host "   - API Logs: $apiLogsPath" -ForegroundColor Gray
Write-Host "   - IIS Logs: C:\inetpub\logs\LogFiles" -ForegroundColor Gray
Write-Host ""
