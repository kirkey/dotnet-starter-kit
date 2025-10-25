# API Documentation Organization

## Overview
This document provides an overview of the documentation structure for the Web API modules.

**Last Updated:** October 25, 2025

---

## Documentation Structure

### Shared Documentation (`/api/docs/`)
Contains cross-cutting documentation that applies to multiple modules or provides general architectural guidance.

**Current Documents:**
- `SPECIFICATION_PATTERN_GUIDE.md` - Guide for implementing the Specification pattern across all modules

---

## Module-Specific Documentation

### Accounting Module (`/api/modules/Accounting/docs/`)
Documentation for the Accounting module including:
- Bank management implementation
- Check management and processing
- Debit/Credit memos
- Chart of accounts
- Advanced accounting entities
- Configuration implementations
- Blazor UI implementations

**Total Documents:** 30+ files

**Key Documents:**
- `README.md` - Module overview
- `CHECK_QUICK_START_FINAL.md` - Quick start guide for check management
- `BANK_MANAGEMENT_IMPLEMENTATION.md` - Bank management features
- `ACCOUNTING_APPLICATIONS_FINAL_SUMMARY.md` - Application layer summary

### Store Module (`/api/modules/Store/docs/`)
Documentation for the Store/Inventory module including:
- Warehouse management
- Inventory tracking (bins, lots, serial numbers)
- Purchase orders and goods receipts
- Stock levels and reservations
- Pick lists and put-away tasks
- Cycle counting
- Import/export functionality
- Blazor UI implementations

**Total Documents:** 43+ files

**Key Documents:**
- `README.md` - Module overview
- `STORE_QUICK_REFERENCE.md` - Quick reference guide
- `IMPLEMENTATION_COMPLETE.md` - Implementation status
- `CYCLE_COUNT_QUICK_REFERENCE.md` - Cycle count features
- `STORE_IMPORT_EXPORT_GUIDE.md` - Import/export guide

---

## UI-Specific Documentation

### Blazor Client Store Pages (`/apps/blazor/client/Pages/Store/Docs/`)
UI-specific implementation documentation for Store pages:
- `GOODS_RECEIPTS_UI_IMPLEMENTATION.md` - Goods receipt UI implementation
- `IMPLEMENTATION_STATUS.md` - UI implementation status

---

## Framework Documentation

### Infrastructure Storage (`/api/framework/Infrastructure/Storage/`)
Documentation for storage and file handling infrastructure:
- `IMPLEMENTATION_SUMMARY.md` - Storage implementation summary
- `IMPORT_EXPORT_GUIDE.md` - Import/export infrastructure guide

---

## Documentation Standards

### File Naming Conventions
- Use UPPERCASE with underscores for implementation docs: `FEATURE_IMPLEMENTATION.md`
- Use PascalCase for entity-specific docs: `Store_Entity_Implementation_Complete.md`
- Use README.md for module overviews
- Use descriptive names that clearly indicate the content

### Content Guidelines
- Include date of creation/last update
- Provide clear summary at the beginning
- Use proper markdown formatting
- Include code examples where applicable
- Document both backend API and frontend UI changes
- Reference related files and implementations

---

## Recently Organized (October 25, 2025)

The following documentation files were moved from the project root to their appropriate module folders:

**Moved to Accounting (`/api/modules/Accounting/docs/`):**
- `SPECIFICATION_REFACTORING_SUMMARY.md` - Accounting module specification refactoring
- `BANK_MANAGEMENT_PAGE_IMPLEMENTATION.md` - Bank management Blazor page

**Moved to Store (`/api/modules/Store/docs/`):**
- `CYCLECOUNT_PATTERN_ALIGNMENT.md` - Cycle count pattern implementation
- `CYCLE_COUNT_REVIEW.md` - Cycle count review and improvements
- `IMPORT_OPTIMIZATION_SUMMARY.md` - Store items import optimization
- `IMPORT_RESPONSE_UPDATES.md` - Import response handling updates
- `PURCHASE_ORDER_PDF_IMPLEMENTATION.md` - Purchase order PDF generation

**Moved to Shared API Docs (`/api/docs/`):**
- `SPECIFICATION_PATTERN_GUIDE.md` - General specification pattern guide (applies to all modules)

---

## Contributing to Documentation

When adding new documentation:
1. Place module-specific docs in the appropriate module's `docs/` folder
2. Place cross-cutting architectural docs in `/api/docs/`
3. Place UI-specific docs in the relevant Blazor page folder under `Docs/`
4. Update this README when adding significant new documentation
5. Follow the naming conventions and content guidelines above
6. Ensure all code examples are current and tested
7. Include practical examples from the actual codebase

---

## Additional Resources

- **Catalog Module:** `/api/modules/Catalog/` - Reference implementation patterns
- **Todo Module:** `/api/modules/Todo/` - Simple CRUD example patterns
- **Coding Instructions:** `/.github/copilot-instructions.md` - Project coding standards

