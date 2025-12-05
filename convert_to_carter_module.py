#!/usr/bin/env python3
"""
Script to convert ICarterModule pattern to CarterModule("prefix") pattern.
This ensures proper NSwag client generation.
"""

import os
import re
import glob

def get_module_prefix(file_path):
    """Determine the module prefix based on the file path."""
    if '/Accounting/' in file_path:
        return 'accounting'
    elif '/Store/' in file_path:
        return 'store'
    elif '/HumanResources/' in file_path:
        return 'humanresources'
    elif '/MicroFinance/' in file_path:
        return 'microfinance'
    return None

def convert_file(file_path):
    """Convert a single file from ICarterModule to CarterModule pattern."""
    
    with open(file_path, 'r') as f:
        content = f.read()
    
    # Skip if already uses CarterModule base class
    if ': CarterModule' in content and 'ICarterModule' not in content:
        print(f"  Skipping (already converted): {file_path}")
        return False
    
    # Skip if doesn't use ICarterModule
    if ': ICarterModule' not in content:
        print(f"  Skipping (not ICarterModule): {file_path}")
        return False
    
    module_prefix = get_module_prefix(file_path)
    if not module_prefix:
        print(f"  Skipping (unknown module): {file_path}")
        return False
    
    original_content = content
    
    # Replace "public class XxxEndpoints : ICarterModule" with "public class XxxEndpoints() : CarterModule("prefix")"
    pattern = r'public class (\w+Endpoints) : ICarterModule'
    replacement = f'public class \\1() : CarterModule("{module_prefix}")'
    content = re.sub(pattern, replacement, content)
    
    # Replace "public void AddRoutes" with "public override void AddRoutes"
    content = content.replace('public void AddRoutes', 'public override void AddRoutes')
    
    # Remove "using Carter;" if it's there and add it back (to ensure it exists)
    # The CarterModule is in Carter namespace so we need using Carter;
    
    if content != original_content:
        with open(file_path, 'w') as f:
            f.write(content)
        print(f"  Converted: {file_path}")
        return True
    else:
        print(f"  No changes needed: {file_path}")
        return False

def main():
    base_path = '/Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules'
    
    # Find all *Endpoints.cs files in the target modules
    modules = ['Accounting', 'Store', 'HumanResources', 'MicroFinance']
    
    total_converted = 0
    total_files = 0
    
    for module in modules:
        module_path = os.path.join(base_path, module)
        if not os.path.exists(module_path):
            print(f"Module path not found: {module_path}")
            continue
        
        print(f"\n=== Processing {module} ===")
        
        # Find all *Endpoints.cs files
        pattern = os.path.join(module_path, '**', '*Endpoints.cs')
        files = glob.glob(pattern, recursive=True)
        
        for file_path in files:
            # Skip v1 subdirectory helper files (like CheckEndpoints.cs in v1 folder)
            if '/v1/' in file_path:
                print(f"  Skipping v1 helper: {file_path}")
                continue
            
            total_files += 1
            if convert_file(file_path):
                total_converted += 1
    
    print(f"\n=== Summary ===")
    print(f"Total files processed: {total_files}")
    print(f"Total files converted: {total_converted}")

if __name__ == '__main__':
    main()
