# Accounting.Domain overview

This module contains the core accounting and utility-billing domain aggregates. All entities now include XML documentation (summaries, property docs, defaults) directly in the source.

Quick reference of aggregates and what they’re for:

- AccountingPeriod: Defines fiscal periods; controls posting via IsClosed; defaults IsClosed=false, IsAdjustmentPeriod=false.
- Accrual: Records obligations/revenue recognized before cash; enforces positive Amount; immutable after Reverse().
- Budget (+ BudgetLine): Annual/period budgets per account with status lifecycle; totals, actuals, variance helpers; defaults Status=Draft.
- ChartOfAccount: USOA-style accounts with type, hierarchy, normal balance, regulatory classification.
- ConsumptionData: Meter consumption snapshot; computes KWhUsed from readings and Multiplier; defaults Multiplier=1 if not provided.
- Customer: A/R customer with contacts, terms, credit limit, and CurrentBalance; defaults IsActive=true, CurrentBalance=0.
- DeferredRevenue: Revenue billed/received but not yet earned; recognize via Recognize().
- DepreciationMethod: Catalog of methods (code/name/formula) for FixedAssets; defaults IsActive=true.
- FixedAsset (+ DepreciationEntry): Asset purchase, service life, depreciation accounts, maintenance metadata; CurrentBookValue starts at PurchasePrice.
- FuelConsumption: Fuel used for generation with quantities, costs, BTU; computes TotalCost and optional efficiency.
- GeneralLedger: GL posting line tied to JournalEntry and Account; validates non-negative Debit/Credit and USOA class.
- InventoryItem: SKU with Quantity, UnitPrice, IsActive; stock adjustments with validation.
- Invoice (+ InvoiceLineItem): Billing document with usage/fixed/tax/fees and line items; status lifecycle; PaidAmount tracking.
- JournalEntry (+ JournalEntryLine): Balanced debit/credit entry with approval and posting lifecycle.
- Member: Utility member account with service location, meter assignment, status, and balance.
- Meter (+ MeterReading): Physical/smart meters with config and readings; validates sequences and status.
- PatronageCapital: Co-op allocation/retirement tracking per fiscal year; Status reflects retired state.
- Payee: Expense payee profile with default expense account mapping.
- Payment (+ PaymentAllocation): Incoming payments with UnappliedAmount and allocations across invoices.
- PostingBatch: Groups journal entries for approval and batch posting/reversal.
- Project (+ JobCostingEntry): Job costing with budget, cost/revenue tracking, and lifecycle.
- RateSchedule (+ RateTier): Utility rate structures with energy/demand rates, fixed charges, and optional tiers.
- RegulatoryReport: Regulatory filings (FERC/EIA/state) with period, workflow, and optional financial aggregates.
- SecurityDeposit: Member deposits with refund lifecycle and references.
- Vendor: Supplier profile with contacts, terms, and activation.

Notes
- All strings are trimmed on input; many have max lengths enforced where relevant.
- Monetary and quantity fields validate for non-negative/positive values as appropriate.
- Where useful, defaults are documented in the XML comments next to each property.

