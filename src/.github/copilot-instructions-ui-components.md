# 🧩 COPILOT INSTRUCTIONS - UI COMPONENTS

**Last Updated**: November 20, 2025  
**Status**: ✅ Production Ready - Core Components (EntityTable, Autocomplete, Dialogs)  
**Scope**: Reusable UI component patterns

> **📌 Reference other files:**
> - `copilot-instructions-ui-foundation.md` - Core UI principles
> - `copilot-instructions-ui-accounting.md` - Accounting patterns
> - `copilot-instructions-ui-store.md` - Store patterns
> - `copilot-instructions-ui-hr.md` - HR patterns

---

## 🗂️ ENTITYTABLE COMPONENT PATTERN

### **✅ EntityTable Architecture**
- ✅ **Generic Table**: `EntityTable<TEntity, TId, TRequest>`
- ✅ **Server/Client Pagination**: Support for both modes
- ✅ **CRUD Operations**: Built-in Create, Read, Update, Delete
- ✅ **Advanced Search**: Expandable search panel with filters
- ✅ **Export/Import**: Excel export functionality
- ✅ **Permission-Based**: Role-based action visibility

**EntityTable Core Features:**
```csharp
/// <summary>
/// EntityTable component for displaying and managing entities with CRUD operations.
/// </summary>
/// <typeparam name="TEntity">The entity type to display.</typeparam>
/// <typeparam name="TId">The entity ID type.</typeparam>
/// <typeparam name="TRequest">The request DTO for create/update operations.</typeparam>
<EntityTable @ref="_table" 
             TEntity="ItemResponse" 
             TId="DefaultIdType" 
             TRequest="ItemViewModel" 
             Context="@Context">
    
    <AdvancedSearchContent>
        <!-- Custom search filters -->
    </AdvancedSearchContent>
    
    <ExtraActions Context="entity">
        <!-- Status-based contextual actions -->
    </ExtraActions>
    
    <EditFormContent Context="context">
        <!-- Form fields for create/edit -->
    </EditFormContent>
    
</EntityTable>
```

### **✅ EntityTableContext Configuration**
- ✅ **Field Definitions**: Define columns to display
- ✅ **Search Function**: Server or client-side search
- ✅ **CRUD Functions**: Create, Update, Delete handlers
- ✅ **Permission Mapping**: Resource and action permissions
- ✅ **Custom Functions**: Defaults, details, duplicate

**Context Setup Pattern:**
```csharp
protected override Task OnInitializedAsync()
{
    Context = new EntityServerTableContext<ItemResponse, DefaultIdType, ItemViewModel>(
        entityName: "Item",
        entityNamePlural: "Items",
        entityResource: FshResources.Store,
        fields:
        [
            new EntityField<ItemResponse>(r => r.Sku, "SKU", "Sku"),
            new EntityField<ItemResponse>(r => r.Name, "Name", "Name"),
            new EntityField<ItemResponse>(r => r.CategoryName, "Category", "CategoryName"),
            new EntityField<ItemResponse>(r => r.UnitPrice, "Price", "UnitPrice", typeof(decimal)),
            new EntityField<ItemResponse>(r => r.MinimumStock, "Min Stock", "MinimumStock", typeof(int)),
        ],
        enableAdvancedSearch: true,
        idFunc: response => response.Id,
        searchFunc: async filter =>
        {
            var paginationFilter = filter.Adapt<PaginationFilter>();
            var result = await Client.SearchItemsEndpointAsync("1", paginationFilter);
            return result.Adapt<PaginationResponse<ItemResponse>>();
        },
        createFunc: async item =>
        {
            await Client.CreateItemEndpointAsync("1", item.Adapt<CreateItemCommand>());
        },
        updateFunc: async (id, item) =>
        {
            await Client.UpdateItemEndpointAsync("1", id, item.Adapt<UpdateItemCommand>());
        },
        deleteFunc: async id => await Client.DeleteItemEndpointAsync("1", id));

    return Task.CompletedTask;
}
```

