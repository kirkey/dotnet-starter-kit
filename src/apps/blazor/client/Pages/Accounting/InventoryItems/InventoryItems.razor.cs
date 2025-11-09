namespace FSH.Starter.Blazor.Client.Pages.Accounting.InventoryItems;

public partial class InventoryItems
{
    // Search filters
    private string? SearchSku;
    private string? SearchName;
    private bool SearchActiveOnly = true;

    protected EntityServerTableContext<InventoryItemResponse, DefaultIdType, InventoryItemViewModel> Context { get; set; } = null!;
    private EntityTable<InventoryItemResponse, DefaultIdType, InventoryItemViewModel>? _table;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<InventoryItemResponse, DefaultIdType, InventoryItemViewModel>(
            entityName: "Inventory Item",
            entityNamePlural: "Inventory Items",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<InventoryItemResponse>(i => i.Sku, "SKU", "Sku"),
                new EntityField<InventoryItemResponse>(i => i.Name, "Item Name", "Name"),
                new EntityField<InventoryItemResponse>(i => i.Quantity, "Quantity", "Quantity", typeof(decimal)),
                new EntityField<InventoryItemResponse>(i => i.UnitPrice, "Unit Price", "UnitPrice", typeof(decimal)),
                new EntityField<InventoryItemResponse>(i => i.Description, "Description", "Description"),
                new EntityField<InventoryItemResponse>(i => i.IsActive, "Active", "IsActive", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: i => i.Id,
            searchFunc: async filter =>
            {
                var request = new SearchInventoryItemsRequest
                {
                    Sku = SearchSku,
                    Name = SearchName,
                    IsActive = SearchActiveOnly ? true : null,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.InventoryItemSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<InventoryItemResponse>>();
            },
            createFunc: async vm => await CreateInventoryItemAsync(vm),
            updateFunc: async (id, vm) => await UpdateInventoryItemAsync(id, vm),
            deleteFunc: null,
            hasExtraActionsFunc: () => true);
        return Task.CompletedTask;
    }

    private async Task CreateInventoryItemAsync(InventoryItemViewModel vm)
    {
        var cmd = new CreateInventoryItemCommand
        {
            Sku = vm.Sku!,
            Name = vm.Name!,
            Quantity = vm.Quantity,
            UnitPrice = vm.UnitPrice,
            Description = vm.Description
        };
        await Client.InventoryItemCreateEndpointAsync("1", cmd);
        Snackbar.Add("Inventory item created successfully", Severity.Success);
    }

    private async Task UpdateInventoryItemAsync(DefaultIdType id, InventoryItemViewModel vm)
    {
        var cmd = new UpdateInventoryItemCommand
        {
            Id = id,
            Name = vm.Name,
            UnitPrice = vm.UnitPrice,
            Description = vm.Description
        };
        await Client.InventoryItemUpdateEndpointAsync("1", id, cmd);
        Snackbar.Add("Inventory item updated successfully", Severity.Success);
    }

    private async Task OnAddStock(InventoryItemResponse item)
    {
        var dialog = await DialogService.ShowAsync<InventoryItemAddStockDialog>("Add Stock", 
            new DialogParameters { ["ItemId"] = item.Id, ["ItemName"] = item.Name });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnReduceStock(InventoryItemResponse item)
    {
        var dialog = await DialogService.ShowAsync<InventoryItemReduceStockDialog>("Reduce Stock", 
            new DialogParameters { ["ItemId"] = item.Id, ["ItemName"] = item.Name });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnDeactivate(InventoryItemResponse item)
    {
        await Client.InventoryItemDeactivateEndpointAsync("1", item.Id);
        Snackbar.Add("Inventory item deactivated successfully", Severity.Success);
        if (_table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnViewDetails(InventoryItemResponse item)
    {
        var parameters = new DialogParameters { [nameof(InventoryItemDetailsDialog.Item)] = item };
        var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true, CloseOnEscapeKey = true };
        await DialogService.ShowAsync<InventoryItemDetailsDialog>("Inventory Item Details", parameters, options);
    }
}

