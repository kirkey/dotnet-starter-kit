# Store Module Blazor Pages - Quick Reference

## Page Inventory (21 Total)

### Core Entities (4)
| Page | Route | Operations | Workflow |
|------|-------|------------|----------|
| Categories | `/store/categories` | CRUD | - |
| Items | `/store/items` | CRUD | - |
| Suppliers | `/store/suppliers` | CRUD | - |
| PurchaseOrders | `/store/purchase-orders` | CRUD | Submit, Approve, Send, Receive, Cancel |

### Warehouse Management (4)
| Page | Route | Operations | Workflow |
|------|-------|------------|----------|
| Warehouses | `/store/warehouses` | CRUD | - |
| WarehouseLocations | `/store/warehouse-locations` | CRUD | - |
| Bins | `/store/bins` | CRUD | - |
| GoodsReceipts | `/store/goods-receipts` | CR-D | - |

### Inventory Tracking (6)
| Page | Route | Operations | Workflow |
|------|-------|------------|----------|
| ItemSuppliers | `/store/item-suppliers` | CRUD | - |
| LotNumbers | `/store/lot-numbers` | CRUD | - |
| SerialNumbers | `/store/serial-numbers` | CRUD | - |
| StockLevels | `/store/stock-levels` | CRUD | Reserve*, Allocate*, Release* |
| InventoryReservations | `/store/inventory-reservations` | CRUD | Release |
| InventoryTransactions | `/store/inventory-transactions` | CR-D | Approve |

### Operations (4)
| Page | Route | Operations | Workflow |
|------|-------|------------|----------|
| InventoryTransfers | `/store/inventory-transfers` | CRUD | Approve, MarkInTransit, Complete, Cancel |
| StockAdjustments | `/store/stock-adjustments` | CRUD | Approve |
| PickLists | `/store/pick-lists` | CR-D | Assign |
| PutAwayTasks | `/store/put-away-tasks` | CR-D | Assign |

### Dashboard (1)
| Page | Route | Operations |
|------|-------|------------|
| StoreDashboard | `/store/dashboard` | View statistics |

**Legend**: 
- CRUD = Create, Read, Update, Delete
- CR-D = Create, Read, Delete (no Update)
- \* = Placeholder UI only, API not implemented

---

## Autocomplete Components

### Available Components
```csharp
// Item selection
<AutocompleteItem @bind-Value="context.ItemId" Label="Item" Required="true" />

// Warehouse selection  
<AutocompleteWarehouseId @bind-Value="context.WarehouseId" Label="Warehouse" Required="true" />

// Supplier selection (supports nullable IDs)
<AutocompleteSupplier @bind-Value="context.SupplierId" Label="Supplier" />

// Category selection
<AutocompleteCategoryId @bind-Value="context.CategoryId" Label="Category" />
```

---

## Common Form Patterns

### Text Input
```razor
<MudTextField @bind-Value="context.Name" For="@(() => context.Name)" Label="Name" Required="true" />
```

### Numeric Input
```razor
<MudNumericField @bind-Value="context.Quantity" For="@(() => context.Quantity)" Label="Quantity" Required="true" />
```

### Date Picker
```razor
<MudDatePicker @bind-Date="context.Date" For="@(() => context.Date)" Label="Date" Required="true" />
```

### Select Dropdown
```razor
<MudSelect @bind-Value="context.Status" For="@(() => context.Status)" Label="Status" Required="true">
    <MudSelectItem Value="@("Draft")">Draft</MudSelectItem>
    <MudSelectItem Value="@("Active")">Active</MudSelectItem>
</MudSelect>
```

### Multi-line Text
```razor
<MudTextField @bind-Value="context.Notes" For="@(() => context.Notes)" Label="Notes" Lines="3" />
```

---

## Workflow Operation Pattern

### Single Operation (e.g., Approve)
```razor
<ExtraActions Context="context">
    <MudMenuItem OnClick="@(() => ApproveItem(context.Id))">
        <div class="d-flex align-center">
            <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Class="mr-2" />
            <span>Approve</span>
        </div>
    </MudMenuItem>
</ExtraActions>
```

```csharp
private async Task ApproveItem(DefaultIdType id)
{
    bool? result = await DialogService.ShowMessageBox(
        "Confirm Approval",
        "Are you sure you want to approve this item?",
        yesText: "Approve",
        cancelText: "Cancel");

    if (result == true)
    {
        var command = new ApproveItemCommand();
        await Client.ApproveItemEndpointAsync("1", id, command);
        await _table.ReloadDataAsync();
    }
}
```

### Status-Based Operations (e.g., Workflow)
```razor
<ExtraActions Context="context">
    @if (context.Status == "Draft")
    {
        <MudMenuItem OnClick="@(() => Submit(context.Id))">
            <div class="d-flex align-center">
                <MudIcon Icon="@Icons.Material.Filled.Send" Class="mr-2" />
                <span>Submit</span>
            </div>
        </MudMenuItem>
    }
    @if (context.Status == "Submitted")
    {
        <MudMenuItem OnClick="@(() => Approve(context.Id))">
            <div class="d-flex align-center">
                <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Class="mr-2" />
                <span>Approve</span>
            </div>
        </MudMenuItem>
    }
</ExtraActions>
```

---

## EntityServerTableContext Setup