### **✅ EntityField Configuration**
```csharp
// Simple field
new EntityField<TEntity>(response => response.Name, "Name", "Name")

// Typed field (for proper sorting/filtering)
new EntityField<TEntity>(response => response.Price, "Price", "Price", typeof(decimal))
new EntityField<TEntity>(response => response.Date, "Date", "Date", typeof(DateOnly))

// Custom template
new EntityField<TEntity>(
    response => response.Status, 
    "Status", 
    "Status",
    Template: entity => @<MudChip Color="@GetStatusColor(entity.Status)">@entity.Status</MudChip>
)

// Boolean field with icon
new EntityField<TEntity>(
    response => response.IsActive,
    "Active",
    "IsActive",
    typeof(bool),
    Template: entity => @<MudIcon Icon="@(entity.IsActive ? Icons.Material.Filled.Check : Icons.Material.Filled.Close)" 
                                  Color="@(entity.IsActive ? Color.Success : Color.Error)" />
)
```

---

## 🔍 AUTOCOMPLETE COMPONENTS

### **✅ AutocompleteBase Pattern**
- ✅ **Base Pattern**: `AutocompleteBase<TDto, TClient, TKey>`
- ✅ **Search Integration**: API-driven search with caching
- ✅ **Display Formatting**: Custom text display functions
- ✅ **Lazy Loading**: Load on demand with dictionary caching
- ✅ **Validation Support**: Integrated with FluentValidation

**Autocomplete Base Pattern:**
```csharp
public abstract class AutocompleteBase<TDto, TClient, TKey> : MudAutocomplete<TKey>
    where TDto : class, new()
    where TClient : class
{
    protected Dictionary<TKey, TDto> _dictionary = [];
    [Inject] protected TClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        CoerceText = CoerceValue = Clearable = Dense = ResetValueOnEmptyText = true;
        SearchFunc = SearchText!;
        ToStringFunc = GetTextValue;
        Variant = Variant.Filled;
        return base.SetParametersAsync(parameters);
    }

    protected abstract Task<TDto?> GetItem(TKey code);
    protected abstract Task<IEnumerable<TKey>> SearchText(string? value, CancellationToken token);
    protected abstract string GetTextValue(TKey? code);
}
```

### **✅ Implementing Custom Autocomplete**
```csharp
/// <summary>
/// Autocomplete component for selecting a Warehouse by ID.
/// </summary>
public class AutocompleteWarehouse : AutocompleteBase<WarehouseResponse, IClient, DefaultIdType>
{
    protected override async Task<WarehouseResponse?> GetItem(DefaultIdType id)
    {
        if (_dictionary.TryGetValue(id, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.GetWarehouseEndpointAsync("1", id))
            .ConfigureAwait(false);

        if (dto is not null) _dictionary[id] = dto;
        return dto;
    }

    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchWarehousesRequest
        {
            PageNumber = 1,
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = ["name", "description"],
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SearchWarehousesEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        if (response?.Items is { } items)
        {
            _dictionary = items
                .Where(x => x.Id != DefaultIdType.Empty)
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .ToDictionary(x => x.Id);
        }

        return _dictionary.Keys;
    }

    protected override string GetTextValue(DefaultIdType? code) =>
        code is not null && _dictionary.TryGetValue(code, out var item)
            ? item.Name ?? string.Empty
            : string.Empty;
}
```

### **✅ Using Autocomplete in Forms**
```csharp
<MudItem xs="12" sm="6" md="4">
    <AutocompleteWarehouse @bind-Value="context.FromWarehouseId" 
                          For="@(() => context.FromWarehouseId)" 
                          Label="From Warehouse" 
                          Required="true" />
</MudItem>

<MudItem xs="12" sm="6" md="4">
    <AutocompleteCategoryId @bind-Value="context.CategoryId"
                           For="@(() => context.CategoryId)"
                           Label="Category"
                           Variant="Variant.Filled" />
</MudItem>

<MudItem xs="12" sm="6" md="4">
    <AutocompleteBrand @bind-Value="context.Brand"
                      For="@(() => context.Brand!)"
                      Label="Brand"
                      Variant="Variant.Filled" />
</MudItem>
```

---

## 💬 DIALOG PATTERNS

### **✅ AddEditModal Pattern**
- ✅ **Generic Modal**: Handles Create/Edit/View modes
- ✅ **Validation Integration**: DataAnnotationsValidator + FluentValidation
- ✅ **Mode Detection**: IsCreate, IsViewMode properties
- ✅ **Icon Indicators**: Different icons per mode
- ✅ **Cascading Values**: ReadOnly mode cascades to children

