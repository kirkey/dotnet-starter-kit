# USOA Category Validation Fix

## Problem
The application was failing to start with the error:
```
Invalid USOA category: Plant
```

## Root Cause
The `ChartOfAccount` entity has a validation method `IsValidUsoaCategory()` that only accepts specific USOA (Uniform System of Accounts) categories. The seed data was using invalid categories that were not in the allowed list.

## Valid USOA Categories
The following categories are valid according to the `ChartOfAccount` entity:
- `Production`
- `Transmission`
- `Distribution`
- `Customer Accounts`
- `Customer Service`
- `Sales`
- `Administrative`
- `General`
- `Maintenance`
- `Operation`
- `Operations`
- `COGS`
- `Inventory`

## Invalid Categories Found and Fixed

### 1. "Plant" (11 occurrences) ❌
**Invalid Category:** `Plant`  
**Used For:** Electric plant assets and accumulated depreciation

**Fix Applied:**
- `1500` - Electric Plant in Service → Changed to `Operations`
- `1510` - Generation Plant → Changed to `Production`
- `1520` - Transmission Plant → Changed to `Transmission`
- `1530` - Distribution Plant → Changed to `Distribution`
- `1540` - General Plant → Changed to `General`
- `1600` - Accumulated Depreciation → Changed to `Operations`
- `1610` - Accumulated Depreciation - Generation → Changed to `Production`
- `1620` - Accumulated Depreciation - Transmission → Changed to `Transmission`
- `1630` - Accumulated Depreciation - Distribution → Changed to `Distribution`
- `1640` - Accumulated Depreciation - General → Changed to `General`
- `1700` - Construction Work in Progress → Changed to `Operations`

### 2. "Depreciation" (5 occurrences) ❌
**Invalid Category:** `Depreciation`  
**Used For:** Depreciation expense accounts

**Fix Applied:**
- `7000` - Depreciation & Amortization → Changed to `Administrative`
- `7100` - Depreciation - Generation → Changed to `Production`
- `7200` - Depreciation - Transmission → Changed to `Transmission`
- `7300` - Depreciation - Distribution → Changed to `Distribution`
- `7400` - Depreciation - General → Changed to `General`

### 3. "Taxes" (3 occurrences) ❌
**Invalid Category:** `Taxes`  
**Used For:** Tax expense accounts

**Fix Applied:**
- `8000` - Taxes Other Than Income → Changed to `Administrative`
- `8100` - Property Taxes → Changed to `Administrative`
- `8200` - Payroll Taxes → Changed to `Administrative`

### 4. "Financial" (3 occurrences) ❌
**Invalid Category:** `Financial`  
**Used For:** Interest expense accounts

**Fix Applied:**
- `9000` - Interest Expense → Changed to `Administrative`
- `9100` - Interest on Bonds → Changed to `Administrative`
- `9200` - Interest on Notes → Changed to `Administrative`

## Total Changes
- **22 accounts** updated with valid USOA categories
- **4 invalid categories** removed
- **All accounts** now use valid USOA categories

## Category Mapping Logic

### Plant Assets → Functional Categories
Instead of generic "Plant", accounts are now categorized by their functional area:
- **Production:** Generation plant and related depreciation
- **Transmission:** Transmission plant and related depreciation
- **Distribution:** Distribution plant and related depreciation
- **General:** General/administrative plant and related depreciation
- **Operations:** Overall plant summaries and construction WIP

### Expense Categories → Administrative or Functional
- **Depreciation expenses:** Mapped to their functional areas (Production, Transmission, Distribution) or Administrative for general
- **Tax expenses:** All mapped to Administrative
- **Interest expenses:** All mapped to Administrative

## Benefits of This Fix

### ✅ Compliance
- All accounts now use FERC-compliant USOA categories
- Better alignment with utility accounting standards
- Proper functional area segregation

### ✅ Reporting
- More accurate functional area reporting
- Better cost allocation by utility function
- Improved regulatory reporting capabilities

### ✅ Clarity
- Clearer categorization of accounts
- Easier to understand account purposes
- Better alignment with utility operations

## Verification

### Before Fix
```csharp
ChartOfAccount.Create("1510", "Generation Plant", "Asset", "Plant", ...)  // ❌ Invalid
ChartOfAccount.Create("7100", "Depreciation - Generation", "Expense", "Depreciation", ...) // ❌ Invalid
ChartOfAccount.Create("8100", "Property Taxes", "Expense", "Taxes", ...) // ❌ Invalid
ChartOfAccount.Create("9000", "Interest Expense", "Expense", "Financial", ...) // ❌ Invalid
```

### After Fix
```csharp
ChartOfAccount.Create("1510", "Generation Plant", "Asset", "Production", ...) // ✅ Valid
ChartOfAccount.Create("7100", "Depreciation - Generation", "Expense", "Production", ...) // ✅ Valid
ChartOfAccount.Create("8100", "Property Taxes", "Expense", "Administrative", ...) // ✅ Valid
ChartOfAccount.Create("9000", "Interest Expense", "Expense", "Administrative", ...) // ✅ Valid
```

## Files Modified
- `/api/modules/Accounting/Accounting.Infrastructure/Persistence/AccountingDbInitializer.cs`

## Status
✅ **COMPLETE** - All USOA category validation errors resolved

## Date
October 31, 2025

