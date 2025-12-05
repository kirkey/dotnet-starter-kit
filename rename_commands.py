#!/usr/bin/env python3
"""
Script to rename commands from [Entity][Action]Command to [Action][Entity]Command pattern.
Also updates all references in related files.
"""

import os
import re
import glob

# Define the mappings: (old_name, new_name)
RENAMES = [
    # AccountingPeriod
    ("AccountingPeriodCloseCommand", "CloseAccountingPeriodCommand"),
    ("AccountingPeriodCloseHandler", "CloseAccountingPeriodHandler"),
    ("AccountingPeriodReopenCommand", "ReopenAccountingPeriodCommand"),
    ("AccountingPeriodReopenHandler", "ReopenAccountingPeriodHandler"),
    
    # Bank
    ("BankCreateCommand", "CreateBankCommand"),
    ("BankCreateHandler", "CreateBankHandler"),
    ("BankUpdateCommand", "UpdateBankCommand"),
    ("BankUpdateHandler", "UpdateBankHandler"),
    ("BankDeleteCommand", "DeleteBankCommand"),
    ("BankDeleteHandler", "DeleteBankHandler"),
    
    # Bill
    ("BillCreateCommand", "CreateBillCommand"),
    ("BillCreateHandler", "CreateBillHandler"),
    ("BillUpdateCommand", "UpdateBillCommand"),
    ("BillUpdateHandler", "UpdateBillHandler"),
    
    # Check
    ("CheckCreateCommand", "CreateCheckCommand"),
    ("CheckCreateHandler", "CreateCheckHandler"),
    ("CheckUpdateCommand", "UpdateCheckCommand"),
    ("CheckUpdateHandler", "UpdateCheckHandler"),
    ("CheckIssueCommand", "IssueCheckCommand"),
    ("CheckIssueHandler", "IssueCheckHandler"),
    ("CheckVoidCommand", "VoidCheckCommand"),
    ("CheckVoidHandler", "VoidCheckHandler"),
    ("CheckClearCommand", "ClearCheckCommand"),
    ("CheckClearHandler", "ClearCheckHandler"),
    
    # Customer
    ("CustomerCreateCommand", "CreateCustomerCommand"),
    ("CustomerCreateHandler", "CreateCustomerHandler"),
    ("CustomerUpdateCommand", "UpdateCustomerCommand"),
    ("CustomerUpdateHandler", "UpdateCustomerHandler"),
    
    # Payee
    ("PayeeCreateCommand", "CreatePayeeCommand"),
    ("PayeeCreateHandler", "CreatePayeeHandler"),
    ("PayeeUpdateCommand", "UpdatePayeeCommand"),
    ("PayeeUpdateHandler", "UpdatePayeeHandler"),
    ("PayeeDeleteCommand", "DeletePayeeCommand"),
    ("PayeeDeleteHandler", "DeletePayeeHandler"),
    
    # Payment
    ("PaymentCreateCommand", "CreatePaymentCommand"),
    ("PaymentCreateHandler", "CreatePaymentHandler"),
    ("PaymentUpdateCommand", "UpdatePaymentCommand"),
    ("PaymentUpdateHandler", "UpdatePaymentHandler"),
    ("PaymentDeleteCommand", "DeletePaymentCommand"),
    ("PaymentDeleteHandler", "DeletePaymentHandler"),
    
    # Vendor
    ("VendorCreateCommand", "CreateVendorCommand"),
    ("VendorCreateHandler", "CreateVendorHandler"),
    ("VendorUpdateCommand", "UpdateVendorCommand"),
    ("VendorUpdateHandler", "UpdateVendorHandler"),
    ("VendorDeleteCommand", "DeleteVendorCommand"),
    ("VendorDeleteHandler", "DeleteVendorHandler"),
    
    # PostingBatch
    ("PostingBatchCreateCommand", "CreatePostingBatchCommand"),
    ("PostingBatchCreateHandler", "CreatePostingBatchHandler"),
    ("PostingBatchApproveCommand", "ApprovePostingBatchCommand"),
    ("PostingBatchApproveHandler", "ApprovePostingBatchHandler"),
    ("PostingBatchRejectCommand", "RejectPostingBatchCommand"),
    ("PostingBatchRejectHandler", "RejectPostingBatchHandler"),
    ("PostingBatchPostCommand", "PostPostingBatchCommand"),
    ("PostingBatchPostHandler", "PostPostingBatchHandler"),
    ("PostingBatchReverseCommand", "ReversePostingBatchCommand"),
    ("PostingBatchReverseHandler", "ReversePostingBatchHandler"),
    
    # TrialBalance
    ("TrialBalanceCreateCommand", "CreateTrialBalanceCommand"),
    ("TrialBalanceCreateHandler", "CreateTrialBalanceHandler"),
    ("TrialBalanceFinalizeCommand", "FinalizeTrialBalanceCommand"),
    ("TrialBalanceFinalizeHandler", "FinalizeTrialBalanceHandler"),
    ("TrialBalanceReopenCommand", "ReopenTrialBalanceCommand"),
    ("TrialBalanceReopenHandler", "ReopenTrialBalanceHandler"),
]

def find_files_to_update(base_path):
    """Find all C# files that might contain references."""
    patterns = [
        os.path.join(base_path, "src", "**", "*.cs"),
        os.path.join(base_path, "src", "**", "*.razor"),
        os.path.join(base_path, "src", "**", "*.razor.cs"),
    ]
    files = []
    for pattern in patterns:
        files.extend(glob.glob(pattern, recursive=True))
    return files

def update_file_content(file_path, renames):
    """Update file content with new names."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
    except:
        return False, 0
    
    original = content
    changes = 0
    
    for old_name, new_name in renames:
        if old_name in content:
            content = content.replace(old_name, new_name)
            changes += 1
    
    if content != original:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        return True, changes
    
    return False, 0

def rename_file(old_path, old_name, new_name):
    """Rename a file."""
    if not os.path.exists(old_path):
        return False
    
    directory = os.path.dirname(old_path)
    new_path = os.path.join(directory, new_name + ".cs")
    
    if os.path.exists(new_path):
        print(f"  WARNING: Target already exists: {new_path}")
        return False
    
    os.rename(old_path, new_path)
    print(f"  Renamed: {old_name}.cs -> {new_name}.cs")
    return True

def main():
    base_path = '/Users/kirkeypsalms/Projects/dotnet-starter-kit'
    
    print("=== Step 1: Update all file contents ===")
    files = find_files_to_update(base_path)
    print(f"Found {len(files)} files to check")
    
    updated_files = 0
    for file_path in files:
        modified, changes = update_file_content(file_path, RENAMES)
        if modified:
            updated_files += 1
            print(f"  Updated: {file_path} ({changes} changes)")
    
    print(f"\nUpdated {updated_files} files")
    
    print("\n=== Step 2: Rename files ===")
    # Find and rename Command/Handler files
    app_path = os.path.join(base_path, "src", "api", "modules", "Accounting", "Accounting.Application")
    
    for old_name, new_name in RENAMES:
        # Find files matching old_name
        pattern = os.path.join(app_path, "**", f"{old_name}.cs")
        matching_files = glob.glob(pattern, recursive=True)
        
        for old_path in matching_files:
            rename_file(old_path, old_name, new_name)
    
    print("\n=== Done ===")
    print("Note: You may need to regenerate NSwag clients after this change.")

if __name__ == '__main__':
    main()
