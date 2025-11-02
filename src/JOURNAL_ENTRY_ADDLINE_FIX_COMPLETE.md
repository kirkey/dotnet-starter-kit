# Journal Entry AddLine/Lines Refactoring - COMPLETE ✅

## Issue Summary
After refactoring JournalEntry to follow the Budget/BudgetDetail pattern (removing `AddLine()` method and `Lines` property), several handlers were still using the old API, causing compilation errors.

## Errors Fixed

### 1. CreateInvoiceFromConsumptionHandler.cs ✅
**Errors:**
- Line 84: `je.AddLine(arAccount.Id, invoice.TotalAmount, 0m, ...)`
- Line 86: `je.AddLine(revenueAccount.Id, 0m, invoice.TotalAmount, ...)`

**Fix Applied:**
- Added `IRepository<JournalEntryLine>` dependency injection
- Created journal entry lines separately using `JournalEntryLine.Create()`
- Lines created after journal entry is saved (requires je.Id)

### 2. RetirePatronageHandler.cs ✅
**Errors:**
- Line 33: `je.AddLine(equityAccount.Id, request.Amount, 0m, ...)`
- Line 35: `je.AddLine(cashAccount.Id, 0m, request.Amount, ...)`

**Fix Applied:**
- Added `IRepository<JournalEntryLine>` dependency injection
- Replaced AddLine calls with `JournalEntryLine.Create()`
- Proper save sequencing

### 3. PostJournalEntryCommandHandler.cs ✅
**Errors:**
- Line 35: `journalEntry.Lines.Sum(l => l.DebitAmount)`
- Line 36: `journalEntry.Lines.Sum(l => l.CreditAmount)`
- Line 44: `foreach (var line in journalEntry.Lines)`

**Fix Applied:**
- Added `IReadRepository<JournalEntryLine>` dependency injection
- Query lines using `JournalEntryLinesByJournalEntryIdSpec`
- Changed `line.Description` to `line.Memo` (property renamed)

### 4. ReverseJournalEntryHandler.cs ✅
**Errors:**
- Line 38: `foreach (var line in originalEntry.Lines)`
- Line 40: `reversingEntry.AddLine(...)`

**Fix Applied:**
- Added read and write repositories for JournalEntryLine
- Query original lines using specification
- Create reversed lines with `JournalEntryLine.Create()`
- Proper save sequencing: journal entry first, then lines

### 5. AccountingDbInitializer.cs ✅
**Errors:**
- Line 511: `je = je.AddLine(cashAccount.Id, 25000m, 0m, ...)`
- Line 512: `je = je.AddLine(revenueAccount.Id, 0m, 25000m, ...)`

**Fix Applied:**
- Save journal entry first
- Create lines using `JournalEntryLine.Create()`
- Add lines to context.JournalEntryLines
- Post journal entry after lines are created

---

## Pattern Applied

### Old Pattern (Removed)
```csharp
var je = JournalEntry.Create(...);
je.AddLine(accountId, debit, credit, description);
je.AddLine(accountId, debit, credit, description);
je.Post();
await repository.AddAsync(je);
```

### New Pattern (Budget/BudgetDetail Style)
```csharp
// 1. Create and save journal entry
var je = JournalEntry.Create(...);
await journalRepo.AddAsync(je);
await journalRepo.SaveChangesAsync();

// 2. Create lines separately (requires je.Id)
var line1 = JournalEntryLine.Create(je.Id, accountId, debit, credit, memo, reference);
var line2 = JournalEntryLine.Create(je.Id, accountId, debit, credit, memo, reference);
await lineRepo.AddAsync(line1);
await lineRepo.AddAsync(line2);
await lineRepo.SaveChangesAsync();

// 3. Post after lines exist
je.Post();
await journalRepo.UpdateAsync(je);
await journalRepo.SaveChangesAsync();
```

### Querying Lines
```csharp
// Old: journalEntry.Lines
var spec = new JournalEntryLinesByJournalEntryIdSpec(journalEntryId);
var lines = await lineRepo.ListAsync(spec, cancellationToken);
```

---

## Property Changes

