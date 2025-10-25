# CycleCount Pattern Alignment with Budget

## Date: October 16, 2025

## Summary
Updated CycleCount and CycleCountItem entities to follow the same Domain-Driven Design (DDD) patterns used in Budget and BudgetDetail for consistency across the codebase.

## Key Changes Made

### 1. **CycleCountItem Entity** (`Store.Domain/Entities/CycleCountItem.cs`)

#### Before:
```csharp
public sealed class CycleCountItem : AuditableEntity, IAggregateRoot
{
    public ICollection<CycleCountItem> Items { get; private set; }
}
```

#### After:
```csharp
public sealed class CycleCountItem : AuditableEntity
{
    // Removed IAggregateRoot interface
}
```

**Rationale**: 
- `CycleCountItem` is NOT an aggregate root - it's owned by the `CycleCount` aggregate
- This matches the pattern where `BudgetDetail` does NOT implement `IAggregateRoot`
- Fixes the optimistic concurrency exception that was occurring
- Properly establishes the aggregate boundary

---

### 2. **CycleCount Entity** (`Store.Domain/Entities/CycleCount.cs`)

#### Before:
```csharp
public ICollection<CycleCountItem> Items { get; private set; } = new List<CycleCountItem>();
```

#### After:
```csharp
private readonly List<CycleCountItem> _items = new();
/// <summary>
/// Collection of cycle count items, each representing a counted item with system vs physical quantities.
/// Read-only to enforce proper aggregate management.
/// </summary>
public IReadOnlyCollection<CycleCountItem> Items => _items.AsReadOnly();
```

**Rationale**:
- Uses private backing field `_items` with read-only public property
- Matches the exact pattern used in `Budget` with `_budgetDetails`
- Provides proper encapsulation and prevents external modification
- Enforces aggregate boundary rules

---

### 3. **Added UpdateCounts() Method for External Management**

#### New Method:
```csharp
/// <summary>
/// Update the aggregate counts after items have changed. 
/// Intended to be called by application handlers managing CycleCountItem entities.
/// Similar to Budget.SetTotals() for consistency.
/// </summary>
public CycleCount UpdateCounts(int totalItems, int itemsCountedCorrect, int itemsWithDiscrepancies)
{
    TotalItemsToCount = totalItems;
    ItemsCountedCorrect = itemsCountedCorrect;
    ItemsWithDiscrepancies = itemsWithDiscrepancies;
    AccuracyPercentage = totalItems > 0 ? (decimal)itemsCountedCorrect / totalItems * 100 : 100;
    return this;
}
```

**Rationale**:
- Mirrors `Budget.SetTotals()` method pattern
- Allows application handlers to update aggregate totals
- Decouples detail management from aggregate calculation
- Supports external orchestration of child entity changes

---

### 4. **CycleCountConfiguration** (`Store.Infrastructure/Persistence/Configurations/CycleCountConfiguration.cs`)

#### Added:
```csharp
// Configure Items collection with private backing field for proper encapsulation
builder.HasMany(x => x.Items)
    .WithOne(x => x.CycleCount)
    .HasForeignKey(x => x.CycleCountId)
    .OnDelete(DeleteBehavior.Cascade);

// Map the private backing field for the Items collection
builder.Metadata
    .FindNavigation(nameof(CycleCount.Items))!
    .SetPropertyAccessMode(PropertyAccessMode.Field);
```

**Rationale**:
- Explicitly configures EF Core to use the private `_items` backing field
- Matches the `BudgetConfiguration` pattern
- Ensures proper change tracking with private collections
- Prevents accidental external modifications

---

### 5. **CycleCountItemConfiguration** (`Store.Infrastructure/Persistence/Configurations/CycleCountItemConfiguration.cs`)

#### Removed:
```csharp
builder.HasOne(x => x.CycleCount)
    .WithMany(x => x.Items)
    .HasForeignKey(x => x.CycleCountId)
    .OnDelete(DeleteBehavior.Cascade);
```

**Rationale**:
- Relationship is now configured from the parent side (CycleCount) only
- Follows the same pattern as Budget/BudgetDetail
- Avoids duplicate relationship configuration
- Single source of truth for the relationship

---

## Pattern Consistency Achieved

### Budget Pattern (Reference)
```
Budget (Aggregate Root)
  └── private readonly List<BudgetDetail> _budgetDetails
  └── public IReadOnlyCollection<BudgetDetail> BudgetDetails
  └── Budget.SetTotals(totalBudgeted, totalActual)

BudgetDetail (Owned Entity)
  └── class BudgetDetail : AuditableEntity  // NO IAggregateRoot
```

### CycleCount Pattern (Now Aligned)
```
CycleCount (Aggregate Root)
  └── private readonly List<CycleCountItem> _items
  └── public IReadOnlyCollection<CycleCountItem> Items
  └── CycleCount.UpdateCounts(totalItems, itemsCountedCorrect, itemsWithDiscrepancies)

CycleCountItem (Owned Entity)
  └── class CycleCountItem : AuditableEntity  // NO IAggregateRoot
```

---

## Benefits of This Alignment

1. **Consistency**: Both Budget/BudgetDetail and CycleCount/CycleCountItem follow the same DDD patterns
2. **Encapsulation**: Private backing fields prevent external modification of collections
3. **Aggregate Boundaries**: Clear ownership hierarchy with proper root designation
4. **Concurrency Safety**: Fixes optimistic concurrency issues by proper entity tracking
5. **Maintainability**: Developers familiar with Budget pattern can easily work with CycleCount
6. **Testability**: Clear boundaries make unit testing more straightforward

---

## Migration Notes

- No database schema changes required - only code-level refactoring
- Existing data remains unaffected
- API contracts remain unchanged
- Application handlers continue to work (AddCycleCountItemHandler already updated)

---

## Verification

All builds completed successfully:
- ✅ Store.Domain.csproj
- ✅ Store.Infrastructure.csproj  
- ✅ Store.Application.csproj

No breaking changes introduced.

