namespace FSH.Starter.Blazor.Client.Pages.Store.Warehouses;

/// <summary>
/// Warehouses page logic. Provides CRUD and server-side search over Warehouse entities via the generated API client.
/// </summary>
public partial class Warehouses
{
    /// <summary>
    /// The entity table context for managing warehouses with server-side operations.
    /// </summary>
    protected EntityServerTableContext<WarehouseResponse, DefaultIdType, WarehouseViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for warehouses.
    /// </summary>
    private EntityTable<WarehouseResponse, DefaultIdType, WarehouseViewModel> _table = null!;

    /// <summary>
    /// Search filter for warehouse name.
    /// </summary>
    private string? SearchName { get; set; }

    /// <summary>
    /// Search filter for warehouse code.
    /// </summary>
    private string? SearchCode { get; set; }

    /// <summary>
    /// Search filter for active status.
    /// </summary>
    private bool? SearchIsActive { get; set; }

    /// <summary>
    /// Search filter for main warehouse.
    /// </summary>
    private bool? SearchIsMainWarehouse { get; set; }

    /// <summary>
    /// Dialog options for modal dialogs.
    /// </summary>
    private readonly DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        MaxWidth = MaxWidth.ExtraLarge,
        FullWidth = true
    };

    /// <summary>
    /// Gets the status color based on active status.
    /// </summary>
    private static Color GetStatusColor(bool isActive) => isActive ? Color.Success : Color.Default;

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<WarehouseResponse, DefaultIdType, WarehouseViewModel>(
            entityName: "Warehouse",
            entityNamePlural: "Warehouses",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<WarehouseResponse>(response => response.Code, "Code", "Code"),
                new EntityField<WarehouseResponse>(response => response.Name, "Name", "Name"),
                new EntityField<WarehouseResponse>(response => response.WarehouseType, "Type", "WarehouseType"),
                new EntityField<WarehouseResponse>(response => response.Address, "Address", "Address"),
                new EntityField<WarehouseResponse>(response => response.ManagerName, "Manager", "ManagerName"),
                new EntityField<WarehouseResponse>(response => response.TotalCapacity, "Capacity", "TotalCapacity", typeof(decimal)),
                new EntityField<WarehouseResponse>(response => response.CapacityUnit, "Unit", "CapacityUnit"),
                new EntityField<WarehouseResponse>(response => response.IsMainWarehouse, "Main", "IsMainWarehouse", typeof(bool)),
                new EntityField<WarehouseResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchWarehousesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    Name = SearchName,
                    Code = SearchCode,
                    IsActive = SearchIsActive,
                    IsMainWarehouse = SearchIsMainWarehouse
                };
                var result = await Client.SearchWarehousesEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<WarehouseResponse>>();
            },
            createFunc: async viewModel =>
            {
                var command = new CreateWarehouseCommand
                {
                    Code = viewModel.Code!,
                    Name = viewModel.Name!,
                    Description = viewModel.Description,
                    Address = viewModel.Address!,
                    ManagerName = viewModel.ManagerName!,
                    ManagerEmail = viewModel.ManagerEmail!,
                    ManagerPhone = viewModel.ManagerPhone!,
                    TotalCapacity = viewModel.TotalCapacity,
                    CapacityUnit = viewModel.CapacityUnit!,
                    WarehouseType = viewModel.WarehouseType!,
                    IsActive = viewModel.IsActive,
                    IsMainWarehouse = viewModel.IsMainWarehouse,
                    Notes = viewModel.Notes
                };
                await Client.CreateWarehouseEndpointAsync("1", command).ConfigureAwait(false);
                Snackbar.Add("Warehouse created successfully", Severity.Success);
            },
            updateFunc: async (id, viewModel) =>
            {
                var command = new UpdateWarehouseCommand
                {
                    Code = viewModel.Code!,
                    Name = viewModel.Name!,
                    Description = viewModel.Description,
                    Address = viewModel.Address!,
                    ManagerName = viewModel.ManagerName!,
                    ManagerEmail = viewModel.ManagerEmail!,
                    ManagerPhone = viewModel.ManagerPhone!,
                    TotalCapacity = viewModel.TotalCapacity,
                    CapacityUnit = viewModel.CapacityUnit!,
                    WarehouseType = viewModel.WarehouseType!,
                    IsActive = viewModel.IsActive,
                    IsMainWarehouse = viewModel.IsMainWarehouse,
                    Notes = viewModel.Notes
                };
                await Client.UpdateWarehouseEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Warehouse updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteWarehouseEndpointAsync("1", id).ConfigureAwait(false);
                Snackbar.Add("Warehouse deleted successfully", Severity.Success);
            });

        return Task.CompletedTask;
    }

    /// <summary>
    /// Views the full details of a warehouse in a dialog.
    /// </summary>
    private async Task ViewWarehouseDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<WarehouseDetailsDialog>
        {
            { x => x.WarehouseId, id }
        };

        var dialog = await DialogService.ShowAsync<WarehouseDetailsDialog>(
            "Warehouse Details",
            parameters,
            _dialogOptions);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Assigns a manager to the specified warehouse.
    /// </summary>
    private async Task AssignManager(DefaultIdType id)
    {
        var parameters = new DialogParameters<WarehouseAssignManagerDialog>
        {
            { x => x.WarehouseId, id }
        };

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<WarehouseAssignManagerDialog>(
            "Assign Manager",
            parameters,
            options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }
}

