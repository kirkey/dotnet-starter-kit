# Store Dashboard Updates - October 26, 2025

## Overview
Enhanced the Store Dashboard to display critical warehouse operations entities with actual data from the Store API endpoints.

## Changes Made

### 1. **New KPI Metrics Added**
Added a third row of KPI cards to track warehouse operations:
- **Goods Receipts Count**: Total number of goods receipts processed
- **Inventory Transfers Pending**: Number of transfers that are pending or in-transit
- **Active Pick Lists**: Number of pick lists currently in progress
- **Cycle Counts In Progress**: Number of ongoing cycle count operations

### 2. **New Dashboard Sections**

#### Recent Goods Receipts Table
- Displays the 10 most recent goods receipts
- Shows: Receipt Number, PO Number, Warehouse, Received Date, Status
- Real-time data from `SearchGoodsReceiptsEndpointAsync`
- Status indicators with color-coded chips

#### Active Inventory Transfers Table
- Shows active transfers (Pending, Approved, and InTransit statuses)
- Displays: Transfer Number, From Warehouse, To Warehouse, Item Count, Status
- Filters active transfers on client side to avoid permission issues
- Color-coded status chips for easy identification
- Graceful handling of 403 unauthorized errors

#### Active Pick Lists Table
- Lists pick lists currently in progress
- Shows: Pick List Number, Warehouse, Assigned To, Priority, Status
- Priority levels color-coded (High=Red, Medium=Orange, Normal=Blue)
- Real-time assignment tracking
- Uses single OrderBy to avoid specification conflicts

#### Put Away Tasks Table
- Displays incomplete put away tasks
- Shows: Task Number, Warehouse, Item Count, Assigned To, Status
- Filters out completed tasks automatically
- Tracks worker assignments
- Uses single OrderBy for better performance

### 3. **Data Loading Methods**

#### LoadGoodsReceiptsAsync
- Fetches recent goods receipts sorted by received date
- Populates both metrics and detailed table data
- Error handling with user-friendly notifications

#### LoadInventoryTransfersAsync
- Queries all transfers and filters active ones on client side
- Handles 403 unauthorized errors gracefully (silently fails)
- Filters for Pending, Approved, and InTransit statuses
- Single query approach to avoid permission issues

#### LoadPickListsAsync
- Filters for InProgress status
- Converts numeric priority values to readable labels
- Uses single OrderBy ("Priority desc") to avoid specification conflicts
- Sorts by priority for relevance

#### LoadPutAwayTasksAsync
- Retrieves all tasks and filters out completed ones
- Uses TotalLines property for item count
- Displays warehouse name from task name
- Uses single OrderBy to avoid specification conflicts

#### LoadCycleCountsAsync
- Retrieves all cycle counts
- Filters for InProgress status in code
- Updates the KPI metric

### 4. **Helper Methods for UI**

#### Color Coding Methods
- `GetTransferStatusColor()`: Maps transfer statuses to appropriate colors
- `GetPriorityColor()`: Maps priority levels to visual indicators
- `GetPickListStatusColor()`: Handles pick list status visualization
- `GetPutAwayStatusColor()`: Manages put away task status colors

### 5. **New Model Classes**

#### GoodsReceiptItem
- ReceiptNumber
- PurchaseOrderNumber
- WarehouseName
- ReceivedDate
- Status

#### InventoryTransferItem
- TransferNumber
- FromWarehouse
- ToWarehouse
- ItemCount
- Status

#### PickListItem
- PickListNumber
- WarehouseName
- AssignedTo
- Priority
- Status

#### PutAwayTaskItem
- TaskNumber
- WarehouseName
- ItemCount
- AssignedTo
- Status

## Technical Details

### API Integration
- All data is loaded from actual Store module API endpoints
- Uses parallel loading with `Task.WhenAll` for performance
- Proper error handling with Snackbar notifications
- Respects nullable reference types

### UI/UX Enhancements
- Refresh buttons on each section for manual updates
- Consistent card styling with gradients for KPIs
- Responsive grid layout using MudBlazor components
- Color-coded status indicators for quick visual scanning
- Empty state messages when no data is available
- Hover and striped table effects for better readability

### Performance Considerations
- Parallel data loading reduces initial load time
- Limited result sets (10 items per table) for performance
- Efficient status filtering on both client and server side
- Lazy loading approach with conditional rendering

## Files Modified

1. `/apps/blazor/client/Pages/Store/Dashboard/StoreDashboard.razor`
   - Added tertiary KPI row
   - Added four new data tables
   - Added refresh functionality

