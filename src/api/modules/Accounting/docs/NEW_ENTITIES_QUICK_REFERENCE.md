# New Accounting Entities - Quick Reference Guide

## 🚀 Quick Start

This guide provides quick reference for the 7 new accounting entities implemented on October 31, 2025.

---

## 1. Bill (Accounts Payable Invoice)

### Purpose
Vendor invoice tracking for accounts payable with approval workflow.

### Quick Create
```csharp
var bill = Bill.Create(
    billNumber: "BILL-2025-001",
    vendorId: vendorId,
    vendorInvoiceNumber: "INV-98765",
    billDate: DateTime.Today,
    dueDate: DateTime.Today.AddDays(30),
    subtotalAmount: 5000.00m,
    taxAmount: 300.00m,
    shippingAmount: 100.00m,
    paymentTerms: "Net 30"
);
```

### Common Operations
```csharp
// Submit for approval
bill.SubmitForApproval();

// Approve
bill.Approve(approvedBy: "john.doe@company.com");

// Apply payment
bill.ApplyPayment(amount: 5400.00m, paymentDate: DateTime.Today);

// Add line item
bill.AddLineItem("Office supplies", quantity: 10, unitPrice: 50.00m);
```

### Key Status Flow
**Draft** → **PendingApproval** → **Approved** → **Paid** / **Void**

---

## 2. InterconnectionAgreement (Net Metering)

### Purpose
Track solar/wind customer generation and net metering credits.

### Quick Create
```csharp
var agreement = InterconnectionAgreement.Create(
    agreementNumber: "ICA-2025-001",
    memberId: memberId,
    generationType: "Solar",
    installedCapacityKW: 10.5m,
    interconnectionDate: DateTime.Today,
    netMeteringRate: 0.0875m,
    excessGenerationRate: 0.0350m,
    monthlyServiceCharge: 15.00m
);
```

### Common Operations
```csharp
// Activate after inspection
agreement.Activate();

// Record monthly generation
agreement.RecordGeneration(
    generationKWh: 1200.5m,
    consumptionKWh: 850.0m,
    periodDate: DateTime.Today
);

// Apply credit to invoice
agreement.ApplyCredit(amount: 30.62m, reason: "Net generation credit");
```

### Key Status Flow
**Pending** → **Active** → **Suspended** / **Terminated**

---

## 3. PowerPurchaseAgreement (Wholesale Power)

### Purpose
Track wholesale power purchase contracts and costs.

### Quick Create
```csharp
var ppa = PowerPurchaseAgreement.Create(
    contractNumber: "PPA-2025-001",
    counterpartyName: "ABC Solar Farm LLC",
    contractType: "Purchase",
    startDate: new DateTime(2025, 1, 1),
    endDate: new DateTime(2030, 12, 31),
    energyPricePerKWh: 0.0450m,
    minimumPurchaseKWh: 1000000m,
    energySource: "Solar"
);
```

### Common Operations
```csharp
// Record monthly settlement
ppa.RecordSettlement(
    energyKWh: 1500000m,
    settlementAmount: 67500.00m,
    settlementDate: DateTime.Today
);

// Apply price escalation
ppa.ApplyPriceEscalation();

// Suspend contract
ppa.Suspend(reason: "Counterparty maintenance");
```

### Key Status Flow
**Active** → **Suspended** / **Expired** / **Terminated**

---

## 4. Customer (General Customer Account)

### Purpose
General customer accounts beyond utility members.

### Quick Create
```csharp
var customer = Customer.Create(
    customerNumber: "CUST-2025-001",
    customerName: "ABC Corporation",
    customerType: "Business",
    billingAddress: "123 Main St, Anytown, ST 12345",
    email: "billing@abc.com",
    creditLimit: 50000.00m,
    paymentTerms: "Net 30"
);
```

### Common Operations
```csharp
// Update credit limit
customer.UpdateCreditLimit(newCreditLimit: 75000.00m, authorizedBy: "CFO");

// Place on credit hold
customer.PlaceOnCreditHold(reason: "Past due balance");

// Update balance (called by invoice/payment)
customer.UpdateBalance(amount: 5000.00m, transactionDate: DateTime.Today);

// Check credit
if (customer.IsOverCreditLimit) { /* handle */ }
var available = customer.AvailableCredit;
```

### Key Status Flow
**Active** → **CreditHold** / **Inactive** / **Collections**

---

## 5. PrepaidExpense

### Purpose
Track prepaid assets and systematic amortization.

