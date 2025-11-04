# Bills Module - Quick Reference Guide

## Overview
The Bills module provides comprehensive vendor bill management with approval workflows, line item tracking, and general ledger integration.

---

## 🚀 Quick Start

### Creating a New Bill

1. **Navigate** to `/accounting/bills`
2. **Click** "Create" button or use the "New Bill" action button
3. **Fill in** bill header:
   - Bill Number (required)
   - Vendor (required - searchable dropdown)
   - Bill Date (required)
   - Due Date (required)
   - Accounting Period (optional)
   - Payment Terms (optional, e.g., "Net 30")
   - Purchase Order Number (optional)
   - Description (optional)
   - Notes (optional)

4. **Add Line Items** (at least one required):
   - Line Number (auto-increments)
   - Description (required)
   - Quantity (required, must be > 0)
   - Unit Price (required, >= 0)
   - Amount (calculated or manual)
   - Chart of Account (required - GL account)
   - Tax Code (optional)
   - Tax Amount (optional)
   - Project (optional)
   - Cost Center (optional)
   - Notes (optional)

5. **Review Totals**:
   - Subtotal (sum of line amounts)
   - Total Tax
   - Grand Total

6. **Click** "Save" to create bill in Draft status

---

## 📊 Bill Lifecycle

### Status Flow
```
Draft → Submitted → Approved → Posted → Paid
                  ↓
               Rejected
                  ↓
                Void
```

### Status Descriptions

- **Draft**: Newly created, editable
- **Submitted**: Sent for approval, pending review
- **Approved**: Approved for payment, ready to post
- **Rejected**: Rejected during approval, requires revision
- **Posted**: Posted to general ledger, largely immutable
- **Paid**: Payment processed and recorded
- **Void**: Cancelled/voided bill

---

## 🔄 Workflow Actions

### 1. Approve Bill
**When:** Bill is in Draft or Submitted status  
**Requirements:** 
- Approver name
**Effect:** 
- Status changes to Approved
- Records approver and approval date
- Bill is ready for posting

### 2. Reject Bill
**When:** Bill is in Draft or Submitted status  
**Requirements:** 
- Rejector name
- Rejection reason (required)
**Effect:** 
- Status changes to Rejected
- Records rejection details
- Bill can be revised

### 3. Post to General Ledger
**When:** Bill is Approved and not yet Posted  
**Requirements:** 
- Bill must be approved
**Effect:** 
- Status changes to Posted
- IsPosted flag set to true
- Bill becomes largely immutable
- GL entries created (integration point)

### 4. Mark as Paid
**When:** Bill is Posted and not yet Paid  
**Requirements:** 
- Payment date
**Effect:** 
- Status changes to Paid
- IsPaid flag set to true
- Records payment date
- Bill is fully processed

### 5. Void Bill
**When:** Bill is not yet Paid  
**Requirements:** 
- Void reason (required)
**Effect:** 
- Status changes to Void
- Records void reason
- Bill is cancelled

---

## 🎯 Action Navigation Menu

### Primary Actions
- **New Bill**: Opens create bill modal
- **Reports**: (Coming soon) Bill reports and analytics
- **Payment Batch**: (Coming soon) Batch payment processing

### Quick Filters
- **Pending Approvals**: Shows bills waiting for approval
- **Unposted Bills**: Shows approved bills ready to post
- **Unpaid Bills**: Shows posted bills awaiting payment

### Utility Actions
- **Aging Report**: (Coming soon) AP aging analysis
- **Export**: (Coming soon) Export bills to Excel
- **Settings**: (Coming soon) Bill module configuration

---

## 🔍 Search & Filter

### Advanced Search Options

**Text Search:**
- Bill Number (partial match)

**Status Filters:**
- Status (Draft, Submitted, Approved, Rejected, Posted, Paid, Void)
- Approval Status (Pending, Approved, Rejected)

**Date Filters:**
- Bill Date From/To
- Due Date From/To

**Boolean Filters:**
- Posted Only (toggle)
- Paid Only (toggle)

### Column Sorting
Click any column header to sort:
- Bill Number
- Vendor
- Bill Date
- Due Date
- Amount
- Status
- Approval Status

---

## ✏️ Editing Bills

### What Can Be Edited

**Draft/Submitted Bills:**
- ✅ All header fields
- ✅ Line items (add, edit, delete)
- ✅ Totals recalculate automatically

