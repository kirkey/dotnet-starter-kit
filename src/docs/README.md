# 📚 Documentation Index

Welcome to the comprehensive documentation for the .NET Starter Kit project.

## 📁 Documentation Structure

All documentation has been organized into feature-specific folders for easy navigation and maintenance.

### 🗂️ Quick Navigation

| Category | Location | Description |
|----------|----------|-------------|
| 📊 **Accounting** | [`./accounting/`](./accounting/) | Accounting module documentation |
| 🏪 **Store** | [`./store/`](./store/) | Store & inventory management |
| 🏭 **Warehouse** | [`./warehouse/`](./warehouse/) | Warehouse operations |
| 📦 **Inventory Counting** | [`./inventory-counting/`](./inventory-counting/) | Cycle counting & mobile counting |
| 🎨 **Blazor UI** | [`./blazor-ui/`](./blazor-ui/) | Frontend UI best practices |
| 🔌 **SignalR** | [`./signalr/`](./signalr/) | Real-time communication |
| 🏗️ **Architecture** | [`./architecture/`](./architecture/) | System architecture & patterns |

---

## 📊 Accounting Module

**Location:** [`./accounting/`](./accounting/)

Comprehensive accounting system documentation covering:
- Chart of Accounts
- General Ledger & Trial Balance  
- Journal Entries & Posting Batches
- Accounts Receivable & Payable
- Banks, Checks & Reconciliations
- Invoices, Bills & Credit/Debit Memos
- Fiscal Period Close & Retained Earnings
- Budgets, Projects & Cost Centers
- Accruals, Deferrals & Prepaid Expenses
- Tax Codes & Regulatory Compliance
- Fixed Assets & Depreciation
- Meters & Consumption Tracking

**Key Documents:**
- [Best Practices Review](./accounting/ACCOUNTING_BEST_PRACTICES_REVIEW.md)
- [Quick Reference Guide](./accounting/ACCOUNTING_DOCS_QUICK_REFERENCE.md)
- [Chart of Accounts](./accounting/CHART_OF_ACCOUNTS_REVIEW_COMPLETE.md)
- [General Ledger & Trial Balance](./accounting/GENERAL_LEDGER_TRIAL_BALANCE_REVIEW_COMPLETE.md)
- [Banks, Checks & Reconciliations](./accounting/BANKS_CHECKS_RECONCILIATIONS_PAYMENTS_REVIEW_COMPLETE.md)

---

## 🏪 Store Module

**Location:** [`./store/`](./store/)

Store and inventory management system documentation:
- Purchase Orders & Supplier Management
- Goods Receipts & Stock Adjustments
- Items, Categories & SKUs
- Stock Levels & Reservations
- Dashboard & Analytics

**Key Documents:**
- [Best Practices](./store/STORE_WAREHOUSE_BEST_PRACTICES_COMPLETE.md)
- [Quick Reference](./store/STORE_WAREHOUSE_QUICK_REFERENCE.md)
- [Purchase Order Flow](./store/PURCHASE_ORDER_GOODS_RECEIPT_FLOW_SUMMARY.md)
- [Auto-Reorder Feature](./store/PURCHASE_ORDER_AUTO_REORDER_FEATURE.md)
- [Dashboard Enhancements](./store/STORE_DASHBOARD_ENHANCEMENTS.md)

---

## 🏭 Warehouse Module

**Location:** [`./warehouse/`](./warehouse/)

Warehouse operations and management documentation:
- Warehouses & Locations
- Bins & Storage Management
- Put-Away Tasks
- Pick Lists & Fulfillment
- Inventory Transfers
- Master Data Management

**Key Documents:**
- [Warehouses UI Implementation](./warehouse/WAREHOUSES_UI_IMPLEMENTATION_COMPLETE.md)
- [Warehouse Locations](./warehouse/WAREHOUSE_LOCATIONS_UI_IMPLEMENTATION_COMPLETE.md)
- [Operations & Master Data](./warehouse/WAREHOUSE_OPERATIONS_MASTER_DATA_REVIEW_COMPLETE.md)
- [Inventory Management](./warehouse/INVENTORY_MANAGEMENT_REVIEW_COMPLETE.md)

---

## 📦 Inventory Counting

**Location:** [`./inventory-counting/`](./inventory-counting/)

Comprehensive cycle counting and physical inventory documentation:
- Cycle Count Planning & Execution
- Mobile Counting Interface
- Barcode Scanning
- Multi-Warehouse Counting
- Accuracy Tracking & Reporting