**AddEditModal Structure:**
```csharp
<EditForm Model="@RequestModel" OnValidSubmit="SaveAsync">
    <MudDialog>
        <TitleContent>
            <MudText Typo="Typo.h6">
                @if (IsViewMode)
                {
                    <MudIcon Icon="@Icons.Material.Filled.Visibility" Class="mr-3 mb-n1" />
                }
                else if (IsCreate)
                {
                    <MudIcon Icon="@Icons.Material.Filled.Add" Class="mr-3 mb-n1" />
                }
                else
                {
                    <MudIcon Icon="@Icons.Material.Filled.Update" Class="mr-3 mb-n1" />
                }
                @Title
            </MudText>
        </TitleContent>

        <DialogContent>
            @if (!IsViewMode)
            {
                <DataAnnotationsValidator />
                <FshValidation @ref="_customValidation" />
            }
            <MudGrid Spacing="2">
                <CascadingValue Value="@IsViewMode" Name="IsReadOnly">
                    @ChildContent(RequestModel)
                </CascadingValue>
            </MudGrid>
        </DialogContent>

        <DialogActions>
            @if (!IsViewMode)
            {
                <MudButton OnClick="MudDialog.Cancel" 
                          StartIcon="@Icons.Material.Filled.Cancel" 
                          Variant="Variant.Filled">
                   Cancel
                </MudButton>
                @if (IsCreate)
                {
                    <MudButton ButtonType="ButtonType.Submit" 
                              Color="Color.Success" 
                              StartIcon="@Icons.Material.Filled.Save" 
                              Variant="Variant.Filled">
                        Save
                    </MudButton>
                }
                else
                {
                    <MudButton ButtonType="ButtonType.Submit" 
                              Color="Color.Secondary" 
                              StartIcon="@Icons.Material.Filled.PlaylistAddCheck" 
                              Variant="Variant.Filled">
                        Update
                    </MudButton>
                }
            }
            else
            {
                <MudButton OnClick="MudDialog.Cancel" 
                          StartIcon="@Icons.Material.Filled.Close" 
                          Variant="Variant.Filled" 
                          Color="Color.Primary">
                   Close
                </MudButton>
            }
        </DialogActions>
    </MudDialog>
</EditForm>
```

### **✅ Details Dialog Pattern**
- ✅ **Read-Only Display**: View entity details
- ✅ **Tabular Layout**: MudSimpleTable for key-value pairs
- ✅ **Status Chips**: Color-coded status indicators
- ✅ **Related Data**: Links to related entities
- ✅ **Conditional Display**: Show/hide based on data

**Details Dialog Structure:**
```csharp
<MudDialog>
    <DialogContent>
        <MudContainer Style="max-height: 70vh; overflow-y: auto;">
            @if (_loading)
            {
                <MudProgressCircular Color="Color.Primary" Indeterminate="true" />
            }
            else if (_entity != null)
            {
                <MudGrid>
                    <MudItem xs="12">
                        <MudText Typo="Typo.h5" Class="mb-4">@Title Details</MudText>
                    </MudItem>

                    <MudItem xs="12">
                        <MudSimpleTable Dense="true" Style="margin-bottom: 1rem;">
                            <tbody>
                                <tr>
                                    <td style="width: 35%; font-weight: 500;">Number</td>
                                    <td><strong>@_entity.Number</strong></td>
                                </tr>
                                <tr>
                                    <td style="font-weight: 500;">Status</td>
                                    <td>
                                        <MudChip T="string" 
                                                Color="@GetStatusColor(_entity.Status)" 
                                                Size="Size.Small">
                                            @_entity.Status
                                        </MudChip>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-weight: 500;">Date</td>
                                    <td>@_entity.Date.ToString("MMMM dd, yyyy")</td>
                                </tr>
                                @if (!string.IsNullOrEmpty(_entity.Description))
                                {
                                    <tr>
                                        <td style="font-weight: 500;">Description</td>
                                        <td>@_entity.Description</td>
                                    </tr>
                                }
                            </tbody>
                        </MudSimpleTable>
                    </MudItem>

                    <MudItem xs="12">
                        <MudDivider Class="my-4" />
                    </MudItem>

                    <MudItem xs="12">
                        <MudText Typo="Typo.h6" Class="mb-3">Line Items</MudText>
                        @if (_entity.Items?.Any() == true)
                        {
                            <MudTable Items="@_entity.Items" 
                                     Dense="true" 
                                     Hover="true" 
                                     Bordered="true" 
                                     Striped="true">
                                <HeaderContent>
                                    <MudTh>Item</MudTh>
                                    <MudTh Style="text-align: right;">Quantity</MudTh>
                                    <MudTh Style="text-align: right;">Unit Price</MudTh>
                                    <MudTh Style="text-align: right;">Total</MudTh>
                                </HeaderContent>
                                <RowTemplate>
                                    <MudTd DataLabel="Item">@context.Name</MudTd>
                                    <MudTd DataLabel="Quantity" Style="text-align: right;">
                                        @context.Quantity
                                    </MudTd>
                                    <MudTd DataLabel="Unit Price" Style="text-align: right;">
                                        @context.UnitPrice.ToString("C2")
                                    </MudTd>
                                    <MudTd DataLabel="Total" Style="text-align: right;">
                                        @((context.Quantity * context.UnitPrice).ToString("C2"))
                                    </MudTd>
                                </RowTemplate>
                            </MudTable>
                        }
                        else
                        {
                            <MudAlert Severity="Severity.Info">No items found.</MudAlert>
                        }
                    </MudItem>
                </MudGrid>
            }
        </MudContainer>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="@Close" 
                  Color="Color.Primary" 
                  Variant="Variant.Filled">
            Close
        </MudButton>
    </DialogActions>
</MudDialog>
```

