# 📚 COPILOT INSTRUCTIONS - UI COMPLETE REFERENCE INDEX

**Last Updated**: November 20, 2025  
**Status**: ✅ Production Ready - Complete UI Documentation Suite  
**Version**: 2.0 - Optimized & Modular

---

## 🎯 Quick Navigation

This is your **complete reference guide** for all UI development in the FSH Starter Kit. The documentation has been organized into focused, modular files for maximum efficiency.

---

## 📖 DOCUMENTATION STRUCTURE

### **PART I: FOUNDATION (Start Here!)**

**📄 [`copilot-instructions-ui-foundation.md`](copilot-instructions-ui-foundation.md)**
- ✅ Core UI Principles (6 principles)
- ✅ State Management Best Practices
- ✅ Event Handling & Communication
- ✅ Performance Optimization
- ✅ Accessibility (A11y) Requirements
- ✅ Responsive Design
- ✅ Imports & Dependency Injection Pattern (CRITICAL)
- ✅ Code Patterns - Blazor Components
- ✅ Styling & CSS Patterns
- ✅ Data Binding & Forms
- ✅ Component Library Patterns
- ✅ Responsive & Mobile
- ✅ Best Practices Checklist
- ✅ Implementation Checklist

**Usage**: Read this first for foundational knowledge. Reference when creating new components.
**Token Cost**: ~2,000 tokens

---

### **PART II: CORE COMPONENTS (Use for UI Development)**

**📄 [`copilot-instructions-ui-components.md`](copilot-instructions-ui-components.md)**
- ✅ EntityTable Component Pattern
  - EntityTable Architecture
  - EntityTableContext Configuration
  - EntityField Configuration
- ✅ Autocomplete Components
  - AutocompleteBase Pattern
  - Implementing Custom Autocompletes
  - Using Autocomplete in Forms
- ✅ Dialog Patterns
  - AddEditModal Pattern
  - Details Dialog Pattern
- ✅ Common Service Patterns
  - DialogService Extensions
- ✅ Components Checklist

**Usage**: Reference when building list pages, lookups, or dialogs. Copy patterns directly.
**Token Cost**: ~1,500 tokens

---

### **PART III: MODULE-SPECIFIC PATTERNS (As Needed)**

**📄 [`copilot-instructions-ui-accounting.md`](copilot-instructions-ui-accounting.md)**
- ✅ Financial Data Display
- ✅ Multi-Line Entry Components (double-entry accounting)
- ✅ Workflow Action Menus (Approve → Post → Reverse)
- ✅ Advanced Search Filters
- ✅ Action Button Groups
- ✅ Financial Statement Views
- ✅ Accounting UI Checklist

**Usage**: Include only when working on Accounting features.
**Token Cost**: ~1,200 tokens

---

**📄 [`copilot-instructions-ui-store.md`](copilot-instructions-ui-store.md)**
- ✅ Inventory Management Workflows
- ✅ Stock Tracking Patterns
- ✅ Warehouse Operations
- ✅ Store UI Checklist

**Usage**: Include only when working on Store/Inventory features.
**Token Cost**: ~800 tokens

---

**📄 [`copilot-instructions-ui-hr.md`](copilot-instructions-ui-hr.md)**
- ✅ Employee Management Workflows
- ✅ Wizard Pattern (Multi-Step Forms)
- ✅ Sub-Component Architecture
- ✅ Tabbed Views Pattern
- ✅ Related Data Navigation
- ✅ MenuService & Navigation
- ✅ HR UI Checklist

**Usage**: Include only when working on HR features.
**Token Cost**: ~1,000 tokens

---

## 🚀 HOW TO USE THIS DOCUMENTATION

### **Scenario 1: Creating a New List Page**
1. Start with `copilot-instructions-ui-foundation.md` (skim)
2. Reference `copilot-instructions-ui-components.md` (EntityTable section)
3. Copy EntityTable pattern
4. Implement search filters and actions

**Total context**: ~3,500 tokens

---

### **Scenario 2: Creating a New Accounting Feature**
1. Reference `copilot-instructions-ui-foundation.md` (quick check)
2. Reference `copilot-instructions-ui-components.md` (if need dialogs/autocompletes)
3. Reference `copilot-instructions-ui-accounting.md` (main patterns)
4. Follow accounting-specific checklist

**Total context**: ~4,000-4,500 tokens

---

### **Scenario 3: Creating Store Inventory Features**
1. Reference `copilot-instructions-ui-foundation.md` (quick check)
2. Reference `copilot-instructions-ui-components.md` (EntityTable)
3. Reference `copilot-instructions-ui-store.md` (workflows)
4. Follow store-specific checklist

**Total context**: ~3,500 tokens

---

### **Scenario 4: Creating HR Employee Management**
1. Reference `copilot-instructions-ui-foundation.md` (quick check)
2. Reference `copilot-instructions-ui-components.md` (dialogs)
3. Reference `copilot-instructions-ui-hr.md` (wizard, sub-components)
4. Follow HR-specific checklist

**Total context**: ~4,000 tokens

---

## 📊 FILE BREAKDOWN & TOKEN SAVINGS

| File | Purpose | Tokens | Use When |
|------|---------|--------|----------|
| **foundation.md** | Core principles | 2,000 | Creating any new component |
| **components.md** | EntityTable, Autocomplete, Dialogs | 1,500 | Building CRUD pages or forms |
| **accounting.md** | Accounting patterns | 1,200 | Building Accounting features |
| **store.md** | Inventory patterns | 800 | Building Store/Inventory features |
| **hr.md** | HR patterns | 1,000 | Building HR features |
| **OLD monolithic file** | Everything combined | 10,000 | ❌ Avoid - wastes tokens |