### Quick Create
```csharp
var prepaid = PrepaidExpense.Create(
    prepaidNumber: "PREPAID-2025-001",
    description: "Annual insurance premium - FY2025",
    totalAmount: 12000.00m,
    startDate: new DateTime(2025, 1, 1),
    endDate: new DateTime(2025, 12, 31),
    prepaidAssetAccountId: prepaidAssetAccountId,
    expenseAccountId: expenseAccountId,
    paymentDate: DateTime.Today,
    amortizationSchedule: "Monthly"
);
```

### Common Operations
```csharp
// Calculate monthly amount
var monthlyAmount = prepaid.CalculateMonthlyAmortization();

// Record monthly amortization
prepaid.RecordAmortization(
    amortizationAmount: monthlyAmount,
    postingDate: DateTime.Today,
    journalEntryId: jeId
);

// Check status
if (prepaid.IsFullyAmortized) { prepaid.Close(); }
```

### Key Status Flow
**Active** → **FullyAmortized** → **Closed**

---

## 6. InterCompanyTransaction

### Purpose
Track transactions between legal entities for consolidation.

### Quick Create
```csharp
var icTrans = InterCompanyTransaction.Create(
    transactionNumber: "IC-2025-001",
    fromEntityId: parentEntityId,
    fromEntityName: "Parent Company Inc.",
    toEntityId: subsidiaryEntityId,
    toEntityName: "Subsidiary LLC",
    transactionDate: DateTime.Today,
    amount: 25000.00m,
    transactionType: "Billing",
    fromAccountId: icReceivableAccountId,
    toAccountId: icPayableAccountId
);
```

### Common Operations
```csharp
// Match with counterparty transaction
icTrans.MatchWith(matchingTransactionId: otherTransId);

// Reconcile
icTrans.Reconcile(reconciledBy: "accounting@company.com");

// Record settlement
icTrans.RecordSettlement(settlementDate: DateTime.Today);

// Post elimination for consolidation
icTrans.PostElimination();
```

### Key Status Flow
**Pending** → **Matched** → **Reconciled** → **Closed**

---

## 7. RetainedEarnings

### Purpose
Annual retained earnings tracking for equity reporting.

### Quick Create
```csharp
var re = RetainedEarnings.Create(
    fiscalYear: 2025,
    openingBalance: 500000.00m,
    fiscalYearStartDate: new DateTime(2025, 1, 1),
    fiscalYearEndDate: new DateTime(2025, 12, 31)
);
```

### Common Operations
```csharp
// Update net income at year-end
re.UpdateNetIncome(netIncome: 125000.00m);

// Record distribution
re.RecordDistribution(
    amount: 75000.00m,
    distributionDate: DateTime.Today,
    distributionType: "Patronage Refund"
);

// Record capital contribution
re.RecordCapitalContribution(
    amount: 50000.00m,
    contributionDate: DateTime.Today,
    contributionType: "Member Capital"
);

// Appropriate retained earnings
re.AppropriateRetainedEarnings(
    amount: 100000.00m,
    purpose: "Capital Projects Reserve"
);

// Close fiscal year
re.Close(closedBy: "CFO");
```

### Key Calculation
```
Closing Balance = Opening + Net Income - Distributions + Capital Contributions + Other Changes
```

---

## 🔍 Common Patterns

### Entity Creation
All entities use static `Create()` factory methods:
```csharp
var entity = EntityName.Create(
    required_param1,
    required_param2,
    optional_param1: value,
    description: "Optional description",
    notes: "Optional notes"
);
```

### Entity Updates
All entities have `Update()` methods with optional parameters:
```csharp
entity.Update(
    field1: newValue1,
    field2: newValue2,
    description: updatedDescription
);
```

### Status Transitions
Most entities have lifecycle methods:
```csharp
entity.Activate();
entity.Suspend(reason: "...");
entity.Terminate(reason: "...");
entity.Close();
```

### Domain Events
All operations publish domain events automatically:
```csharp
// Events are queued in entity.DomainEvents collection
// Published by infrastructure after saving
```

---

## 🎯 Integration Points

### Bill → Vendor, PurchaseOrder, Payment
```csharp
// Create bill from vendor
var bill = Bill.Create(vendorId: vendor.Id, ...);

// Link to purchase order
bill = Bill.Create(purchaseOrderId: po.Id, ...);

// Apply payment
bill.ApplyPayment(amount, paymentDate);
```

### InterconnectionAgreement → Member, Meter, Invoice
```csharp
// Create for member
var agreement = InterconnectionAgreement.Create(memberId: member.Id, ...);

// Link meter
agreement.Update(meterId: meter.Id);

// Credits applied to invoices
invoice.AddCredit(agreement.CurrentCreditBalance);
```

