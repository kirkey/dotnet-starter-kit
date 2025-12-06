#!/usr/bin/env python3
"""
Script to fix incorrect FshActions mappings in MicroFinance endpoints.
Maps endpoint operation names to correct action types.
"""
import os
import re
from pathlib import Path

# Define action mapping rules based on operation name patterns
ACTION_MAPPINGS = {
    # Financial transactions - require specific permissions
    r'Deposit': 'Deposit',
    r'Withdraw': 'Withdraw',
    r'Transfer': 'Transfer',
    r'Freeze': 'Freeze',
    r'Unfreeze': 'Unfreeze',
    r'WriteOff': 'WriteOff',
    r'Mature': 'Mature',
    r'Renew': 'Renew',
    r'Disburse': 'Disburse',
    r'Close': 'Close',
    r'Post': 'Post',
    
    # Workflow actions
    r'Approve': 'Approve',
    r'Reject': 'Reject',
    r'Submit': 'Submit',
    r'Complete': 'Complete',
    r'Cancel': 'Cancel',
    r'Void': 'Void',
    r'Process': 'Process',
    
    # CRUD operations
    r'Create': 'Create',
    r'Update': 'Update',
    r'Delete': 'Delete',
    r'Search': 'Search',
    r'Get': 'View',  # Get operations are View
}

def get_correct_action(endpoint_name: str) -> str:
    """Determine the correct action based on the endpoint name."""
    # Check each pattern in order (more specific patterns first)
    for pattern, action in ACTION_MAPPINGS.items():
        if re.search(pattern, endpoint_name, re.IGNORECASE):
            return action
    # Default to View for read operations
    return 'View'

def fix_file(filepath: str) -> tuple[int, list[str]]:
    """Fix action mappings in a single file."""
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    
    original_content = content
    fixes_applied = []
    
    # Pattern to find .WithName(OperationName) followed by any content up to .RequirePermission(...)
    # We need to match the WithName to its corresponding RequirePermission
    
    # Find all endpoint blocks by looking at the pattern:
    # .WithName(ConstantName) ... .RequirePermission(FshPermission.NameFor(FshActions.XXX, ...))
    
    # Strategy: Find each WithName, extract the operation name, then find the next RequirePermission
    # and update the action if needed
    
    lines = content.split('\n')
    new_lines = []
    i = 0
    current_endpoint_name = None
    
    while i < len(lines):
        line = lines[i]
        
        # Check for WithName to capture the endpoint name
        withname_match = re.search(r'\.WithName\((\w+)\)', line)
        if withname_match:
            current_endpoint_name = withname_match.group(1)
        
        # Check for RequirePermission and fix if we have a current endpoint name
        if current_endpoint_name and 'RequirePermission' in line and 'FshActions.' in line:
            # Extract current action
            action_match = re.search(r'FshActions\.(\w+)', line)
            if action_match:
                current_action = action_match.group(1)
                correct_action = get_correct_action(current_endpoint_name)
                
                # Only fix if different
                if current_action != correct_action:
                    old_line = line
                    new_line = line.replace(f'FshActions.{current_action}', f'FshActions.{correct_action}')
                    lines[i] = new_line
                    fixes_applied.append(f"  {current_endpoint_name}: {current_action} -> {correct_action}")
                
            current_endpoint_name = None  # Reset after processing
        
        i += 1
    
    new_content = '\n'.join(lines)
    
    if new_content != original_content:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(new_content)
    
    return len(fixes_applied), fixes_applied

def main():
    endpoints_dir = Path('/Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Infrastructure/Endpoints')
    
    if not endpoints_dir.exists():
        print(f"Error: Directory not found: {endpoints_dir}")
        return
    
    total_fixes = 0
    files_modified = 0
    
    for filepath in sorted(endpoints_dir.glob('*.cs')):
        fixes_count, fixes = fix_file(str(filepath))
        if fixes_count > 0:
            files_modified += 1
            total_fixes += fixes_count
            print(f"\n{filepath.name}:")
            for fix in fixes:
                print(fix)
    
    print(f"\n{'='*60}")
    print(f"Summary: {total_fixes} fixes applied in {files_modified} files")

if __name__ == '__main__':
    main()