**Approved Bills (not posted):**
- ✅ Header fields only
- ❌ Line items locked

**Posted Bills:**
- ❌ Cannot edit
- Use Void to cancel if needed

**Paid Bills:**
- ❌ Cannot edit
- Fully locked

### Editing Line Items

**In-line Editor:**
- Click "+ Add Line" to add new line item
- Click "Edit" icon on existing line
- Click "Delete" icon to remove line
- Changes save on bill save
- Totals update in real-time

---

## 💡 Best Practices

### 1. Bill Numbering
- Use consistent bill number format
- Include vendor invoice number
- Example: "INV-2025-001" or "VENDOR-12345"

### 2. Date Management
- Bill Date = invoice date from vendor
- Due Date = payment due date (consider payment terms)
- Typical terms: Net 30 means due 30 days after bill date

### 3. Line Item Organization
- Use clear, descriptive line item descriptions
- Assign correct GL accounts for proper categorization
- Use projects/cost centers for departmental tracking
- Include tax codes where applicable

### 4. Approval Process
- Review totals before approving
- Verify GL account assignments
- Check for duplicate bills
- Confirm vendor and amounts match source documents

### 5. Posting Timing
- Post bills in appropriate accounting period
- Batch post multiple bills for efficiency
- Verify period is open before posting

### 6. Payment Tracking
- Mark as paid only after payment processed
- Record actual payment date
- Keep payment reference documentation

---

## 🔐 Permissions

Required permissions by action:

| Action | Permission |
|--------|-----------|
| View Bills | `Permissions.Accounting.View` |
| Create Bill | `Permissions.Accounting.Create` |
| Edit Bill | `Permissions.Accounting.Edit` |
| Delete Bill | `Permissions.Accounting.Delete` |
| Approve Bill | `Permissions.Bills.Approve` |
| Reject Bill | `Permissions.Bills.Reject` |
| Post Bill | `Permissions.Bills.Post` |
| Mark Paid | `Permissions.Bills.MarkPaid` |
| Void Bill | `Permissions.Bills.Void` |

---

## ⚠️ Common Issues & Solutions

### Issue: Cannot Edit Bill
**Cause:** Bill is posted or paid  
**Solution:** Posted/paid bills cannot be edited. Use Void to cancel if needed.

### Issue: Cannot Delete Bill
**Cause:** Bill is posted or paid  
**Solution:** Only draft bills can be deleted. Use Void for posted bills.

### Issue: Cannot Add Line Items
**Cause:** Bill is posted or paid  
**Solution:** Line items are locked on posted/paid bills. Edit before posting.

### Issue: Total Not Updating
**Cause:** Line items not saved  
**Solution:** Save bill to recalculate totals. Totals auto-update on save.

### Issue: Approval Button Missing
**Cause:** Wrong status or insufficient permissions  
**Solution:** Verify bill status and user permissions.

### Issue: Cannot Mark as Paid
**Cause:** Bill not posted yet  
**Solution:** Bill must be posted before marking as paid.

---

## 📱 Keyboard Shortcuts

| Key | Action |
|-----|--------|
| `Ctrl + N` | New Bill (when implemented) |
| `Ctrl + S` | Save Bill (in edit mode) |
| `Esc` | Close Modal |
| `Tab` | Navigate fields |
| `Enter` | Submit form |

---

## 🔗 Related Modules

- **Vendors**: Manage vendor information
- **Chart of Accounts**: GL account setup
- **Projects**: Project tracking and costing
- **Cost Centers**: Department/division tracking
- **Tax Codes**: Tax rate configuration
- **Accounting Periods**: Period management
- **General Ledger**: GL integration (future)
- **Payments**: Payment processing (future)

---

## 📞 Support

For issues or questions:
1. Check this quick reference
2. Review BILL_IMPLEMENTATION_REVIEW.md
3. Contact system administrator
4. Submit bug report if needed

---

## 🎓 Training Resources

### New User Training
1. Review bill lifecycle section
2. Practice creating test bills
3. Understand approval workflow
4. Learn search and filter options

### Advanced Features
1. Multi-line item management
2. Project/cost center allocation
3. Tax calculation setup
4. Batch operations (when available)

---

**Last Updated:** November 4, 2025  
**Version:** 1.0  
**Module Status:** Production Ready ✅

