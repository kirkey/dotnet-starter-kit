#!/usr/bin/env python3
"""Script to remove City parameter from MemberSeeder member tuples."""

import re

def remove_city_from_member_tuples(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Pattern to match member tuples with City (11 parameters: MBR-XXX through City through true/false)
    # Format: ("MBR-XXX", ..., decimal, year, "CityName", boolean)
    pattern = r'(\("MBR-\d+",\s+[^,]+,\s+[^,]+,\s+(?:[^,]+|null),\s+[^,]+,\s+[^,]+,\s+[^,]+,\s+[^,]+,\s+\d+,\s+\d{4}),\s+"[^"]+",\s+(true|false)\)'
    
    # Replace with 10-parameter version (remove City)
    replacement = r'\1, \2)'
    
    updated_content = re.sub(pattern, replacement, content)
    
    # Count replacements
    original_matches = len(re.findall(pattern, content))
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(updated_content)
    
    return original_matches

if __name__ == "__main__":
    file_path = "/Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Infrastructure/Persistence/Seeders/MemberSeeder.cs"
    count = remove_city_from_member_tuples(file_path)
    print(f"Removed City from {count} member entries")
