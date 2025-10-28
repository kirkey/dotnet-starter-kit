#!/bin/bash

# Blazor Publish Verification Script
# Checks if the published Blazor WebAssembly application has all necessary files for IIS deployment

BLAZOR_DIR="publishfsh9/blazor"

echo "=========================================="
echo "Blazor Publish Verification"
echo "=========================================="
echo ""

# Check if directory exists
if [ ! -d "$BLAZOR_DIR" ]; then
    echo "❌ ERROR: $BLAZOR_DIR directory not found!"
    echo "Run: make publish-blazor"
    exit 1
fi

echo "📁 Checking $BLAZOR_DIR..."
echo ""

# Count files
TOTAL_FILES=$(find "$BLAZOR_DIR" -type f 2>/dev/null | wc -l | tr -d ' ')
FRAMEWORK_FILES=$(find "$BLAZOR_DIR/_framework" -type f 2>/dev/null | wc -l | tr -d ' ')
WASM_FILES=$(find "$BLAZOR_DIR/_framework" -name "*.wasm" 2>/dev/null | wc -l | tr -d ' ')

echo "📊 File Counts:"
echo "   Total files: $TOTAL_FILES"
echo "   Files in _framework/: $FRAMEWORK_FILES"
echo "   .wasm files: $WASM_FILES"
echo ""

# Check critical files
echo "🔍 Checking Critical Files:"

check_file() {
    if [ -f "$BLAZOR_DIR/$1" ]; then
        SIZE=$(ls -lh "$BLAZOR_DIR/$1" 2>/dev/null | awk '{print $5}')
        echo "   ✅ $1 ($SIZE)"
        return 0
    else
        echo "   ❌ MISSING: $1"
        return 1
    fi
}

ERRORS=0

check_file "index.html" || ((ERRORS++))
check_file "appsettings.json" || ((ERRORS++))
check_file "web.config" || ((ERRORS++))
check_file "service-worker.js" || ((ERRORS++))
check_file "manifest.webmanifest" || ((ERRORS++))
check_file "_framework/blazor.boot.json" || ((ERRORS++))
check_file "_framework/blazor.webassembly.js" || ((ERRORS++))
check_file "_framework/dotnet.js" || ((ERRORS++))

# Check for at least one .wasm file
if [ "$WASM_FILES" -gt 0 ]; then
    echo "   ✅ WebAssembly runtime files present ($WASM_FILES files)"
else
    echo "   ❌ MISSING: No .wasm files found!"
    ((ERRORS++))
fi

echo ""

# Check directories
echo "📂 Checking Directories:"
check_dir() {
    if [ -d "$BLAZOR_DIR/$1" ]; then
        COUNT=$(find "$BLAZOR_DIR/$1" -type f 2>/dev/null | wc -l | tr -d ' ')
        echo "   ✅ $1/ ($COUNT files)"
        return 0
    else
        echo "   ❌ MISSING: $1/"
        return 1
    fi
}

check_dir "_framework" || ((ERRORS++))
check_dir "_content" || ((ERRORS++))
check_dir "css" || ((ERRORS++))

echo ""

# Size check
DIR_SIZE=$(du -sh "$BLAZOR_DIR" 2>/dev/null | cut -f1)
echo "💾 Total Size: $DIR_SIZE"
echo ""

# Final verdict
echo "=========================================="
if [ $ERRORS -eq 0 ]; then
    if [ "$TOTAL_FILES" -lt 50 ]; then
        echo "⚠️  WARNING: Only $TOTAL_FILES files found"
        echo "   Expected: 200+ files for complete Blazor publish"
        echo "   Your publish may be incomplete"
        echo ""
        echo "Try running: make clean-publish && make publish-blazor"
    else
        echo "✅ SUCCESS! Blazor publish is COMPLETE"
        echo ""
        echo "Your application is ready for IIS deployment!"
        echo ""
        echo "Next steps:"
        echo "1. Update $BLAZOR_DIR/appsettings.json with your API URL"
        echo "2. Create deployment ZIP: cd $BLAZOR_DIR && zip -r ../FSH.Starter.Blazor.zip ."
        echo "3. Transfer to Windows Server and extract to IIS folder"
    fi
else
    echo "❌ ERRORS FOUND: $ERRORS critical files/directories missing"
    echo ""
    echo "Run the following to fix:"
    echo "  make clean-publish"
    echo "  make publish-blazor"
fi
echo "=========================================="

