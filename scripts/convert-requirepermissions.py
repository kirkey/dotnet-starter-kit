#!/usr/bin/env python3
"""
Scan HR endpoint files for RequirePermission("Permissions.X.Y") and replace with
.RequirePermission(FshPermission.NameFor(FshActions.Y, FshResources.<MappedResource>))

Usage:
  python3 scripts/convert-requirepermissions.py [--apply]

By default runs in dry-run mode and prints planned replacements.
"""
import re
import sys
import os
from pathlib import Path

REPO = Path(__file__).resolve().parents[1]
HR_ENDPOINTS = REPO / 'src' / 'api' / 'modules' / 'HumanResources' / 'HumanResources.Infrastructure' / 'Endpoints'

# Mapping from permission resource token to FshResources value (best-effort)
RESOURCE_MAP = {
    'Documents': 'Employees',
    'EmployeeDesignations': 'Employees',
    'LeaveRequests': 'Leaves',
    'LeaveBalances': 'Leaves',
    'LeaveTypes': 'Leaves',
    'EmployeeEducations': 'Employees',
    'Designations': 'Organization',
    'OrganizationalUnits': 'Organization',
    'Holidays': 'Organization',
    'EmployeeDependents': 'Employees',
    'PayComponentRates': 'Payroll',
    'Payrolls': 'Payroll',
    'Payroll': 'Payroll',
    'Timesheets': 'Timesheets',
    'TimesheetLines': 'Timesheets',
    'Shifts': 'Employees',
    'ShiftAssignments': 'Attendance',
    'BenefitAllocations': 'Benefits',
    'EmployeeContacts': 'Employees',
    'EmployeeEducations': 'Employees',
    'EmployeeDependents': 'Employees',
}

PERM_RE = re.compile(r"\.RequirePermission\(\s*\"Permissions\.([A-Za-z0-9_]+)\.([A-Za-z0-9_]+)\"\s*\)")

files_changed = []
plans = []

for path in HR_ENDPOINTS.rglob('*Endpoint.cs'):
    txt = path.read_text(encoding='utf-8')
    matches = list(PERM_RE.finditer(txt))
    if not matches:
        continue
    new_txt = txt
    for m in reversed(matches):
        resource_token = m.group(1)
        action_token = m.group(2)
        mapped = RESOURCE_MAP.get(resource_token, None)
        if mapped is None:
            # try simple plural->singular map: strip trailing 's'
            mapped = resource_token
            if mapped.endswith('ies'):
                mapped = mapped[:-3] + 'y'
            elif mapped.endswith('s'):
                mapped = mapped[:-1]
            # capitalize to Pascal case resource name (best-effort)
            mapped = mapped[0].upper() + mapped[1:]
        # ensure FshActions token capitalization: use action_token as-is
        replacement = f'.RequirePermission(FshPermission.NameFor(FshActions.{action_token}, FshResources.{mapped}))'
        start, end = m.span()
        new_txt = new_txt[:start] + replacement + new_txt[end:]
        plans.append((str(path), m.group(0), replacement))
    # ensure using Shared.Authorization present
    if 'FshPermission.NameFor' in new_txt and 'using Shared.Authorization;' not in new_txt:
        # insert after last using ...; line
        lines = new_txt.splitlines(True)
        last_using = -1
        for i,l in enumerate(lines):
            if l.strip().startswith('using ') and l.strip().endswith(';'):
                last_using = i
        insert_at = last_using + 1
        lines.insert(insert_at, 'using Shared.Authorization;\n')
        new_txt = ''.join(lines)
    if txt != new_txt:
        files_changed.append(path)
        if '--apply' in sys.argv:
            path.write_text(new_txt, encoding='utf-8')

# Output
if not plans:
    print('No string-based RequirePermission("Permissions.X.Y") patterns found in HR endpoints.')
else:
    print('Plans:')
    for p, old, new in plans:
        print(f'- {p}:')
        print(f'    {old}  ->  {new}')
    print('\nFiles affected:')
    for f in files_changed:
        print(' -', f)
    if '--apply' not in sys.argv:
        print('\nDry-run only. Re-run with --apply to apply the replacements.')
    else:
        print('\nApplied changes to files.')


