#!/bin/bash
# Quick NSwag regeneration script (non-interactive)
# Usage: ./nswag-quick.sh

set -e

ROOT_DIR=$(git rev-parse --show-toplevel)
INFRASTRUCTURE_PRJ="${ROOT_DIR}/src/apps/blazor/infrastructure/Infrastructure.csproj"
NSWAG_JSON="${ROOT_DIR}/src/apps/blazor/infrastructure/Api/nswag.json"

echo "Regenerating NSwag clients..."
echo "Project: $INFRASTRUCTURE_PRJ"
echo ""

# Method 1: Using dotnet build target (preferred)
dotnet build -t:NSwag "$INFRASTRUCTURE_PRJ"

# Alternative Method 2: Direct nswag command (uncomment if needed)
# cd "${ROOT_DIR}/src/apps/blazor/infrastructure/Api"
# nswag run nswag.json

echo ""
echo "✓ NSwag clients regenerated successfully!"
echo ""
echo "Generated files in: ${ROOT_DIR}/src/apps/blazor/infrastructure/Api/"

