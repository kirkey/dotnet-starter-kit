# 📦 COPILOT INSTRUCTIONS - UI STORE MODULE

**Last Updated**: November 20, 2025  
**Status**: ✅ Production Ready - Store Module UI Patterns  
**Scope**: Store/Inventory-specific UI patterns and workflows

> **📌 Reference other files:**
> - `copilot-instructions-ui-foundation.md` - Core UI principles
> - `copilot-instructions-ui-components.md` - Core components
> - `copilot-instructions-ui-accounting.md` - Accounting patterns
> - `copilot-instructions-ui-hr.md` - HR patterns

---

## ✅ INVENTORY MANAGEMENT WORKFLOWS

- ✅ **Status-Based Workflows**: Draft → Approved → InTransit → Completed
- ✅ **Transfer Management**: Between warehouse transfers with approval process
- ✅ **Stock Tracking**: Serial numbers, lot numbers, bin locations
- ✅ **Inventory Transactions**: Real-time stock level updates
- ✅ **Multi-Attribute Items**: Weight, dimensions, perishability tracking

**Inventory Transfer Workflow Pattern:**
```csharp
<ExtraActions Context="transfer">
    @if (transfer.Status == "Draft")
    {
        <MudMenuItem OnClick="@(() => ApproveTransfer(transfer.Id))">
            <div class="d-flex align-center">
                <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Class="mr-2" />
                <span>Approve Transfer</span>
            </div>
        </MudMenuItem>
    }
    @if (transfer.Status == "Approved")
    {
        <MudMenuItem OnClick="@(() => MarkInTransit(transfer.Id))">
            <div class="d-flex align-center">
                <MudIcon Icon="@Icons.Material.Filled.LocalShipping" Class="mr-2" />
                <span>Mark In Transit</span>
            </div>
        </MudMenuItem>
    }
    @if (transfer.Status == "InTransit")
    {
        <MudMenuItem OnClick="@(() => CompleteTransfer(transfer.Id))">
            <div class="d-flex align-center">
                <MudIcon Icon="@Icons.Material.Filled.Done" Class="mr-2" />
                <span>Complete Transfer</span>
            </div>
        </MudMenuItem>
    }
    @if (transfer.Status is "Draft" or "Approved")
    {
        <MudMenuItem OnClick="@(() => CancelTransfer(transfer.Id))">
            <div class="d-flex align-center">
                <MudIcon Icon="@Icons.Material.Filled.Cancel" Class="mr-2" Color="Color.Error" />
                <span>Cancel Transfer</span>
            </div>
        </MudMenuItem>
    }
</ExtraActions>
```

---

## ✅ STOCK TRACKING PATTERNS

- ✅ **Item Attributes**: SKU, Barcode, Serial, Lot tracking
- ✅ **Location Tracking**: Bin, Aisle, Section management
- ✅ **Quantity Management**: Min/Max stock levels, Reorder points
- ✅ **Multi-Unit Support**: Weight units, dimensions, packaging

**Stock Level Display:**
```csharp
<MudGrid>
    <MudItem xs="12" sm="6" md="3">
        <MudNumericField T="int?" @bind-Value="context.MinimumStock" 
                        Label="Min Stock" Immediate="true" />
    </MudItem>
    <MudItem xs="12" sm="6" md="3">
        <MudNumericField T="int?" @bind-Value="context.MaximumStock" 
                        Label="Max Stock" Immediate="true" />
    </MudItem>
    <MudItem xs="12" sm="6" md="3">
        <MudNumericField T="int?" @bind-Value="context.ReorderPoint" 
                        Label="Reorder Point" Immediate="true" />
    </MudItem>
    <MudItem xs="12" sm="6" md="3">
        <MudNumericField T="int?" @bind-Value="context.ReorderQuantity" 
                        Label="Reorder Qty" Immediate="true" />
    </MudItem>
</MudGrid>
```

---

## ✅ WAREHOUSE OPERATIONS

- ✅ **Warehouse Selection**: Multi-warehouse support
- ✅ **Location Management**: Bins, aisles, shelves
- ✅ **Transfer Types**: Standard, Emergency, Replenishment, Return
- ✅ **Priority Handling**: Low, Normal, High, Urgent

**Warehouse Selection Pattern:**
```csharp
<MudItem xs="12" sm="6" md="4">
    <AutocompleteWarehouse @bind-Value="context.FromWarehouseId" 
                          For="@(() => context.FromWarehouseId)" 
                          Label="From Warehouse" 
                          Required="true" />
</MudItem>

<MudItem xs="12" sm="6" md="4">
    <AutocompleteWarehouse @bind-Value="context.ToWarehouseId" 
                          For="@(() => context.ToWarehouseId)" 
                          Label="To Warehouse" 
                          Required="true" />
</MudItem>

<MudItem xs="12" sm="6" md="4">
    <MudSelect @bind-Value="context.TransferType" 
               For="@(() => context.TransferType)" 
               Label="Transfer Type" 
               Required="true">
        <MudSelectItem Value="@("Standard")">Standard</MudSelectItem>
        <MudSelectItem Value="@("Emergency")">Emergency</MudSelectItem>
        <MudSelectItem Value="@("Replenishment")">Replenishment</MudSelectItem>
        <MudSelectItem Value="@("Return")">Return</MudSelectItem>
    </MudSelect>
</MudItem>
```

---

## 📊 STORE UI CHECKLIST

### **Inventory Management**
- [ ] Item tracking with SKU, barcode, serial/lot numbers
- [ ] Multi-warehouse support with location tracking
- [ ] Stock level monitoring (min/max/reorder point)
- [ ] Perishable item handling with shelf life
- [ ] Weight and dimension tracking
- [ ] Supplier management integration

### **Workflow Implementation**
- [ ] Status-based conditional actions in ExtraActions
- [ ] Transfer workflows (Draft → Approved → InTransit → Completed)
- [ ] Approval processes with role-based permissions
- [ ] Cancellation capability for draft/approved items
- [ ] Real-time status updates

### **Autocomplete Components**
- [ ] Inherit from AutocompleteBase<TDto, TClient, TKey>
- [ ] Implement GetItem for single item retrieval
- [ ] Implement SearchText for search functionality
- [ ] Implement GetTextValue for display formatting
- [ ] Cache results in dictionary for performance
- [ ] Handle null/empty values gracefully

### **EntityTable Usage**
- [ ] Configure EntityTableContext with all required fields
- [ ] Define EntityField for each column
- [ ] Implement searchFunc for server pagination
- [ ] Implement createFunc, updateFunc, deleteFunc
- [ ] Add AdvancedSearchContent for custom filters
- [ ] Add ExtraActions for contextual menu items
- [ ] Add EditFormContent for form fields

### **Dialog Implementation**
- [ ] Use AddEditModal for CRUD operations
- [ ] Create custom detail dialogs for read-only views
- [ ] Implement loading states with progress indicators
- [ ] Use MudSimpleTable for key-value displays
- [ ] Add related entity links where applicable
- [ ] Include line item tables for detailed views

---

## ✅ VERIFICATION STATUS

**Store Module**: ✅ A+ Verified & Documented  

**Last verified**: November 20, 2025

