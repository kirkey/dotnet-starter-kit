# Validator Fix - CustomValidator → AbstractValidator

## Issue
```
Error CS0246: The type or namespace name 'CustomValidator<>' could not be found
```

Occurred in:
- `CreateJournalEntryLineValidator.cs`
- `UpdateJournalEntryLineValidator.cs`

## Root Cause

The validators were using `CustomValidator<>` which doesn't exist in the codebase. The correct base class from FluentValidation is `AbstractValidator<>`.

## Pattern Analysis

### Todo Module Pattern
```csharp
using FluentValidation;

public class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoValidator(TodoDbContext context)
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}
```

### Catalog Module Pattern
```csharp
using FluentValidation;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MinimumLength(2).MaximumLength(75);
        RuleFor(p => p.Price).GreaterThan(0);
    }
}
```

### Accounting Module Pattern (Existing Validators)
```csharp
// No explicit using needed - GlobalUsings.cs has:
// global using FluentValidation;

public sealed class CreateBudgetDetailValidator : AbstractValidator<CreateBudgetDetailCommand>
{
    public CreateBudgetDetailValidator()
    {
        RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.BudgetedAmount).GreaterThanOrEqualTo(0m);
    }
}
```

## Solution Applied

### File: `CreateJournalEntryLineValidator.cs`
```diff
- public sealed class CreateJournalEntryLineValidator : CustomValidator<CreateJournalEntryLineCommand>
+ public sealed class CreateJournalEntryLineValidator : AbstractValidator<CreateJournalEntryLineCommand>
```

### File: `UpdateJournalEntryLineValidator.cs`
```diff
- public sealed class UpdateJournalEntryLineValidator : CustomValidator<UpdateJournalEntryLineCommand>
+ public sealed class UpdateJournalEntryLineValidator : AbstractValidator<UpdateJournalEntryLineCommand>
```

## Why No Using Statement Needed

The Accounting.Application project has a `GlobalUsings.cs` file that includes:

```csharp
global using FluentValidation;
```

This makes FluentValidation available to all files in the project without explicit using statements.

## Verification

✅ Both validators now compile without errors
✅ Pattern matches all other validators in the codebase
✅ Consistent with Todo, Catalog, and other Accounting module validators

## Files Modified
1. `/api/modules/Accounting/Accounting.Application/JournalEntries/Lines/Create/CreateJournalEntryLineValidator.cs`
2. `/api/modules/Accounting/Accounting.Application/JournalEntries/Lines/Update/UpdateJournalEntryLineValidator.cs`

## Status
✅ **FIXED** - All compilation errors resolved

---

**Date**: November 2, 2025  
**Error Code**: CS0246  
**Resolution**: Changed `CustomValidator<>` to `AbstractValidator<>`  
**Build Status**: ✅ Success

