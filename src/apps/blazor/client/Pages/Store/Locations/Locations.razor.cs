namespace FSH.Starter.Blazor.Client.Pages.Store.Locations;

/// <summary>
/// Warehouse Locations page logic. Provides CRUD and server-side search over WarehouseLocation entities via the generated API client.
/// </summary>
public partial class Locations
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    /// <summary>
    /// The entity table context for managing warehouse locations with server-side operations.
    /// </summary>
    protected EntityServerTableContext<GetWarehouseLocationListResponse, DefaultIdType, WarehouseLocationViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for warehouse locations.
    /// </summary>
    private EntityTable<GetWarehouseLocationListResponse, DefaultIdType, WarehouseLocationViewModel> _table = null!;

    private ClientPreference _preference = new();

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
    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

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
                    Name = viewModel.Name!.ToUpperInvariant(),
                    Description = viewModel.Description,
                    Code = viewModel.Code!.ToUpperInvariant(),
                    Aisle = viewModel.Aisle!.ToUpperInvariant(),
                    Section = viewModel.Section!.ToUpperInvariant(),
                    Shelf = viewModel.Shelf!.ToUpperInvariant(),
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
                    Name = viewModel.Name!.ToUpperInvariant(),
                    Description = viewModel.Description,
                    Code = viewModel.Code!.ToUpperInvariant(),
                    Aisle = viewModel.Aisle!.ToUpperInvariant(),
                    Section = viewModel.Section!.ToUpperInvariant(),
                    Shelf = viewModel.Shelf!.ToUpperInvariant(),
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
    }

    /// <summary>
    /// Show locations help dialog.
    /// </summary>
    private async Task ShowLocationsHelp()
    {
        await DialogService.ShowAsync<LocationsHelpDialog>("Locations Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

