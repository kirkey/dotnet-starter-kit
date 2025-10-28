# FSH API Status Check Script for Windows Server
# Run this on your Windows Server to check the API status

$apiPath = "D:\www\fsh9\api"
$apiUrl = "https://api.zaneco.ph:7000"
$swaggerUrl = "$apiUrl/swagger/"

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "FSH API Status Check" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Check if API directory exists
Write-Host "1. Checking API directory..." -NoNewline
if (Test-Path $apiPath) {
    Write-Host " OK" -ForegroundColor Green
    $exePath = Join-Path $apiPath "FSH.Starter.WebApi.Host.exe"
    if (Test-Path $exePath) {
        Write-Host "   - Host.exe found: $exePath" -ForegroundColor Gray
    } else {
        Write-Host "   - WARNING: Host.exe not found!" -ForegroundColor Yellow
    }
} else {
    Write-Host " NOT FOUND" -ForegroundColor Red
    Write-Host "   - Path does not exist: $apiPath" -ForegroundColor Red
    exit 1
}

# Check required directories
Write-Host ""
Write-Host "2. Checking required directories..." -ForegroundColor White
$directories = @("wwwroot", "files", "logs")
foreach ($dir in $directories) {
    $dirPath = Join-Path $apiPath $dir
    Write-Host "   - $dir : " -NoNewline
    if (Test-Path $dirPath) {
        Write-Host "OK" -ForegroundColor Green
    } else {
        Write-Host "MISSING (will be auto-created)" -ForegroundColor Yellow
    }
}

# Check appsettings.Production.json
Write-Host ""
Write-Host "3. Checking configuration..." -NoNewline
$configPath = Join-Path $apiPath "appsettings.Production.json"
if (Test-Path $configPath) {
    Write-Host " OK" -ForegroundColor Green
    $config = Get-Content $configPath -Raw | ConvertFrom-Json
    
    # Check SwaggerOptions
    if ($config.SwaggerOptions -and $config.SwaggerOptions.Enable) {
        Write-Host "   - Swagger: ENABLED" -ForegroundColor Green
    } else {
        Write-Host "   - Swagger: DISABLED" -ForegroundColor Yellow
    }
    
    # Check Database Provider
    if ($config.DatabaseOptions) {
        Write-Host "   - Database: $($config.DatabaseOptions.Provider)" -ForegroundColor Gray
    }
} else {
    Write-Host " MISSING" -ForegroundColor Red
}

# Check latest log file
Write-Host ""
Write-Host "4. Checking latest log file..." -ForegroundColor White
$logsPath = Join-Path $apiPath "logs"
if (Test-Path $logsPath) {
    $latestLog = Get-ChildItem $logsPath -Filter "app-*.log" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
    if ($latestLog) {
        Write-Host "   - Latest log: $($latestLog.Name)" -ForegroundColor Gray
        Write-Host "   - Last modified: $($latestLog.LastWriteTime)" -ForegroundColor Gray
        Write-Host "   - Size: $([math]::Round($latestLog.Length/1KB, 2)) KB" -ForegroundColor Gray
        
        # Check for errors in last 50 lines
        $lastLines = Get-Content $latestLog.FullName -Tail 50
        $errors = $lastLines | Select-String -Pattern "\[ERR\]|\[FTL\]"
        $warnings = $lastLines | Select-String -Pattern "\[WRN\]"
        
        if ($errors) {
            Write-Host "   - Recent errors found: $($errors.Count)" -ForegroundColor Red
        } else {
            Write-Host "   - No recent errors" -ForegroundColor Green
        }
        
        if ($warnings) {
            Write-Host "   - Recent warnings: $($warnings.Count)" -ForegroundColor Yellow
        }
    } else {
        Write-Host "   - No log files found" -ForegroundColor Yellow
    }
}

# Test API connectivity
Write-Host ""
Write-Host "5. Testing API connectivity..." -ForegroundColor White
Write-Host "   - Testing: $apiUrl" -NoNewline
try {
    $response = Invoke-WebRequest -Uri $apiUrl -Method GET -TimeoutSec 10 -UseBasicParsing -ErrorAction Stop
    Write-Host " OK (Status: $($response.StatusCode))" -ForegroundColor Green
} catch {
    Write-Host " FAILED" -ForegroundColor Red
    Write-Host "   - Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Swagger
Write-Host ""
Write-Host "6. Testing Swagger..." -ForegroundColor White
Write-Host "   - Testing: $swaggerUrl" -NoNewline
try {
    $response = Invoke-WebRequest -Uri $swaggerUrl -Method GET -TimeoutSec 10 -UseBasicParsing -ErrorAction Stop
    Write-Host " OK (Status: $($response.StatusCode))" -ForegroundColor Green
} catch {
    if ($_.Exception.Response.StatusCode.value__ -eq 404) {
        Write-Host " NOT FOUND" -ForegroundColor Yellow
        Write-Host "   - Swagger may be disabled in configuration" -ForegroundColor Yellow
    } elseif ($_.Exception.Message -match "authentication failed") {
        Write-Host " AUTHENTICATION REQUIRED" -ForegroundColor Yellow
        Write-Host "   - Enable Swagger in appsettings.Production.json" -ForegroundColor Yellow
    } else {
        Write-Host " FAILED" -ForegroundColor Red
        Write-Host "   - Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "Status Check Complete" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Show last 20 lines of log
Write-Host "Last 20 lines of log:" -ForegroundColor Cyan
Write-Host "--------------------------------------" -ForegroundColor Gray
if ($latestLog) {
    Get-Content $latestLog.FullName -Tail 20
}

Write-Host ""
Write-Host "To view full logs, run:" -ForegroundColor Yellow
Write-Host "  notepad $logsPath\$($latestLog.Name)" -ForegroundColor Gray
Write-Host ""

