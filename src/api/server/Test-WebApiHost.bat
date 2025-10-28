@echo off
REM Test-WebApiHost.bat
REM This script runs the WebApi Host and pauses on error so you can see what went wrong

echo ==================================================================
echo FSH.Starter.WebApi.Host - Test Runner
echo ==================================================================
echo.
echo Starting application...
echo.

REM Check if the DLL exists
if not exist "FSH.Starter.WebApi.Host.dll" (
    echo ERROR: FSH.Starter.WebApi.Host.dll not found!
    echo.
    echo Make sure you are running this from the publish directory.
    echo.
    pause
    exit /b 1
)

REM Run the application
dotnet FSH.Starter.WebApi.Host.dll

REM Capture the exit code
set EXIT_CODE=%ERRORLEVEL%

echo.
echo ==================================================================
if %EXIT_CODE% EQU 0 (
    echo Application exited normally
) else (
    echo Application exited with error code: %EXIT_CODE%
    echo.
    echo Check the log files in the 'logs' directory for details:
    echo   - logs\app-*.log      : Application logs
    echo   - logs\startup-*.log  : Startup logs
    echo   - logs\stdout-*.log   : IIS stdout logs
)
echo ==================================================================
echo.
echo Press any key to exit...
pause > nul
exit /b %EXIT_CODE%