### PowerPurchaseAgreement → Vendor
```csharp
// Link vendor for payments
var ppa = PowerPurchaseAgreement.Create(vendorId: vendor.Id, ...);
```

### Customer → Invoice, Payment
```csharp
// Create invoice for customer
var invoice = Invoice.Create(customerId: customer.Id, ...);

// Update customer balance
customer.UpdateBalance(invoice.TotalAmount, DateTime.Today);
```

### PrepaidExpense → Vendor, JournalEntry
```csharp
// Create from vendor payment
var prepaid = PrepaidExpense.Create(
    vendorId: vendor.Id,
    paymentId: payment.Id,
    ...
);

// Link journal entry
prepaid.RecordAmortization(journalEntryId: je.Id, ...);
```

---

## ⚠️ Common Validations

### Amount Validations
```csharp
// Amounts must be positive (unless explicitly allowing negative)
if (amount <= 0) throw new ArgumentException("Amount must be positive");
```

### Date Validations
```csharp
// End date must be after start date
if (endDate <= startDate) throw new ArgumentException("Invalid date range");

// Due date must be on or after transaction date
if (dueDate < transactionDate) throw new ArgumentException("Invalid due date");
```

### Status Validations
```csharp
// Cannot modify closed/paid/reconciled entities
if (IsClosed) throw new InvalidOperationException("Cannot modify closed entity");
```

### Required Fields
```csharp
// Check for null/empty strings
if (string.IsNullOrWhiteSpace(value)) 
    throw new ArgumentException("Field is required");
```

---

## 📊 Reporting Queries

### Accounts Payable Aging
```csharp
// Bills grouped by age
var aging = bills
    .Where(b => b.Status != "Paid" && b.Status != "Void")
    .GroupBy(b => b.DaysPastDue switch {
        <= 0 => "Current",
        <= 30 => "1-30 Days",
        <= 60 => "31-60 Days",
        <= 90 => "61-90 Days",
        _ => "Over 90 Days"
    });
```

### Net Metering Summary
```csharp
// Active agreements with generation
var summary = agreements
    .Where(a => a.IsActive)
    .Select(a => new {
        a.MemberId,
        a.InstalledCapacityKW,
        a.YearToDateGeneration,
        a.CurrentCreditBalance
    });
```

### Power Purchase Cost Analysis
```csharp
// PPA costs by counterparty
var costs = ppas
    .Where(p => p.IsActive)
    .GroupBy(p => p.CounterpartyName)
    .Select(g => new {
        Counterparty = g.Key,
        TotalCost = g.Sum(p => p.YearToDateCost),
        TotalEnergy = g.Sum(p => p.YearToDateEnergyKWh),
        AvgCost = g.Average(p => p.AverageCostPerKWh)
    });
```

### Customer Credit Analysis
```csharp
// Customers over credit limit
var overLimit = customers
    .Where(c => c.IsOverCreditLimit)
    .Select(c => new {
        c.CustomerNumber,
        c.CustomerName,
        c.CurrentBalance,
        c.CreditLimit,
        OverAmount = c.CurrentBalance - c.CreditLimit,
        c.CreditUtilizationPercentage
    });
```

### Prepaid Expense Schedule
```csharp
// Unamortized prepaid expenses
var prepaidSchedule = prepaidExpenses
    .Where(p => !p.IsFullyAmortized && p.Status == "Active")
    .Select(p => new {
        p.PrepaidNumber,
        p.Description,
        p.TotalAmount,
        p.RemainingAmount,
        p.NextAmortizationDate,
        p.AmortizationPercentage
    });
```

---

## 🔒 Security Considerations

### Authorization Required
- **Bill Approval:** Requires manager/supervisor role
- **Credit Limit Changes:** Requires CFO/controller authorization
- **Year-End Closing:** Requires accounting manager approval
- **Inter-Company Reconciliation:** Requires corporate accounting
- **Prepaid Cancellation:** Requires proper authorization

### Audit Trail
All entities inherit from `AuditableEntity`:
- `CreatedBy` / `CreatedOn`
- `LastModifiedBy` / `LastModifiedOn`
- Domain events for all significant actions

---

## 📚 Related Documentation

- **Full Implementation Summary:** `NEW_ENTITIES_IMPLEMENTATION_SUMMARY.md`
- **System Review:** `ACCOUNTING_SYSTEM_REVIEW_POWER_UTILITY.md`
- **Existing Patterns:** See Catalog and Todo modules for CQRS examples

---

**Last Updated:** October 31, 2025  
**Version:** 1.0  
**Status:** Domain Layer Complete ✅

