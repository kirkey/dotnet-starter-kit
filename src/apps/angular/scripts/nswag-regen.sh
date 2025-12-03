#!/bin/bash

# NSwag TypeScript Client Generation Script for Angular
# This script regenerates the API client from the backend Swagger specification

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ANGULAR_DIR="$SCRIPT_DIR/.."

echo "🔄 Generating TypeScript API client from Swagger..."

# Check if nswag is installed
if ! command -v nswag &> /dev/null; then
    echo "⚠️  NSwag CLI not found. Installing globally..."
    npm install -g nswag
fi

# Navigate to angular directory
cd "$ANGULAR_DIR"

# Run NSwag
nswag run nswag.json

if [ $? -eq 0 ]; then
    echo "✅ API client generated successfully!"
    echo "📁 Output: src/app/api/api-client.generated.ts"
else
    echo "❌ Failed to generate API client"
    echo ""
    echo "Make sure the API is running at https://localhost:7000"
    echo "You can also download the swagger.json manually and update nswag.json to use a local file"
    exit 1
fi
