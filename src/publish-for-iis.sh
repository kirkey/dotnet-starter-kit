#!/bin/bash
# Bash Script to Publish API and Blazor Client
# Run this script from your development machine (macOS/Linux)

set -e

CONFIGURATION="Release"
OUTPUT_PATH="./deploy"

echo "====================================="
echo "FSH Starter Kit - Deployment Builder"
echo "====================================="
echo ""

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_PATH="$SCRIPT_DIR"

# Define paths
API_PROJECT_PATH="$ROOT_PATH/api/server"
BLAZOR_PROJECT_PATH="$ROOT_PATH/apps/blazor/client"
FULL_OUTPUT_PATH="$ROOT_PATH/$OUTPUT_PATH"
API_OUTPUT_PATH="$FULL_OUTPUT_PATH/api"
BLAZOR_OUTPUT_PATH="$FULL_OUTPUT_PATH/blazor"

# Create output directory
echo "Creating output directory at: $FULL_OUTPUT_PATH"
rm -rf "$FULL_OUTPUT_PATH"
mkdir -p "$FULL_OUTPUT_PATH"
mkdir -p "$API_OUTPUT_PATH"
mkdir -p "$BLAZOR_OUTPUT_PATH"

# Publish API
echo ""
echo "Publishing API..."
echo "Project: $API_PROJECT_PATH"
echo "Output: $API_OUTPUT_PATH"

cd "$API_PROJECT_PATH"
# Publish for IIS - self-contained to generate .exe executable
dotnet publish -c $CONFIGURATION -o "$API_OUTPUT_PATH" \
  --self-contained \
  -r win-x64 \
  /p:PublishProfile="" \
  /p:EnableSdkContainerSupport=false \
  /p:DebugType=none \
  /p:DebugSymbols=false
if [ $? -eq 0 ]; then
    echo "✓ API published successfully for IIS (win-x64 self-contained)"
else
    echo "✗ API publish failed"
    exit 1
fi
cd "$ROOT_PATH"

# Publish Blazor Client
echo ""
echo "Publishing Blazor Client..."
echo "Project: $BLAZOR_PROJECT_PATH"
echo "Output: $BLAZOR_OUTPUT_PATH"

cd "$BLAZOR_PROJECT_PATH"
BLAZOR_TEMP="$BLAZOR_OUTPUT_PATH/temp"
dotnet publish -c $CONFIGURATION -o "$BLAZOR_TEMP"
if [ $? -eq 0 ]; then
    # Copy wwwroot content to the output
    cp -r "$BLAZOR_TEMP/wwwroot/"* "$BLAZOR_OUTPUT_PATH/"
    rm -rf "$BLAZOR_TEMP"
    echo "✓ Blazor client published successfully"
else
    echo "✗ Blazor publish failed"
    exit 1
fi
cd "$ROOT_PATH"

# Create deployment packages
echo ""
echo "Creating deployment packages..."

TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
API_ZIP_NAME="FSH.Starter.API_$TIMESTAMP.zip"
BLAZOR_ZIP_NAME="FSH.Starter.Blazor_$TIMESTAMP.zip"

cd "$FULL_OUTPUT_PATH"

# Compress API
cd api
zip -r "../$API_ZIP_NAME" .
cd ..
echo "✓ API package created: $API_ZIP_NAME"

# Compress Blazor
cd blazor
zip -r "../$BLAZOR_ZIP_NAME" .
cd ..
echo "✓ Blazor package created: $BLAZOR_ZIP_NAME"

cd "$ROOT_PATH"

# Summary
echo ""
echo "====================================="
echo "Deployment packages ready!"
echo "====================================="
echo ""
echo "Deployment packages location:"
echo "  API Package: $FULL_OUTPUT_PATH/$API_ZIP_NAME"
echo "  Blazor Package: $FULL_OUTPUT_PATH/$BLAZOR_ZIP_NAME"
echo ""
echo "Uncompressed folders:"
echo "  API: $API_OUTPUT_PATH"
echo "  Blazor: $BLAZOR_OUTPUT_PATH"
echo ""
echo "Next Steps:"
echo "1. Transfer the packages to your IIS server"
echo "2. Follow the IIS_DEPLOYMENT_GUIDE.md for deployment instructions"
echo "3. Update appsettings.json on the server with production settings"
echo ""