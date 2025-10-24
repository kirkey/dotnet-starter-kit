# Documentation Index

This directory contains general documentation and links to module-specific documentation.

---

## 📚 General Documentation

### Architecture & Patterns

1. **[CODE_PATTERNS_GUIDE.md](./CODE_PATTERNS_GUIDE.md)**
   - Coding patterns and best practices
   - CQRS implementation guidelines
   - DRY principles
   - Validation patterns

2. **[VISUAL_OVERVIEW.md](./VISUAL_OVERVIEW.md)**
   - System architecture overview
   - Visual diagrams and flow charts

### Implementation & Checklists

3. **[EXECUTIVE_SUMMARY.md](./EXECUTIVE_SUMMARY.md)**
   - Project executive summary
   - High-level overview
   - Status and milestones

4. **[IMPLEMENTATION_CHECKLIST.md](./IMPLEMENTATION_CHECKLIST.md)**
   - Implementation task checklist
   - Progress tracking
   - Completion status

---

## 🏢 Module-Specific Documentation

### Store Module
📁 **[src/api/modules/Store/docs/](../src/api/modules/Store/docs/README.md)**

**Key Topics:**
- Goods Receipt & Inventory Management (Partial Receiving Support)
- Warehouse Management
- Cycle Count
- Pick Lists & Put-Away Tasks
- Stock Levels & Inventory Transactions
- Lot & Serial Number Tracking
- Import/Export Functionality
- Blazor UI Implementation

**Total Documents**: 40+ comprehensive guides

**Quick Links:**
- [Goods Receipt Implementation](../src/api/modules/Store/docs/GOODS_RECEIPT_IMPLEMENTATION.md)
- [Partial Receiving Guide](../src/api/modules/Store/docs/PARTIAL_RECEIVING_IMPLEMENTATION.md)
- [Store Quick Reference](../src/api/modules/Store/docs/STORE_QUICK_REFERENCE.md)
- [Blazor Implementation](../src/api/modules/Store/docs/STORE_BLAZOR_IMPLEMENTATION_COMPLETE.md)

---

### Accounting Module
📁 **[src/api/modules/Accounting/docs/](../src/api/modules/Accounting/docs/README.md)**

**Key Topics:**
- Check Management System
- Bank Management
- Debit/Credit Memos
- Chart of Accounts
- Journal Entries
- Accounting Periods
- Configuration Management
- Blazor UI Implementation

**Total Documents**: 28+ comprehensive guides

**Quick Links:**
- [Check Implementation](../src/api/modules/Accounting/docs/CHECK_IMPLEMENTATION_FINAL_SUMMARY.md)
- [Check Quick Start](../src/api/modules/Accounting/docs/CHECK_QUICK_START_FINAL.md)
- [Bank Management](../src/api/modules/Accounting/docs/BANK_MANAGEMENT_IMPLEMENTATION.md)
- [Debit/Credit Memos](../src/api/modules/Accounting/docs/DEBIT_CREDIT_MEMOS_IMPLEMENTATION_COMPLETE.md)

---

## 🗂️ Documentation Structure

```
dotnet-starter-kit/
├── docs/                                    ← General documentation
│   ├── README.md                           ← This file
│   ├── CODE_PATTERNS_GUIDE.md
│   ├── EXECUTIVE_SUMMARY.md
│   ├── IMPLEMENTATION_CHECKLIST.md
│   └── VISUAL_OVERVIEW.md
│
└── src/api/modules/
    ├── Store/
    │   └── docs/                           ← Store module docs (40+ files)
    │       ├── README.md                   ← Store documentation index
    │       ├── GOODS_RECEIPT_*.md
    │       ├── PARTIAL_RECEIVING_*.md
    │       ├── WAREHOUSE_*.md
    │       ├── CYCLE_COUNT_*.md
    │       └── Store_*_Implementation_Complete.md
    │
    ├── Accounting/
    │   └── docs/                           ← Accounting module docs (28+ files)
    │       ├── README.md                   ← Accounting documentation index
    │       ├── CHECK_*.md
    │       ├── BANK_MANAGEMENT_*.md
    │       ├── DEBIT_CREDIT_MEMOS_*.md
    │       └── ACCOUNTING_*.md
    │
    └── Catalog/
        └── (future module documentation)
```

---

## 🎯 Quick Navigation

### By Feature Area