| Old Property | New Property | Notes |
|--------------|--------------|-------|
| `line.Description` | `line.Memo` | Property renamed for consistency |
| `journalEntry.Lines` | Query via spec | No longer a navigation property |

---

## Files Modified

### Application Layer (4 files)
1. `/Billing/Handlers/CreateInvoiceFromConsumptionHandler.cs`
   - Added `IRepository<JournalEntryLine>` dependency
   - Replaced AddLine with Create pattern

2. `/Patronages/Handlers/RetirePatronageHandler.cs`
   - Added `IRepository<JournalEntryLine>` dependency
   - Replaced AddLine with Create pattern

3. `/GeneralLedgers/Commands/PostJournalEntry/v1/PostJournalEntryCommandHandler.cs`
   - Added `IReadRepository<JournalEntryLine>` dependency
   - Query lines using specification
   - Changed Description to Memo

4. `/JournalEntries/Reverse/ReverseJournalEntryHandler.cs`
   - Added read and write JournalEntryLine repositories
   - Query original lines using specification
   - Create reversed lines with proper save sequence

### Infrastructure Layer (1 file)
5. `/Persistence/AccountingDbInitializer.cs`
   - Updated seed data creation
   - Proper save sequencing for journal entry and lines

---

## Dependency Injection Pattern

All handlers now follow this pattern:

```csharp
public sealed class SomeHandler(
    // ... other dependencies ...
    [FromKeyedServices("accounting:journal-lines")] IRepository<JournalEntryLine> journalLineRepo
    // OR for read-only:
    [FromKeyedServices("accounting:journal-lines")] IReadRepository<JournalEntryLine> journalLineRepo
) : IRequestHandler<SomeCommand, SomeResponse>
```

---

## Save Sequencing Rules

1. **Create journal entry first** - Need je.Id for lines
2. **Save journal entry** - Persists to database
3. **Create lines** - Reference je.Id
4. **Save lines** - Persists line records
5. **Post journal entry** (optional) - Marks as posted
6. **Update journal entry** - Saves posted status

**Critical:** Lines require the parent journal entry ID, so the journal entry must be saved first.

---

## Testing Checklist

- [x] CreateInvoiceFromConsumptionHandler compiles
- [x] RetirePatronageHandler compiles
- [x] PostJournalEntryCommandHandler compiles
- [x] ReverseJournalEntryHandler compiles
- [x] AccountingDbInitializer compiles
- [x] All handlers follow new pattern
- [x] Proper dependency injection
- [x] Correct save sequencing

---

## Build Status

✅ **All compilation errors resolved**
- No CS1061 errors (AddLine not found)
- No CS1061 errors (Lines not found)
- All handlers properly refactored
- Pattern consistency maintained

---

## Breaking Changes Summary

### For Developers
1. **Cannot use `je.AddLine()` anymore** - Use `JournalEntryLine.Create()` instead
2. **Cannot access `je.Lines` directly** - Query using `JournalEntryLinesByJournalEntryIdSpec`
3. **Must save journal entry before creating lines** - Lines need je.Id
4. **Property renamed** - `Description` → `Memo` on JournalEntryLine

### Migration Steps
1. Add `IRepository<JournalEntryLine>` or `IReadRepository<JournalEntryLine>` dependency
2. Replace `je.AddLine(...)` with:
   ```csharp
   await journalRepo.SaveChangesAsync(); // Save je first
   var line = JournalEntryLine.Create(je.Id, ...);
   await lineRepo.AddAsync(line);
   await lineRepo.SaveChangesAsync();
   ```
3. Replace `je.Lines` with specification query
4. Update `line.Description` to `line.Memo`

---

## Conclusion

✅ **STATUS: COMPLETE**

All handlers have been successfully refactored to use the new Budget/BudgetDetail master-detail pattern. The journal entry functionality now:
- Maintains separation between master (JournalEntry) and detail (JournalEntryLine) entities
- Follows consistent patterns with the rest of the codebase
- Properly manages save sequencing
- Uses dependency injection for line repositories
- Compiles without errors

---

**Date Completed**: November 2, 2025  
**Files Modified**: 5  
**Errors Fixed**: 9 compilation errors  
**Build Status**: ✅ SUCCESS