2. `/apps/blazor/client/Pages/Store/Dashboard/StoreDashboard.razor.cs`
   - Added new list properties for warehouse entities
   - Implemented data loading methods
   - Added color helper methods
   - Created new model classes
   - Updated metrics class with new properties

## Benefits

1. **Real-Time Visibility**: Dashboard now shows actual operational data
2. **Better Decision Making**: Key warehouse metrics at a glance
3. **Operational Efficiency**: Quick identification of pending tasks and bottlenecks
4. **Performance Monitoring**: Track goods receipts, transfers, and picking operations
5. **Inventory Accuracy**: Monitor cycle count progress

## Future Enhancements

Potential additions for the dashboard:
- Warehouse capacity utilization charts
- Stock adjustment history
- Serial/Lot number tracking summary
- Supplier performance metrics
- Item supplier relationships overview
- Bin utilization statistics
- Warehouse location efficiency metrics

## Testing Recommendations

1. Verify data loads correctly with actual Store data
2. Test refresh functionality on each section
3. Validate color coding for different statuses
4. Test error handling when API is unavailable
5. Verify responsive behavior on different screen sizes
6. Test with empty data scenarios
7. Performance testing with large datasets

## Known Issues & Solutions

### Issue 1: 403 Unauthorized on Inventory Transfers
**Problem**: Some users may not have permission to access inventory transfers.

**Solution**: The dashboard now handles 403 errors gracefully by:
- Silently failing without showing error messages
- Setting the metric count to 0
- Continuing to load other dashboard sections

### Issue 2: 500 Error with Multiple OrderBy Specifications
**Problem**: The Ardalis Specification library throws an error when multiple OrderBy clauses are chained together.

**Error Message**: "The specification contains more than one Order chain!"

**Root Cause**: The Store module specifications already have built-in ordering chains defined in their classes. For example:
- `SearchPickListsSpec` has: `.OrderByDescending(x => x.Priority).ThenByDescending(x => x.CreatedOn).ThenBy(x => x.PickListNumber)`
- `SearchGoodsReceiptsSpec` has: `.OrderByDescending(x => x.ReceivedDate, !request.HasOrderBy()).ThenBy(x => x.ReceiptNumber)`

When we pass `OrderBy` in the command, the base class `EntitiesByPaginationFilterSpec` applies it first, then the specification's built-in ordering (including `.ThenBy()` calls) tries to add to the chain, causing a conflict.

**Solution**: Remove all `OrderBy` parameters from search commands and let each specification use its built-in default ordering:
- **PickLists**: Orders by Priority desc, CreatedOn desc, PickListNumber
- **GoodsReceipts**: Orders by ReceivedDate desc, ReceiptNumber
- **InventoryTransfers**: Orders by TransferDate desc, TransferNumber
- **PutAwayTasks**: Orders by TaskNumber (default)
- **Items**: Orders by Name (default)

**Code Changes**:
```csharp
// Before: Passing OrderBy causing conflict
new SearchPickListsCommand
{
    Status = "InProgress",
    PageNumber = 1,
    PageSize = 10,
    OrderBy = new[] { "Priority desc" } // ❌ Conflicts with spec's built-in ordering
}

// After: No OrderBy, use spec's default
new SearchPickListsCommand
{
    Status = "InProgress",
    PageNumber = 1,
    PageSize = 10
    // ✅ Let specification use its built-in ordering
}
```

**Impact**: Data is now sorted using the specification's predefined logic, which is typically more appropriate than single-field sorting.

## Troubleshooting

### Dashboard Shows "Error loading..." Messages
1. Check user permissions for Store module endpoints
2. Verify API is running and accessible
3. Check browser console for detailed error messages
4. Ensure authentication token is valid

### Empty Tables Showing
1. Verify data exists in the database for the relevant entities
2. Check filter criteria (e.g., "InProgress" status for pick lists)
3. Ensure warehouse data is properly configured
4. Review API endpoint response in Network tab

### Performance Issues
1. Reduce PageSize in search commands if needed
2. Check database query performance
3. Consider adding indexes on commonly filtered fields
4. Monitor parallel loading performance

## API Permissions Required

To fully utilize the dashboard, users need the following permissions:
- `Permissions.Store.Items.View`
- `Permissions.Store.PurchaseOrders.View`
- `Permissions.Store.Warehouses.View`
- `Permissions.Store.Suppliers.View`
- `Permissions.Store.StockLevels.View`
- `Permissions.Store.GoodsReceipts.View`
- `Permissions.Store.InventoryTransfers.View` (optional - graceful fallback)
- `Permissions.Store.PickLists.View`
- `Permissions.Store.PutAwayTasks.View`
- `Permissions.Store.CycleCounts.View`