#### Inventory & Warehouse Management
- 📍 [Store Module Docs](../src/api/modules/Store/docs/README.md)
- 📍 [Goods Receipt System](../src/api/modules/Store/docs/GOODS_RECEIPT_IMPLEMENTATION.md)
- 📍 [Partial Receiving](../src/api/modules/Store/docs/PARTIAL_RECEIVING_IMPLEMENTATION.md)
- 📍 [Warehouse Management](../src/api/modules/Store/docs/WAREHOUSE_REFACTORING_COMPLETE.md)

#### Financial Management
- 📍 [Accounting Module Docs](../src/api/modules/Accounting/docs/README.md)
- 📍 [Check Management](../src/api/modules/Accounting/docs/CHECK_IMPLEMENTATION_FINAL_SUMMARY.md)
- 📍 [Bank Management](../src/api/modules/Accounting/docs/BANK_MANAGEMENT_IMPLEMENTATION.md)
- 📍 [Debit/Credit Memos](../src/api/modules/Accounting/docs/DEBIT_CREDIT_MEMOS_IMPLEMENTATION_COMPLETE.md)

#### Development Guidelines
- 📍 [Code Patterns Guide](./CODE_PATTERNS_GUIDE.md)
- 📍 [Implementation Checklist](./IMPLEMENTATION_CHECKLIST.md)

---

## 🔧 For Developers

### Getting Started

1. **Review Architecture**
   - Read [VISUAL_OVERVIEW.md](./VISUAL_OVERVIEW.md)
   - Understand [CODE_PATTERNS_GUIDE.md](./CODE_PATTERNS_GUIDE.md)

2. **Choose Your Module**
   - Store: [Store Module Docs](../src/api/modules/Store/docs/README.md)
   - Accounting: [Accounting Module Docs](../src/api/modules/Accounting/docs/README.md)

3. **Follow Implementation Guides**
   - Each module has detailed implementation guides
   - Quick reference guides available
   - Code examples provided

### Adding New Documentation

When creating new documentation:

1. **Determine Scope**
   - General → Place in `/docs/`
   - Module-specific → Place in `/src/api/modules/{Module}/docs/`

2. **Follow Naming Conventions**
   - Use UPPERCASE for document names
   - Use underscores for spaces: `FEATURE_IMPLEMENTATION.md`
   - Include status: `_COMPLETE`, `_SUMMARY`, etc.

3. **Update Indexes**
   - Add entry to module's `README.md`
   - Update this index if general documentation

4. **Include Standard Sections**
   - Overview/Summary
   - Implementation details
   - Code examples
   - Testing guide
   - Quick reference (if applicable)

---

## 📊 Documentation Statistics

### By Module

| Module | Documents | Status |
|--------|-----------|--------|
| Store | 40+ | ✅ Complete |
| Accounting | 28+ | ✅ Complete |
| General | 4 | ✅ Complete |
| **Total** | **72+** | **✅ Organized** |

### By Category

| Category | Count |
|----------|-------|
| Implementation Guides | 35+ |
| Quick References | 8+ |
| Blazor UI Docs | 12+ |
| Architecture Docs | 10+ |
| Refactoring Summaries | 7+ |

---

## 🎉 Recent Updates

### October 24, 2025
- ✅ Organized all documentation into module-specific folders
- ✅ Created comprehensive README indexes for each module
- ✅ Updated Store module with 40+ documentation files
- ✅ Updated Accounting module with 28+ documentation files
- ✅ Created master documentation index (this file)

---

## 🤝 Contributing

### Documentation Standards

1. **Format**: Use Markdown (.md)
2. **Structure**: Clear headings and sections
3. **Examples**: Include code snippets
4. **Diagrams**: Use ASCII art or mermaid diagrams
5. **Status**: Mark completion status clearly

### Review Process

1. Create documentation
2. Place in appropriate location
3. Update relevant README.md
4. Submit for review
5. Merge after approval

---

## 📞 Support

For questions about documentation:

1. Check the relevant module's README first
2. Review specific feature documentation
3. Consult code examples
4. Check implementation guides

For technical questions:
- Review module-specific docs
- Check API documentation in Swagger
- Refer to code patterns guide

---

## 🔗 External Resources

- [Project Repository](https://github.com/your-repo)
- [API Documentation](http://localhost:5001/swagger) (when running locally)
- [Wiki](https://github.com/your-repo/wiki) (if applicable)

---

**Last Updated**: October 24, 2025  
**Maintainer**: Development Team  
**Status**: ✅ Complete and Organized