### Standard Pattern
```csharp
protected override void OnInitialized()
{
    Context = new EntityServerTableContext<ResponseType, DefaultIdType, ViewModelType>(
        entityName: "Entity Name",
        entityNamePlural: "Entity Names",
        entityResource: FshResources.Store,
        fields:
        [
            new EntityField<ResponseType>(x => x.Name, "Name", "Name"),
            new EntityField<ResponseType>(x => x.Status, "Status", "Status"),
            new EntityField<ResponseType>(x => x.Date, "Date", "Date", typeof(DateTime))
        ],
        enableAdvancedSearch: true,
        idFunc: response => response.Id,
        getDetailsFunc: async id =>
        {
            var dto = await Client.GetEntityEndpointAsync("1", id).ConfigureAwait(false);
            return dto.Adapt<ViewModelType>();
        },
        searchFunc: async filter =>
        {
            var paginationFilter = filter.Adapt<PaginationFilter>();
            var command = paginationFilter.Adapt<SearchEntitiesCommand>();
            var result = await Client.SearchEntitiesEndpointAsync("1", command).ConfigureAwait(false);
            return result.Adapt<PaginationResponse<ResponseType>>();
        },
        createFunc: async viewModel =>
        {
            await Client.CreateEntityEndpointAsync("1", viewModel.Adapt<CreateEntityCommand>()).ConfigureAwait(false);
        },
        updateFunc: async (id, viewModel) =>
        {
            await Client.UpdateEntityEndpointAsync("1", id, viewModel.Adapt<UpdateEntityCommand>()).ConfigureAwait(false);
        },
        deleteFunc: async id => await Client.DeleteEntityEndpointAsync("1", id).ConfigureAwait(false));
}
```

### Without Update (Immutable Records)
```csharp
// Remove updateFunc parameter entirely
Context = new EntityServerTableContext<ResponseType, DefaultIdType, ViewModelType>(
    // ... other parameters ...
    createFunc: async viewModel => { /* ... */ },
    deleteFunc: async id => { /* ... */ });
```

---

## Common Field Types

```csharp
// Text field
new EntityField<ResponseType>(x => x.Name, "Name", "Name")

// Numeric field
new EntityField<ResponseType>(x => x.Quantity, "Qty", "Quantity", typeof(int))
new EntityField<ResponseType>(x => x.Price, "Price", "Price", typeof(double))

// Date field
new EntityField<ResponseType>(x => x.Date, "Date", "Date", typeof(DateTime))
new EntityField<ResponseType>(x => x.OptionalDate, "Date", "OptionalDate", typeof(DateTime?))

// Boolean field
new EntityField<ResponseType>(x => x.IsActive, "Active", "IsActive", typeof(bool))
```

---

## Icons Reference

### Common Icons Used
```csharp
Icons.Material.Filled.CheckCircle     // Approve
Icons.Material.Filled.Send            // Submit/Send
Icons.Material.Filled.Cancel          // Cancel
Icons.Material.Filled.Done            // Complete
Icons.Material.Filled.LocalShipping   // Ship/Transit
Icons.Material.Filled.Inbox           // Receive
Icons.Material.Filled.Visibility      // View
Icons.Material.Filled.PersonAdd       // Assign
```

---

## File Structure

```
/Pages/Store/
├── Categories.razor & .cs
├── Items.razor & .cs
├── Suppliers.razor & .cs
├── PurchaseOrders.razor & .cs
├── Warehouses.razor & .cs
├── WarehouseLocations.razor & .cs
├── Bins.razor & .cs
├── ItemSuppliers.razor & .cs
├── LotNumbers.razor & .cs
├── SerialNumbers.razor & .cs
├── StockLevels.razor & .cs
├── InventoryReservations.razor & .cs
├── InventoryTransactions.razor & .cs
├── InventoryTransfers.razor & .cs
├── StockAdjustments.razor & .cs
├── GoodsReceipts.razor & .cs
├── PickLists.razor & .cs
├── PutAwayTasks.razor & .cs
├── StoreDashboard.razor & .cs
└── Components/
    ├── PurchaseOrderItems.razor
    ├── PurchaseOrderItemDialog.razor
    └── PurchaseOrderItemModel.cs
```

---

## Troubleshooting

### DialogService Ambiguity Warning
**Issue**: "Ambiguity between 'PageName.DialogService' and 'PageName.DialogService'"
**Solution**: This is an analyzer warning only, doesn't affect compilation or runtime. Can be safely ignored.

### Unused _table Field Warning
**Issue**: "Unused field '_table'"
**Solution**: Field is required for `@ref="_table"` binding in razor file. Warning can be ignored.

### Property Not Found
**Issue**: Property name doesn't match API response
**Solution**: 
1. Search Client.cs for the response class: `grep "public partial class EntityResponse"`
2. Read the properties to find actual property names
3. Update EntityField and ViewModel to match

### Nullable ID Issues
**Issue**: Autocomplete component doesn't accept nullable IDs
**Solution**: Use AutocompleteSupplier (supports nullable) or make ViewModel property non-nullable

---

## Testing Checklist

- [ ] Create new record
- [ ] View record details
- [ ] Update record (if supported)
- [ ] Delete record
- [ ] Search/filter records
- [ ] Sort by different columns
- [ ] Page through results
- [ ] Test autocomplete selections
- [ ] Test form validation
- [ ] Test workflow operations
- [ ] Test error scenarios

---

## Performance Tips

1. **Use ConfigureAwait(false)**: Always append `.ConfigureAwait(false)` to async calls
2. **Enable Advanced Search**: Set `enableAdvancedSearch: true` for better filtering
3. **Limit Table Fields**: Show only essential columns (5-8 fields maximum)
4. **Use Pagination**: EntityTable handles pagination automatically
5. **Cache Autocomplete**: Autocomplete components cache results by default

---

## Security

All pages use `FshResources.Store` for authorization:
```csharp
entityResource: FshResources.Store
```

This requires users to have appropriate Store module permissions.

---

**Last Updated**: October 4, 2025
**Version**: 1.0
**Status**: Production Ready
