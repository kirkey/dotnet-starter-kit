#!/usr/bin/env python3
"""
Comprehensive Accounting Endpoints Permission Audit & Fix Script
================================================================

This script audits ALL accounting endpoint domains (45+ domains, 250+ endpoints)
and ensures their permissions align with their workflows using FshActions.

Completed domains (SKIP):
- Accruals
- AccountReconciliation  
- AccountReconciliations
- AccountingPeriods
- AccountsPayableAccounts
- AccountsReceivableAccounts
- BankReconciliations

Pending domains (PROCESS): 38+ remaining domains
"""

import os
import re
from pathlib import Path
from dataclasses import dataclass
from typing import Dict, List, Optional, Tuple

# Workflow mapping: HTTP method/pattern → FshAction
WORKFLOW_MAPPING = {
    'MapGet': 'View',
    'MapPost /search': 'View',
    'MapPost / (create)': 'Create',
    'MapPost /{id}/start': 'Process',
    'MapPost /{id}/process': 'Process', 
    'MapPost /{id}/complete': 'Complete',
    'MapPost /{id}/approve': 'Approve',
    'MapPost /{id}/reject': 'Reject',
    'MapPost /{id}/void': 'Void',
    'MapPost /{id}/submit': 'Submit',
    'MapPost /{id}/cancel': 'Cancel',
    'MapPost /{id}/reconcile': 'Update',
    'MapPost /{id}/(record|post|send|collections|payments|discounts)': 'Post',
    'MapPost /{id}/receive': 'Receive',
    'MapPut /{id} (update)': 'Update',
    'MapPut /{id}/reverse': 'Update',
    'MapDelete /{id}': 'Delete',
}

@dataclass
class EndpointInfo:
    """Store endpoint analysis info"""
    file_path: Path
    domain: str
    endpoint_name: str
    http_method: str
    current_permission: Optional[str]
    expected_action: str
    needs_fix: bool
    issue: str = ""

def extract_http_method(content: str) -> str:
    """Extract HTTP method from endpoint code"""
    if '.MapGet(' in content:
        return 'GET'
    elif '.MapPost(' in content:
        if '/search' in content:
            return 'POST /search'
        elif '/{id' in content:
            # Check for specific action
            if '/approve' in content:
                return 'POST /{id}/approve'
            elif '/reject' in content:
                return 'POST /{id}/reject'
            elif '/complete' in content:
                return 'POST /{id}/complete'
            elif '/void' in content:
                return 'POST /{id}/void'
            elif '/start' in content:
                return 'POST /{id}/start'
            elif '/reconcile' in content:
                return 'POST /{id}/reconcile'
            return 'POST /{id}'
        return 'POST /'
    elif '.MapPut(' in content:
        if '/reverse' in content:
            return 'PUT /{id}/reverse'
        return 'PUT /{id}'
    elif '.MapDelete(' in content:
        return 'DELETE /{id}'
    return 'UNKNOWN'

def extract_permission(content: str) -> Optional[str]:
    """Extract current RequirePermission from endpoint"""
    match = re.search(r'RequirePermission\(FshPermission\.NameFor\(FshActions\.(\w+)', content)
    return match.group(1) if match else None

def determine_expected_action(http_method: str, endpoint_name: str) -> str:
    """Determine expected FshAction based on HTTP method and endpoint name"""
    
    # Direct mapping by HTTP method
    if http_method == 'GET':
        return 'View'
    elif http_method == 'POST /search':
        return 'View'
    elif http_method == 'POST /':
        return 'Create'
    elif http_method == 'PUT /{id}':
        return 'Update'
    elif http_method == 'DELETE /{id}':
        return 'Delete'
    elif http_method == 'PUT /{id}/reverse':
        return 'Update'
    elif http_method == 'POST /{id}/reconcile':
        return 'Update'
    
    # Specific POST actions
    elif http_method == 'POST /{id}/approve':
        return 'Approve'
    elif http_method == 'POST /{id}/reject':
        return 'Reject'
    elif http_method == 'POST /{id}/complete':
        return 'Complete'
    elif http_method == 'POST /{id}/void':
        return 'Void'
    elif http_method == 'POST /{id}/start':
        return 'Process'
    elif http_method == 'POST /{id}':
        # Check endpoint name for clues
        if 'Record' in endpoint_name or 'Post' in endpoint_name:
            return 'Post'
        elif 'Receive' in endpoint_name or 'Collection' in endpoint_name:
            return 'Receive'
        else:
            return 'Update'
    
    return 'UNKNOWN'

