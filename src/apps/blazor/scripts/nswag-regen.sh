#!/bin/bash
# This script regenerates NSwag API clients for the Blazor application
# Usage: ./nswag-regen.sh

# Exit on error
set -e

# Save current directory
CURRENT_DIR=$(pwd)

# Get root directory using git
ROOT_DIR=$(git rev-parse --show-toplevel)

# Define paths
HOST_DIR="${ROOT_DIR}/src/apps/blazor/client"
INFRASTRUCTURE_PRJ="${ROOT_DIR}/src/apps/blazor/infrastructure/Infrastructure.csproj"

echo "=========================================="
echo "NSwag API Client Regeneration Script"
echo "=========================================="
echo ""
echo "Make sure you have run the WebAPI project first!"
echo "The API should be running at https://localhost:7000"
echo ""
read -p "Press any key to continue... " -n1 -s
echo ""
echo ""

# Change to host directory
cd "$HOST_DIR"
echo "Host Directory: $HOST_DIR"
echo ""

# Run NSwag generation
echo "Running NSwag generation..."
echo "Command: dotnet build -t:NSwag $INFRASTRUCTURE_PRJ"
echo ""

dotnet build -t:NSwag "$INFRASTRUCTURE_PRJ"

# Return to original directory
cd "$CURRENT_DIR"

echo ""
echo "=========================================="
echo "NSwag Regeneration Complete!"
echo "=========================================="
echo ""
read -p "Press any key to exit... " -n1 -s
echo ""