---

## 🛠️ COMMON SERVICE PATTERNS

### **✅ DialogService Extensions**
- ✅ **Modal Helper**: Simplified dialog invocation
- ✅ **Consistent Options**: Standard dialog configuration
- ✅ **Async Result**: Easy result handling

**DialogService Extensions:**
```csharp
public static class DialogServiceExtensions
{
    public static Task<DialogResult> ShowModalAsync<TDialog>(
        this IDialogService dialogService, 
        DialogParameters parameters)
        where TDialog : ComponentBase =>
        dialogService.ShowModal<TDialog>(parameters).Result!;

    public static IDialogReference ShowModal<TDialog>(
        this IDialogService dialogService, 
        DialogParameters parameters)
        where TDialog : ComponentBase
    {
        var options = new DialogOptions
        {
            BackdropClick = false,
            CloseButton = true, 
            FullWidth = true, 
            MaxWidth = MaxWidth.Large, 
        };

        return dialogService.Show<TDialog>(string.Empty, parameters, options);
    }
}
```

**Usage:**
```csharp
private async Task AddContact()
{
    var parameters = new DialogParameters
    {
        { nameof(EmployeeContactDialog.EmployeeId), EmployeeId },
        { nameof(EmployeeContactDialog.Contact), null }
    };
    
    var result = await DialogService.ShowModalAsync<EmployeeContactDialog>(parameters);
    
    if (!result.Canceled)
    {
        await LoadContacts();
    }
}
```

---

## 📊 COMPONENTS CHECKLIST

### **EntityTable Implementation**
- [ ] Configure EntityTableContext with all required fields
- [ ] Define EntityField for each column
- [ ] Implement searchFunc for server pagination
- [ ] Implement createFunc, updateFunc, deleteFunc
- [ ] Add AdvancedSearchContent for custom filters
- [ ] Add ExtraActions for contextual menu items
- [ ] Add EditFormContent for form fields

### **Autocomplete Components**
- [ ] Inherit from AutocompleteBase<TDto, TClient, TKey>
- [ ] Implement GetItem for single item retrieval
- [ ] Implement SearchText for search functionality
- [ ] Implement GetTextValue for display formatting
- [ ] Cache results in dictionary for performance
- [ ] Handle null/empty values gracefully

### **Dialog Implementation**
- [ ] Use AddEditModal for CRUD operations
- [ ] Create custom detail dialogs for read-only views
- [ ] Implement loading states with progress indicators
- [ ] Use MudSimpleTable for key-value displays
- [ ] Add related entity links where applicable
- [ ] Include line item tables for detailed views

---

## ✅ VERIFICATION STATUS

**EntityTable Component**: ✅ A+ Documented  
**Autocomplete Components**: ✅ A+ Documented  
**Dialog Patterns**: ✅ A+ Documented  
**Common Services**: ✅ A+ Documented  

**Last verified**: November 20, 2025

