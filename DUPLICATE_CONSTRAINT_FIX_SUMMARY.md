# Database Unique Constraint Violation Fix

## Problem
The application was failing to start with multiple `duplicate key value violates unique constraint` errors during database seeding. The errors occurred on:
1. `IX_PurchaseOrderItems_PurchaseOrder_Item` 
2. `IX_InventoryTransferItems_Transfer_Item`

These unique constraints enforce that each parent entity (PurchaseOrder, InventoryTransfer, CycleCount) can only have one line item per child entity (Item).

## Root Cause
The seeding logic in `StoreDbInitializer.cs` was using simple modulo operations to cycle through parent and child entities:
```csharp
for (var i = existingCount + 1; i <= 10; i++)
{
    var parent = parents[i % parents.Count];
    var child = children[i % children.Count];
    // Create item with (parent.Id, child.Id) combination
}
```

This approach created duplicate combinations of (ParentId, ChildId) when:
- The seeding ran multiple times
- Multiple iterations cycled back to the same parent-child combination

## Solution
Updated the seeding logic for entities with composite unique constraints to:

1. **Track existing pairs**: Query the database to get all existing (ParentId, ChildId) combinations
2. **Use nested loops**: Systematically iterate through parents and children instead of using modulo cycling
3. **Leverage HashSet.Add()**: Use the boolean return value of `HashSet.Add()` to check if a pair is new (returns true) or already exists (returns false)
4. **Only create needed items**: Calculate how many new items are needed and only create that many

### Pattern Applied
```csharp
var uniquePairs = new HashSet<(DefaultIdType, DefaultIdType)>();

// Load existing pairs
var existingPairs = await context.Items
    .Select(x => new { x.ParentId, x.ChildId })
    .ToListAsync(cancellationToken);

foreach (var pair in existingPairs)
{
    uniquePairs.Add((pair.ParentId, pair.ChildId));
}

// Create only new combinations
int itemsNeeded = 10 - existingCount;
int itemCount = 0;
for (var parentIdx = 0; parentIdx < parents.Count && itemCount < itemsNeeded; parentIdx++)
{
    for (var childIdx = 0; childIdx < children.Count && itemCount < itemsNeeded; childIdx++)
    {
        var parent = parents[parentIdx];
        var child = children[childIdx];
        var pair = (parent.Id, child.Id);
        
        if (uniquePairs.Add(pair)) // Returns true only if pair was added (new pair)
        {
            items.Add(Item.Create(...));
            itemCount++;
        }
    }
}
```

## Files Modified
- `/Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/Store/Store.Infrastructure/Persistence/StoreDbInitializer.cs`

## Sections Updated
1. **Section 14: PurchaseOrderItems** - Entities with unique constraint on (PurchaseOrderId, ItemId)
2. **Section 19: CycleCountItems** - Entities with unique constraint on (CycleCountId, ItemId)
3. **Section 21: InventoryTransferItems** - Entities with unique constraint on (InventoryTransferId, ItemId)

## Benefits
- ✅ Eliminates duplicate key constraint violations
- ✅ Makes seeding idempotent (can run multiple times safely)
- ✅ Ensures all unique constraints are respected
- ✅ Uses efficient HashSet operations (O(1) lookup and add)
- ✅ Cleaner nested loop approach vs modulo cycling
- ✅ Zero warnings in code analysis (uses HashSet.Add() return value properly)

## Testing
The application now starts successfully without database seeding errors. The seeding logic can be run multiple times without causing constraint violations.

## Related Unique Constraints
Other entities with single-field unique constraints (these don't have the same issue):
- PurchaseOrder.OrderNumber
- InventoryTransfer.TransferNumber
- GoodsReceipt.ReceiptNumber
- PickList.PickListNumber
- StockAdjustment.AdjustmentNumber
- PutAwayTask.TaskNumber
- And others for codes and numbers

