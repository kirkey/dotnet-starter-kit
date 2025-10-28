# View-Logs.ps1
# Helper script to view and monitor log files

param(
    [Parameter(Mandatory=$false)]
    [string]$LogPath = ".\logs",
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("app", "startup", "stdout", "all", "latest")]
    [string]$LogType = "latest",
    
    [Parameter(Mandatory=$false)]
    [switch]$Follow,
    
    [Parameter(Mandatory=$false)]
    [int]$Lines = 50
)

function Show-LogMenu {
    Write-Host ""
    Write-Host "==================================================================" -ForegroundColor Cyan
    Write-Host "FSH.Starter.WebApi Log Viewer" -ForegroundColor Cyan
    Write-Host "==================================================================" -ForegroundColor Cyan
    Write-Host ""
}

function Get-LatestLogFile {
    param([string]$Pattern)
    
    $files = Get-ChildItem -Path $LogPath -Filter $Pattern -ErrorAction SilentlyContinue | 
             Sort-Object LastWriteTime -Descending
    
    if ($files) {
        return $files[0]
    }
    return $null
}

Show-LogMenu

# Verify log path exists
if (-not (Test-Path $LogPath)) {
    Write-Host "ERROR: Log directory not found: $LogPath" -ForegroundColor Red
    Write-Host ""
    Write-Host "The application may not have started yet, or logs are in a different location." -ForegroundColor Yellow
    Write-Host ""
    exit 1
}

Write-Host "Log Directory: $LogPath" -ForegroundColor Green
Write-Host ""

# Determine which log file to view
$logFile = $null

switch ($LogType) {
    "app" {
        $logFile = Get-LatestLogFile "app-*.log"
        $logTypeName = "Application Logs"
    }
    "startup" {
        $logFile = Get-LatestLogFile "startup-*.log"
        $logTypeName = "Startup Logs"
    }
    "stdout" {
        $logFile = Get-LatestLogFile "stdout-*.log"
        $logTypeName = "IIS stdout Logs"
    }
    "latest" {
        # Find the most recently modified log file
        $allLogs = Get-ChildItem -Path $LogPath -Filter "*.log" -ErrorAction SilentlyContinue |
                   Sort-Object LastWriteTime -Descending
        if ($allLogs) {
            $logFile = $allLogs[0]
            $logTypeName = "Latest Log File"
        }
    }
    "all" {
        Write-Host "Available log files:" -ForegroundColor Yellow
        Write-Host ""
        
        $allLogs = Get-ChildItem -Path $LogPath -Filter "*.log" -ErrorAction SilentlyContinue |
                   Sort-Object LastWriteTime -Descending
        
        if (-not $allLogs) {
            Write-Host "No log files found." -ForegroundColor Yellow
            exit 0
        }
        
        foreach ($log in $allLogs) {
            $size = "{0:N2} KB" -f ($log.Length / 1KB)
            $time = $log.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
            Write-Host ("{0,-30} {1,15} {2}" -f $log.Name, $size, $time)
        }
        Write-Host ""
        exit 0
    }
}

if (-not $logFile) {
    Write-Host "ERROR: No log files found for type: $LogType" -ForegroundColor Red
    Write-Host ""
    Write-Host "Available log types: app, startup, stdout, latest, all" -ForegroundColor Yellow
    Write-Host ""
    exit 1
}

Write-Host "Viewing: $logTypeName" -ForegroundColor Yellow
Write-Host "File: $($logFile.Name)" -ForegroundColor White
Write-Host "Size: $("{0:N2} KB" -f ($logFile.Length / 1KB))" -ForegroundColor White
Write-Host "Modified: $($logFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"))" -ForegroundColor White
Write-Host ""
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

if ($Follow) {
    Write-Host "Following log file (Press Ctrl+C to stop)..." -ForegroundColor Green
    Write-Host ""
    
    # Show last N lines first
    Get-Content $logFile.FullName -Tail $Lines
    
    # Then follow new entries
    Get-Content $logFile.FullName -Wait -Tail 0 | ForEach-Object {
        $line = $_
        
        # Color code based on log level
        if ($line -match "\[ERR\]|\[FTL\]|ERROR|FATAL") {
            Write-Host $line -ForegroundColor Red
        }
        elseif ($line -match "\[WRN\]|WARNING") {
            Write-Host $line -ForegroundColor Yellow
        }
        elseif ($line -match "\[INF\]|INFO") {
            Write-Host $line -ForegroundColor White
        }
        elseif ($line -match "\[DBG\]|DEBUG") {
            Write-Host $line -ForegroundColor Gray
        }
        else {
            Write-Host $line
        }
    }
}
else {
    # Just show the last N lines
    Get-Content $logFile.FullName -Tail $Lines | ForEach-Object {
        $line = $_
        
        # Color code based on log level
        if ($line -match "\[ERR\]|\[FTL\]|ERROR|FATAL") {
            Write-Host $line -ForegroundColor Red
        }
        elseif ($line -match "\[WRN\]|WARNING") {
            Write-Host $line -ForegroundColor Yellow
        }
        elseif ($line -match "\[INF\]|INFO") {
            Write-Host $line -ForegroundColor White
        }
        elseif ($line -match "\[DBG\]|DEBUG") {
            Write-Host $line -ForegroundColor Gray
        }
        else {
            Write-Host $line
        }
    }
    
    Write-Host ""
    Write-Host "==================================================================" -ForegroundColor Cyan
    Write-Host "Showing last $Lines lines. Use -Lines parameter to show more." -ForegroundColor Gray
    Write-Host "Use -Follow to continuously monitor the log file." -ForegroundColor Gray
    Write-Host ""
}