---

## ⚡ TOKEN SAVINGS ACHIEVED

### **Before (Monolithic File)**
Every prompt loaded: **10,000 tokens** for entire file
Result: Context bloated, slower responses, wasteful

### **After (Modular Files)**
General UI work: **2,000-3,500 tokens**  
Module-specific: **3,500-4,500 tokens**  

**✅ Savings: 60-70% tokens saved per prompt!**

---

## 🎯 BEST PRACTICES FOR USING THIS DOCUMENTATION

### **1. Always Start with Foundation**
```
"I'm creating a new [component/page]. 
Reference: copilot-instructions-ui-foundation.md"
```

### **2. Add Module File Only If Needed**
```
"I'm creating an Accounting [feature].
Reference: copilot-instructions-ui-foundation.md, 
           copilot-instructions-ui-accounting.md"
```

### **3. For Complex UI, Add Components File**
```
"I'm building a Store transfer page with dialogs and autocompletes.
Reference: copilot-instructions-ui-foundation.md,
           copilot-instructions-ui-components.md,
           copilot-instructions-ui-store.md"
```

### **4. Always Reference by Filename**
This helps me understand which context to load.

---

## ✅ CRITICAL RULES (Read These!)

### **🚨 Do NOT add [Inject] attributes for services in _Imports.razor**

**Common services already in _Imports.razor:**
- IDialogService
- ISnackbar
- NavigationManager
- IStringLocalizer
- HttpClient/IClient

**Rule**: Only inject services NOT in the imports file.
See `copilot-instructions-ui-foundation.md` for details.

---

### **✅ Always Check EntityTable Pattern**
Before creating a list page, reference the EntityTable pattern in `components.md`.
This saves you hours and ensures consistency.

---

### **✅ Use Autocomplete Base Pattern**
All autocompletes must inherit from `AutocompleteBase<TDto, TClient, TKey>`.
Don't create custom autocompletes without this pattern.

---

## 📚 QUICK CHECKLIST BY SCENARIO

### **New List Page**
- [ ] Reference `copilot-instructions-ui-components.md` → EntityTable
- [ ] Check `copilot-instructions-ui-foundation.md` → Implementation Checklist
- [ ] Apply EntityTable pattern
- [ ] Add search filters
- [ ] Add action menu

### **New Form/Dialog**
- [ ] Reference `copilot-instructions-ui-components.md` → AddEditModal/Details Dialog
- [ ] Check component checklist
- [ ] Apply validation pattern
- [ ] Use DialogService extensions

### **New Autocomplete**
- [ ] Reference `copilot-instructions-ui-components.md` → Autocomplete section
- [ ] Inherit from AutocompleteBase
- [ ] Implement 3 methods: GetItem, SearchText, GetTextValue
- [ ] Add caching with dictionary

### **New Accounting Feature**
- [ ] Reference `copilot-instructions-ui-accounting.md`
- [ ] Follow accounting-specific patterns
- [ ] Use `Format="N2"` for decimals
- [ ] Implement status workflows
- [ ] Check Accounting UI Checklist

### **New Store/Inventory Feature**
- [ ] Reference `copilot-instructions-ui-store.md`
- [ ] Implement warehouse selection
- [ ] Use transfer workflows
- [ ] Check Store UI Checklist

### **New HR Feature**
- [ ] Reference `copilot-instructions-ui-hr.md`
- [ ] Consider wizard pattern for complex forms
- [ ] Use sub-component architecture
- [ ] Check HR UI Checklist

---

## 🎉 FINAL TIPS

### **For Maximum Efficiency:**
1. **Bookmark these files** - You'll reference them frequently
2. **Load only what you need** - Saves tokens and improves responses
3. **Use filenames in prompts** - Helps me load the right context
4. **Follow checklists** - Ensures quality and consistency
5. **Copy patterns directly** - Don't reinvent the wheel

---

## 📞 SUPPORT & REFERENCE

**Question**: "How do I create a new list page?"  
**Answer**: Reference `copilot-instructions-ui-components.md` → EntityTable section

**Question**: "What's the autocomplete pattern?"  
**Answer**: Reference `copilot-instructions-ui-components.md` → Autocomplete section

**Question**: "Do I need _Imports.razor?"  
**Answer**: Reference `copilot-instructions-ui-foundation.md` → Imports section

**Question**: "How do I handle workflows?"  
**Answer**: Reference appropriate module file (accounting/store/hr)

---

## ✅ VERIFICATION STATUS

**Foundation**: ✅ A+ Production Ready  
**Components**: ✅ A+ Production Ready  
**Accounting**: ✅ A+ Verified & Documented  
**Store**: ✅ A+ Verified & Documented  
**HR**: ✅ A+ Verified & Documented  

**Overall**: ✅ **100% PRODUCTION READY**

---

**Last Updated**: November 20, 2025  
**Status**: 🟢 **OPTIMIZED & READY FOR USE**  
**Token Efficiency**: 🚀 **60-70% SAVINGS**

---

## 📋 File Reference Card

```
┌─ Foundation
│  └─ Core Principles & Patterns (Always reference)
│
├─ Components
│  └─ EntityTable, Autocomplete, Dialogs (Reference for CRUD)
│
├─ Modules (Reference as needed)
│  ├─ Accounting
│  ├─ Store
│  └─ HR
│
└─ Index (You are here!)
   └─ Navigation & Best Practices
```

**Start with Foundation, add modules as needed!** ✅