def audit_endpoint_file(file_path: Path, domain: str) -> EndpointInfo:
    """Audit a single endpoint file"""
    try:
        content = file_path.read_text()
        endpoint_name = file_path.stem.replace('Endpoint', '')
        
        http_method = extract_http_method(content)
        current_perm = extract_permission(content)
        expected_action = determine_expected_action(http_method, endpoint_name)
        
        # Check if permission is missing
        if current_perm is None:
            needs_fix = True
            issue = "MISSING RequirePermission"
        elif current_perm != expected_action:
            needs_fix = True
            issue = f"Incorrect: {current_perm} → should be {expected_action}"
        else:
            needs_fix = False
            issue = ""
        
        return EndpointInfo(
            file_path=file_path,
            domain=domain,
            endpoint_name=endpoint_name,
            http_method=http_method,
            current_permission=current_perm,
            expected_action=expected_action,
            needs_fix=needs_fix,
            issue=issue
        )
    except Exception as e:
        return EndpointInfo(
            file_path=file_path,
            domain=domain,
            endpoint_name=file_path.stem,
            http_method="ERROR",
            current_permission=None,
            expected_action="",
            needs_fix=True,
            issue=f"Error reading file: {str(e)}"
        )

def audit_all_endpoints() -> Dict[str, List[EndpointInfo]]:
    """Audit all accounting endpoints"""
    endpoint_base = Path('/Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/Accounting/Accounting.Infrastructure/Endpoints')
    
    # Skip these completed domains
    completed = {'Accruals', 'AccountReconciliation', 'AccountReconciliations', 'AccountingPeriods',
                 'AccountsPayableAccounts', 'AccountsReceivableAccounts', 'BankReconciliations'}
    
    results: Dict[str, List[EndpointInfo]] = {}
    
    for domain_dir in sorted(endpoint_base.iterdir()):
        if not domain_dir.is_dir() or domain_dir.name.startswith('.'):
            continue
        
        domain_name = domain_dir.name
        
        # Skip completed domains
        if domain_name in completed:
            continue
        
        v1_dir = domain_dir / 'v1'
        if not v1_dir.exists():
            continue
        
        results[domain_name] = []
        
        for endpoint_file in sorted(v1_dir.glob('*Endpoint.cs')):
            info = audit_endpoint_file(endpoint_file, domain_name)
            results[domain_name].append(info)
    
    return results

def print_audit_summary(results: Dict[str, List[EndpointInfo]]) -> None:
    """Print audit summary"""
    print("\n" + "=" * 100)
    print("📋 ACCOUNTING ENDPOINTS AUDIT SUMMARY")
    print("=" * 100)
    
    total_endpoints = 0
    total_needs_fix = 0
    
    for domain in sorted(results.keys()):
        endpoints = results[domain]
        total_endpoints += len(endpoints)
        needs_fix_count = sum(1 for e in endpoints if e.needs_fix)
        total_needs_fix += needs_fix_count
        
        status = "✅" if needs_fix_count == 0 else "⚠️"
        print(f"\n{status} {domain:35} ({len(endpoints):2} endpoints, {needs_fix_count} issues)")
        
        for endpoint in endpoints:
            if endpoint.needs_fix:
                print(f"   ❌ {endpoint.endpoint_name:40} | {endpoint.http_method:20} | {endpoint.issue}")
    
    print("\n" + "=" * 100)
    print(f"📊 TOTALS: {total_endpoints} endpoints | {total_needs_fix} need fixes")
    print("=" * 100 + "\n")

if __name__ == '__main__':
    results = audit_all_endpoints()
    print_audit_summary(results)

