# Help Dialogs Quick Reference Guide

**Quick lookup for all help dialogs created in this session**

---

## 🔍 **Find a Help Dialog**

| Module | Path | Status | Lines |
|--------|------|--------|-------|
| BankReconciliations | `/Pages/Accounting/BankReconciliations/BankReconciliationsHelpDialog.razor` | ✅ Live | 500+ |
| Banks | `/Pages/Accounting/Banks/BanksHelpDialog.razor` | ✅ Live | 450+ |
| Bills | `/Pages/Accounting/Bills/BillsHelpDialog.razor` | ✅ Live | 750+ |
| Budgets | `/Pages/Accounting/Budgets/BudgetsHelpDialog.razor` | ✅ Live | 450+ |
| Checks | `/Pages/Accounting/Checks/ChecksHelpDialog.razor` | ✅ Live | 550+ |
| Customers | `/Pages/Accounting/Customers/CustomersHelpDialog.razor` | ✅ Live | 500+ |
| CreditMemo | `/Pages/Accounting/CreditMemo/CreditMemoHelpDialog.razor` | ⚠️ Ready | 450+ |
| DebitMemo | `/Pages/Accounting/DebitMemo/DebitMemoHelpDialog.razor` | ⚠️ Ready | 400+ |
| DeferredRevenue | `/Pages/Accounting/DeferredRevenue/DeferredRevenueHelpDialog.razor` | ✅ Live | 550+ |

---

## 📋 **Key Topics by Module**

### BankReconciliations
- Reconciliation process and formula
- Outstanding checks and deposits
- Troubleshooting discrepancies
- Internal controls

### Banks
- Routing numbers (ABA)
- SWIFT codes
- Electronic payments (ACH vs Wire)
- Bank security

### Bills
- Three-way match
- Approval workflow
- Payment terms (2/10 Net 30)
- Fraud prevention

### Budgets
- Budget types (Operating, Capital, etc.)
- Budget vs Actual analysis
- Variance investigation
- Zero-based budgeting

### Checks
- Check lifecycle
- Issuance and voiding
- Stop payments
- Fraud prevention

### Customers
- Customer master file
- Credit management
- ABC analysis
- Collections

### CreditMemo
- Customer credits
- Returns and adjustments
- Application methods
- Fraud controls

### DebitMemo
- Vendor chargebacks
- Quality issues
- Vendor communication
- Dispute handling

### DeferredRevenue
- ASC 606 revenue recognition
- Subscription accounting
- Monthly recognition
- GAAP compliance

---

## 🎯 **Quick Access by Task**

**Bank Reconciliation:**
→ BankReconciliationsHelpDialog → "Reconciliation Process"

**Understanding Routing Numbers:**
→ BanksHelpDialog → "Routing Numbers"

**Three-Way Match:**
→ BillsHelpDialog → "Three-Way Match"

**Budget Variance Analysis:**
→ BudgetsHelpDialog → "Budget vs. Actual Analysis"

**Check Fraud Prevention:**
→ ChecksHelpDialog → "Check Security & Fraud Prevention"

**Customer Credit Limits:**
→ CustomersHelpDialog → "Credit Management"

**Issuing Credit Memos:**
→ CreditMemoHelpDialog → "Creating Credit Memos"

**Charging Vendors:**
→ DebitMemoHelpDialog → "Creating Debit Memos"

**Revenue Recognition:**
→ DeferredRevenueHelpDialog → "Revenue Recognition Principles"

---

## 📚 **Documentation Index**

1. **FINAL_SESSION_SUMMARY_COMPLETE.md** - Complete overview
2. **COMPLETE_HELP_DIALOGS_SESSION_SUMMARY.md** - Detailed summary
3. **BANK_RECONCILIATIONS_BANKS_BILLS_HELP_DIALOGS.md** - Set 1 details
4. **CREDITMEMO_DEBITMEMO_DEFERREDREVENUE_HELP_DIALOGS.md** - Set 3 details
5. **CREDITMEMO_DEBITMEMO_IMPLEMENTATION_TODO.md** - Pending implementation
6. **HELP_DIALOGS_QUICK_REFERENCE.md** - This file

---

## ⚡ **Integration Pattern**

For any page that needs help integration:

```csharp
// In .razor file - add toolbar button
<MudButton StartIcon="@Icons.Material.Filled.Help" OnClick="@ShowModuleHelp">
    Help
</MudButton>

// In .razor.cs file - add method
private async Task ShowModuleHelp()
{
    await DialogService.ShowAsync<ModuleHelpDialog>("Module Help", 
        new DialogParameters(), 
        new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
}
```

---

**Last Updated:** November 18, 2025  
**Total Modules:** 9  
**Status:** Production Ready

