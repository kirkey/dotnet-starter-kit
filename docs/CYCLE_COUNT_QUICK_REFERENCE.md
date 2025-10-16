# Cycle Count Management - Quick Reference Guide

## 📋 Overview
The Cycle Count Management page provides a complete interface for managing warehouse inventory cycle counting operations with full workflow support.

## 🔗 Access
**URL:** `/warehouse/cycle-counts`  
**Menu:** Warehouse → Cycle Counts

## ✨ Features Implemented

### Core Operations
- ✅ **Create** - Schedule new cycle counts
- ✅ **View** - Display cycle count details
- ✅ **Search** - List all cycle counts with pagination
- ✅ **Start** - Begin counting process
- ✅ **Complete** - Finish counting
- ✅ **Cancel** - Cancel with reason
- ✅ **Reconcile** - Adjust inventory

### Form Fields
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Count Number | Text | Yes | Unique identifier (e.g., CC-2025-001) |
| Warehouse | Dropdown | Yes | Warehouse selection |
| Location | Dropdown | No | Specific location within warehouse |
| Scheduled Date | Date | Yes | When count should occur |
| Count Type | Dropdown | Yes | Full, Partial, Spot, ABC |
| Counter Name | Text | No | Person performing count |
| Supervisor Name | Text | No | Person supervising count |
| Notes | Textarea | No | Additional information |

## 🔄 Workflow States

```
┌─────────────┐     Start      ┌─────────────┐    Complete    ┌───────────┐
│  Scheduled  │ ─────────────→ │ InProgress  │ ─────────────→ │ Completed │
└─────────────┘                └─────────────┘                └───────────┘
       │                              │                              │
       │                         Cancel                         Reconcile
       │                              ↓                              ↓
       │                        ┌───────────┐                 [Inventory]
       └──────────────────────→ │ Cancelled │                 [Adjusted]
                                └───────────┘
```

## 🎯 Action Buttons

| Action | Icon | Status Required | Description |
|--------|------|-----------------|-------------|
| Start | ▶️ | Scheduled | Begin the counting process |
| Complete | ✅ | InProgress | Mark count as finished |
| Reconcile | 🔄 | Completed | Adjust inventory levels |
| Cancel | ❌ | Any (not Completed) | Cancel the count |
| Add Item | ➕ | Any | Add items to count (placeholder) |

## 📊 Table Columns

| Column | Description |
|--------|-------------|
| Count # | Unique count identifier |
| Warehouse | Warehouse name |
| Location | Location within warehouse (if specified) |
| Date | Scheduled/actual count date |
| Status | Current workflow state |
| Type | Count type (Full/Partial/Spot/ABC) |
| Counted By | Person who performed count |
| Total Items | Total items in count |
| Counted | Items successfully counted |
| Variance | Items with discrepancies |

## 🛠️ Code Pattern Reference

### Component Structure
```csharp
public partial class CycleCounts : ComponentBase
{
    [Inject] protected IClient Client { get; set; }
    [Inject] protected ISnackbar Snackbar { get; set; }
    [Inject] protected IDialogService DialogService { get; set; }
    
    // EntityServerTableContext pattern
    // Workflow action methods
    // Reference data loading
}
```

### API Client Methods
```csharp
// Search
await Client.SearchCycleCountsEndpointAsync("1");

// Get Details
await Client.GetCycleCountEndpointAsync("1", id);

// Create
await Client.CreateCycleCountEndpointAsync("1", command);

// Workflow Operations
await Client.StartCycleCountEndpointAsync("1", id);
await Client.CompleteCycleCountEndpointAsync("1", id);
await Client.CancelCycleCountEndpointAsync("1", id, command);
await Client.ReconcileCycleCountEndpointAsync("1", id);

// Item Management (placeholders)
await Client.AddCycleCountItemEndpointAsync("1", id, command);
await Client.RecordCycleCountItemEndpointAsync("1", ccId, itemId, command);
```

## 🔧 Common Tasks

### Create a New Cycle Count
1. Click "+ Add Cycle Count" button
2. Fill in required fields (Count Number, Warehouse, Date, Type)
3. Optionally add Counter/Supervisor names
4. Click "Save"

### Start a Cycle Count
1. Find scheduled count in table
2. Click actions menu (⋮)
3. Select "Start"
4. Confirm dialog
5. Status changes to "InProgress"

### Complete a Cycle Count
1. Find in-progress count
2. Click actions menu (⋮)
3. Select "Complete"
4. Confirm dialog
5. Status changes to "Completed"

### Reconcile Inventory
1. Find completed count
2. Click actions menu (⋮)
3. Select "Reconcile"
4. Confirm dialog (WARNING: adjusts inventory!)
5. Inventory levels updated

### Cancel a Cycle Count
1. Find any non-completed count
2. Click actions menu (⋮)
3. Select "Cancel"
4. Confirm dialog
5. Status changes to "Cancelled"

## 📝 Best Practices

1. **Count Numbers**: Use consistent naming (e.g., CC-YYYY-MM-NNN)
2. **Scheduling**: Plan counts during low-activity periods
3. **Start Promptly**: Start counts near scheduled time
4. **Complete Thoroughly**: Ensure all items counted before completing
5. **Reconcile Carefully**: Review variances before reconciling
6. **Document Issues**: Use Notes field for important information

## 🚨 Error Handling

All operations include:
- ✅ User confirmation dialogs for destructive actions
- ✅ Try-catch error handling
- ✅ Success notifications (green snackbar)
- ✅ Error notifications (red snackbar)
- ✅ Automatic table refresh after operations

## 🔮 Future Enhancements

The following features are marked for future implementation:

- **Add Item Dialog**: Graphical interface to add items to count
- **Record Item Dialog**: Update counted quantities per item
- **Items Sub-table**: Expand row to see/edit items
- **Advanced Filtering**: Filter by warehouse, status, date range
- **Export**: Export count results to Excel/PDF
- **Variance Alerts**: Highlight high-variance items

## 📞 Support

For issues or questions:
1. Check backend API is running
2. Verify permissions (FshResources.Store)
3. Review browser console for errors
4. Check network tab for API failures

## 📚 Related Documentation

- [Full Implementation Summary](./CYCLE_COUNT_BLAZOR_IMPLEMENTATION.md)
- [Store Blazor API Mapping](./STORE_BLAZOR_API_ENDPOINT_MAPPING.md)
- [Code Patterns Guide](./CODE_PATTERNS_GUIDE.md)