**Key Documents:**
- [📖 Index](./inventory-counting/INVENTORY_COUNTING_INDEX.md) - **START HERE**
- [Executive Summary](./inventory-counting/INVENTORY_COUNTING_EXECUTIVE_SUMMARY.md)
- [Technical Guide](./inventory-counting/INVENTORY_COUNTING_TECHNICAL_GUIDE.md)
- [Quick Reference](./inventory-counting/INVENTORY_COUNTING_QUICK_REFERENCE.md)
- [Mobile Cycle Count Implementation](./inventory-counting/MOBILE_CYCLE_COUNT_IMPLEMENTATION.md)
- [Mobile Index](./inventory-counting/MOBILE_CYCLE_COUNT_INDEX.md)
- [Multi-Warehouse Guide](./inventory-counting/MULTI_WAREHOUSE_INVENTORY_COUNTING_GUIDE.md)
- [Barcode Scanner Setup](./inventory-counting/BARCODE_SCANNER_SETUP.md)

---

## 🎨 Blazor UI

**Location:** [`./blazor-ui/`](./blazor-ui/)

Frontend user interface best practices and guidelines:
- Component Architecture
- Page Patterns
- Form Validation
- Data Binding
- Dialog Management
- State Management

**Key Documents:**
- [Best Practices](./blazor-ui/BLAZOR_CLIENT_UI_BEST_PRACTICES.md)
- [Quick Reference](./blazor-ui/BLAZOR_CLIENT_UI_QUICK_REFERENCE.md)

---

## 🔌 SignalR Real-Time Communication

**Location:** [`./signalr/`](./signalr/)

Real-time communication implementation using SignalR:
- Hub Architecture
- Connection Management
- Event Broadcasting
- Client Integration
- Testing & Debugging

**Key Documents:**
- [README](./signalr/README_SIGNALR.md)
- [Quickstart Guide](./signalr/SIGNALR_QUICKSTART.md)
- [Implementation Summary](./signalr/SIGNALR_IMPLEMENTATION_SUMMARY.md)
- [Architecture Diagram](./signalr/SIGNALR_ARCHITECTURE_DIAGRAM.md)
- [Testing Guide](./signalr/SIGNALR_TESTING_GUIDE.md)

---

## 🏗️ Architecture & Patterns

**Location:** [`./architecture/`](./architecture/)

System architecture, design patterns, and coding standards:
- CQRS Implementation
- Endpoint Patterns
- Repository Pattern
- Specification Pattern
- Validation Strategy

**Key Documents:**
- [CQRS Implementation Checklist](./architecture/CQRS_IMPLEMENTATION_CHECKLIST.md)
- [Endpoint Pattern Review](./architecture/ENDPOINT_PATTERN_REVIEW.md)

---

## 📝 Documentation Guidelines

### When Creating New Documentation

1. **Place in correct folder** - Match the feature/module
2. **Use descriptive names** - Clear and specific
3. **Include metadata** - Date, author, version
4. **Link related docs** - Create cross-references
5. **Update this index** - Keep navigation current

### Naming Conventions

- Use UPPERCASE with underscores for major guides
- Use descriptive names: `FEATURE_DESCRIPTION_TYPE.md`
- Examples:
  - `ACCOUNTING_BEST_PRACTICES_REVIEW.md`
  - `MOBILE_CYCLE_COUNT_IMPLEMENTATION.md`
  - `SIGNALR_QUICKSTART.md`

### File Organization

```
docs/
├── README.md (this file)
├── accounting/
├── store/
├── warehouse/
├── inventory-counting/
├── blazor-ui/
├── signalr/
└── architecture/
```

---

## 🔍 Quick Search

Looking for something specific? Use these keywords:

- **Accounting:** Chart of Accounts, GL, Journal Entries, AP, AR, Banks
- **Store:** Purchase Orders, Goods Receipts, Items, Stock
- **Warehouse:** Locations, Bins, Put-Away, Pick Lists
- **Counting:** Cycle Counts, Mobile, Barcode Scanner
- **UI:** Blazor, Components, Forms, Dialogs
- **Real-time:** SignalR, Notifications, Events
- **Patterns:** CQRS, Repository, Specification, Validation

---

## 📞 Getting Help

1. Check the relevant module's folder
2. Review the Quick Reference guides
3. Consult the Best Practices documents
4. Check Architecture patterns for design guidance

---

**Last Updated:** November 11, 2025  
**Total Documentation Files:** 60+  
**Organization:** Feature-based folders for maintainability

