@echo off
REM Batch Script to Publish API and Blazor Client for IIS Deployment
REM Run this script from your development machine (Windows)

setlocal enabledelayedexpansion

set CONFIGURATION=Release
set OUTPUT_PATH=deploy

echo =====================================
echo FSH Starter Kit - Deployment Builder
echo =====================================
echo.

REM Get the directory where the script is located
set SCRIPT_DIR=%~dp0
set ROOT_PATH=%SCRIPT_DIR%

REM Define paths
set API_PROJECT_PATH=%ROOT_PATH%api\server
set BLAZOR_PROJECT_PATH=%ROOT_PATH%apps\blazor\client
set FULL_OUTPUT_PATH=%ROOT_PATH%%OUTPUT_PATH%
set API_OUTPUT_PATH=%FULL_OUTPUT_PATH%\api
set BLAZOR_OUTPUT_PATH=%FULL_OUTPUT_PATH%\blazor

REM Create output directory
echo Creating output directory at: %FULL_OUTPUT_PATH%
if exist "%FULL_OUTPUT_PATH%" rmdir /s /q "%FULL_OUTPUT_PATH%"
mkdir "%FULL_OUTPUT_PATH%"
mkdir "%API_OUTPUT_PATH%"
mkdir "%BLAZOR_OUTPUT_PATH%"

REM Publish API
echo.
echo Publishing API...
echo Project: %API_PROJECT_PATH%
echo Output: %API_OUTPUT_PATH%

cd /d "%API_PROJECT_PATH%"
dotnet publish -c %CONFIGURATION% -o "%API_OUTPUT_PATH%" ^
  --self-contained ^
  -r win-x64 ^
  /p:PublishProfile="" ^
  /p:EnableSdkContainerSupport=false ^
  /p:DebugType=none ^
  /p:DebugSymbols=false

if %ERRORLEVEL% neq 0 (
    echo X API publish failed
    exit /b 1
)
echo OK API published successfully for IIS (win-x64 self-contained)
cd /d "%ROOT_PATH%"

REM Publish Blazor Client
echo.
echo Publishing Blazor Client...
echo Project: %BLAZOR_PROJECT_PATH%
echo Output: %BLAZOR_OUTPUT_PATH%

cd /d "%BLAZOR_PROJECT_PATH%"
set BLAZOR_TEMP=%BLAZOR_OUTPUT_PATH%\temp
dotnet publish -c %CONFIGURATION% -o "%BLAZOR_TEMP%"

if %ERRORLEVEL% neq 0 (
    echo X Blazor publish failed
    exit /b 1
)

REM Copy wwwroot content to the output
xcopy "%BLAZOR_TEMP%\wwwroot\*" "%BLAZOR_OUTPUT_PATH%" /E /I /Y >nul
rmdir /s /q "%BLAZOR_TEMP%"
echo OK Blazor client published successfully
cd /d "%ROOT_PATH%"

REM Create deployment packages
echo.
echo Creating deployment packages...

for /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c%%a%%b)
for /f "tokens=1-2 delims=/:" %%a in ('time /t') do (set mytime=%%a%%b)
set TIMESTAMP=%mydate%_%mytime%

set API_ZIP_NAME=FSH.Starter.API_%TIMESTAMP%.zip
set BLAZOR_ZIP_NAME=FSH.Starter.Blazor_%TIMESTAMP%.zip

cd /d "%FULL_OUTPUT_PATH%"

REM Compress API (requires 7-Zip or similar, or PowerShell)
if exist "%ProgramFiles%\7-Zip\7z.exe" (
    "%ProgramFiles%\7-Zip\7z.exe" a -r "%API_ZIP_NAME%" api
    echo OK API package created: %API_ZIP_NAME%
) else (
    REM Use PowerShell as fallback
    powershell -Command "Compress-Archive -Path 'api' -DestinationPath '%API_ZIP_NAME%' -Force"
    echo OK API package created: %API_ZIP_NAME%
)

REM Compress Blazor
if exist "%ProgramFiles%\7-Zip\7z.exe" (
    "%ProgramFiles%\7-Zip\7z.exe" a -r "%BLAZOR_ZIP_NAME%" blazor
    echo OK Blazor package created: %BLAZOR_ZIP_NAME%
) else (
    powershell -Command "Compress-Archive -Path 'blazor' -DestinationPath '%BLAZOR_ZIP_NAME%' -Force"
    echo OK Blazor package created: %BLAZOR_ZIP_NAME%
)

cd /d "%ROOT_PATH%"

REM Summary
echo.
echo =====================================
echo Deployment packages ready!
echo =====================================
echo.
echo Deployment packages location:
echo   API Package: %FULL_OUTPUT_PATH%\%API_ZIP_NAME%
echo   Blazor Package: %FULL_OUTPUT_PATH%\%BLAZOR_ZIP_NAME%
echo.
echo Uncompressed folders:
echo   API: %API_OUTPUT_PATH%
echo   Blazor: %BLAZOR_OUTPUT_PATH%
echo.
echo Next Steps:
echo 1. Transfer the packages to your IIS server
echo 2. Follow the IIS_DEPLOYMENT_GUIDE.md for deployment instructions
echo 3. Update appsettings.json on the server with production settings
echo.
pause

