namespace FSH.Starter.Blazor.Client.Pages.Warehouse.Locations;

/// <summary>
/// Warehouse Locations page logic. Provides CRUD and server-side search over WarehouseLocation entities via the generated API client.
/// </summary>
public partial class Locations
{
    /// <summary>
    /// The entity table context for managing warehouse locations with server-side operations.
    /// </summary>
    protected EntityServerTableContext<GetWarehouseLocationListResponse, DefaultIdType, WarehouseLocationViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for warehouse locations.
    /// </summary>
    private EntityTable<GetWarehouseLocationListResponse, DefaultIdType, WarehouseLocationViewModel> _table = null!;

    /// <summary>
    /// Search filter for location name.
    /// </summary>
    private string? SearchName { get; set; }

    /// <summary>
    /// Search filter for location code.
    /// </summary>
    private string? SearchCode { get; set; }

    /// <summary>
    /// Search filter for warehouse.
    /// </summary>
    private DefaultIdType? SearchWarehouseId { get; set; }

    /// <summary>
    /// Search filter for location type.
    /// </summary>
    private string? SearchLocationType { get; set; }

    /// <summary>
    /// Search filter for aisle.
    /// </summary>
    private string? SearchAisle { get; set; }

    /// <summary>
    /// Search filter for active status.
    /// </summary>
    private bool? SearchIsActive { get; set; }

    /// <summary>
    /// Search filter for temperature control requirement.
    /// </summary>
    private bool? SearchRequiresTemperatureControl { get; set; }

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
        Context = new EntityServerTableContext<GetWarehouseLocationListResponse, DefaultIdType, WarehouseLocationViewModel>(
            entityName: "Warehouse Location",
            entityNamePlural: "Warehouse Locations",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<GetWarehouseLocationListResponse>(response => response.Code, "Code", "Code"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Name, "Name", "Name"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.LocationType, "Type", "LocationType"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Aisle, "Aisle", "Aisle"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Section, "Section", "Section"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Shelf, "Shelf", "Shelf"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Capacity, "Capacity", "Capacity", typeof(decimal)),
                new EntityField<GetWarehouseLocationListResponse>(response => response.UsedCapacity, "Used", "UsedCapacity", typeof(decimal)),
                new EntityField<GetWarehouseLocationListResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var command = new SearchWarehouseLocationsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    SearchTerm = SearchName ?? SearchCode,
                    WarehouseId = SearchWarehouseId,
                    LocationType = SearchLocationType,
                    Aisle = SearchAisle,
                    IsActive = SearchIsActive,
                    RequiresTemperatureControl = SearchRequiresTemperatureControl
                };
                var result = await Client.SearchWarehouseLocationsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<GetWarehouseLocationListResponse>>();
            },
            createFunc: async viewModel =>
            {
                var command = new CreateWarehouseLocationCommand
                {
                    Name = viewModel.Name!,
                    Description = viewModel.Description,
                    Code = viewModel.Code!,
                    Aisle = viewModel.Aisle!,
                    Section = viewModel.Section!,
                    Shelf = viewModel.Shelf!,
                    Bin = viewModel.Bin,
                    WarehouseId = viewModel.WarehouseId!.Value,
                    LocationType = viewModel.LocationType!,
                    Capacity = viewModel.Capacity,
                    CapacityUnit = viewModel.CapacityUnit!,
                    IsActive = viewModel.IsActive,
                    RequiresTemperatureControl = viewModel.RequiresTemperatureControl,
                    MinTemperature = viewModel.MinTemperature,
                    MaxTemperature = viewModel.MaxTemperature,
                    TemperatureUnit = viewModel.TemperatureUnit
                };
                await Client.CreateWarehouseLocationEndpointAsync("1", command).ConfigureAwait(false);
                Snackbar.Add("Warehouse location created successfully", Severity.Success);
            },
            updateFunc: async (id, viewModel) =>
            {
                var command = new UpdateWarehouseLocationCommand
                {
                    Name = viewModel.Name!,
                    Description = viewModel.Description,
                    Code = viewModel.Code!,
                    Aisle = viewModel.Aisle!,
                    Section = viewModel.Section!,
                    Shelf = viewModel.Shelf!,
                    Bin = viewModel.Bin,
                    WarehouseId = viewModel.WarehouseId!.Value,
                    LocationType = viewModel.LocationType!,
                    Capacity = viewModel.Capacity,
                    CapacityUnit = viewModel.CapacityUnit!,
                    IsActive = viewModel.IsActive,
                    RequiresTemperatureControl = viewModel.RequiresTemperatureControl,
                    MinTemperature = viewModel.MinTemperature,
                    MaxTemperature = viewModel.MaxTemperature,
                    TemperatureUnit = viewModel.TemperatureUnit
                };
                await Client.UpdateWarehouseLocationEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Warehouse location updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteWarehouseLocationEndpointAsync("1", id).ConfigureAwait(false);
                Snackbar.Add("Warehouse location deleted successfully", Severity.Success);
            });

        return Task.CompletedTask;
    }

    /// <summary>
    /// Views the full details of a warehouse location in a dialog.
    /// </summary>
    private async Task ViewLocationDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<LocationDetailsDialog>
        {
            { x => x.LocationId, id }
        };

        var dialog = await DialogService.ShowAsync<LocationDetailsDialog>(
            "Location Details",
            parameters,
            _dialogOptions);

        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }
}

