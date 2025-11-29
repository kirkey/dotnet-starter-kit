./update-version.sh -v 1.0.5 -d "Patch release"#!/bin/bash
# Version Update Script
# Updates the version.json file with a new version number and build timestamp

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Default values
VERSION_FILE="wwwroot/version.json"
DEFAULT_VERSION="1.0.4"

# Function to display usage
usage() {
    echo "Usage: $0 [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  -v, --version VERSION    Set the version number (e.g., 1.0.0)"
    echo "  -f, --file PATH          Path to version.json file (default: wwwroot/version.json)"
    echo "  -d, --description DESC   Version description"
    echo "  -h, --help               Display this help message"
    echo ""
    echo "Examples:"
    echo "  $0 -v 1.2.3"
    echo "  $0 -v 2.0.0 -d 'Major release with new features'"
    echo "  $0 --version 1.0.1 --file ./Client/wwwroot/version.json"
    exit 1
}

# Parse command line arguments
VERSION=""
DESCRIPTION="Application version information"

while [[ $# -gt 0 ]]; do
    case $1 in
        -v|--version)
            VERSION="$2"
            shift 2
            ;;
        -f|--file)
            VERSION_FILE="$2"
            shift 2
            ;;
        -d|--description)
            DESCRIPTION="$2"
            shift 2
            ;;
        -h|--help)
            usage
            ;;
        *)
            echo -e "${RED}Error: Unknown option $1${NC}"
            usage
            ;;
    esac
done

# Validate version number
if [ -z "$VERSION" ]; then
    echo -e "${RED}Error: Version number is required${NC}"
    usage
fi

# Validate semantic versioning format
if ! [[ $VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+(-[a-zA-Z0-9]+)?$ ]]; then
    echo -e "${YELLOW}Warning: Version '$VERSION' does not follow semantic versioning (MAJOR.MINOR.PATCH)${NC}"
    read -p "Continue anyway? (y/N) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi

# Get current timestamp in ISO 8601 format
BUILD_DATE=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

# Create version.json content
VERSION_JSON=$(cat <<EOF
{
  "version": "$VERSION",
  "buildDate": "$BUILD_DATE",
  "description": "$DESCRIPTION"
}
EOF
)

# Create directory if it doesn't exist
VERSION_DIR=$(dirname "$VERSION_FILE")
if [ ! -d "$VERSION_DIR" ]; then
    echo -e "${YELLOW}Creating directory: $VERSION_DIR${NC}"
    mkdir -p "$VERSION_DIR"
fi

# Backup existing version file if it exists
if [ -f "$VERSION_FILE" ]; then
    BACKUP_FILE="${VERSION_FILE}.backup"
    echo -e "${YELLOW}Backing up existing version file to: $BACKUP_FILE${NC}"
    cp "$VERSION_FILE" "$BACKUP_FILE"
fi

# Write new version file
echo "$VERSION_JSON" > "$VERSION_FILE"

# Verify file was created
if [ -f "$VERSION_FILE" ]; then
    echo -e "${GREEN}✓ Version file updated successfully!${NC}"
    echo ""
    echo "Version Details:"
    echo "  Version: $VERSION"
    echo "  Build Date: $BUILD_DATE"
    echo "  File: $VERSION_FILE"
    echo ""
    echo "File Contents:"
    cat "$VERSION_FILE"
else
    echo -e "${RED}✗ Failed to create version file${NC}"
    exit 1
fi

