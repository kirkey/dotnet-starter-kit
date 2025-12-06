#!/usr/bin/env python3
"""
Script to fix incorrect FshActions mappings in MicroFinance endpoints.
Maps endpoint operation names to correct action types.

Rules:
1. Action keyword at START of name takes priority (CreateLoan -> Create)
2. Action keyword as standalone operation (Deposit, Withdraw) gets that action
3. Names like GetXXX, SearchXXX use View/Search
4. Names with workflow verbs (Approve, Reject, etc.) use those actions
"""
import os
import re
from pathlib import Path

def get_correct_action(endpoint_name: str) -> str:
    """
    Determine the correct action based on the endpoint name.
    Priority: Start patterns > Specific patterns > Default
    """
    name = endpoint_name
    
    # 1. Check for CRUD at start
    if name.startswith('Create'):
        return 'Create'
    if name.startswith('Update'):
        return 'Update'
    if name.startswith('Delete'):
        return 'Delete'
    if name.startswith('Get'):
        return 'View'
    if name.startswith('Search'):
        return 'Search'
    
    # 2. Check for workflow actions at start
    if name.startswith('Approve'):
        return 'Approve'
    if name.startswith('Reject'):
        return 'Reject'
    if name.startswith('Submit'):
        return 'Submit'
    if name.startswith('Complete'):
        return 'Complete'
    if name.startswith('Cancel'):
        return 'Cancel'
    if name.startswith('Process'):
        return 'Process'
    if name.startswith('Disburse'):
        return 'Disburse'
    if name.startswith('Close'):
        return 'Close'
    if name.startswith('Post'):
        return 'Post'
    if name.startswith('Void'):
        return 'Void'
    
    # 3. Check for financial transaction actions at start
    if name.startswith('Deposit'):
        return 'Deposit'
    if name.startswith('Withdraw'):
        return 'Withdraw'
    if name.startswith('Transfer'):
        return 'Transfer'
    if name.startswith('Freeze'):
        return 'Freeze'
    if name.startswith('Unfreeze'):
        return 'Unfreeze'
    if name.startswith('WriteOff'):
        return 'WriteOff'
    if name.startswith('Mature'):
        return 'Mature'
    if name.startswith('Renew'):
        return 'Renew'
    if name.startswith('Pay'):
        return 'Process'  # Pay operations are processing payments
    if name.startswith('Record'):
        return 'Create'  # Record operations are creating records
    
    # 4. Check for action verbs anywhere in name (for compound names)
    # Only if no start pattern matched
    
    # Default to View for read operations
    return 'View'

def fix_file(filepath: str) -> tuple[int, list[str]]:
    """Fix action mappings in a single file."""
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    
    original_content = content
    fixes_applied = []
    
    lines = content.split('\n')
    current_endpoint_name = None
    
    for i in range(len(lines)):
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
