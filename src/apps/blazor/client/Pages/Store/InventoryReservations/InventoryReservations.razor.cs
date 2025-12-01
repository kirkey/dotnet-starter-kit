namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryReservations;

/// <summary>
/// Inventory Reservations page logic. Provides CRUD and search over InventoryReservation entities using the generated API client.
/// Supports viewing reservation details and releasing active reservations to return inventory to available stock.
/// </summary>
public partial class InventoryReservations
{
    

    private EntityServerTableContext<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel> Context { get; set; } = null!;
    private EntityTable<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel> _table = null!;

    private ClientPreference _preference = new();

    private List<ItemResponse> _items = [];
    private List<WarehouseResponse> _warehouses = [];

    // Search filter properties - bound to UI controls in the razor file
    private DefaultIdType? SearchItemId { get; set; }
    private DefaultIdType? SearchWarehouseId { get; set; }
    private string? SearchStatus { get; set; }
    private string? SearchReservationType { get; set; }
    private DateTime? SearchReservationDateFrom { get; set; }
    private DateTime? SearchReservationDateTo { get; set; }
    private bool? SearchIsExpired { get; set; }
    private bool? SearchIsActive { get; set; }

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

        SetupContext();
    }

    private void SetupContext()
    {
        Context = new EntityServerTableContext<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel>(
            entityName: "Inventory Reservation",
            entityNamePlural: "Inventory Reservations",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<InventoryReservationResponse>(x => x.ReservationNumber, "Reservation #", "ReservationNumber"),
                new EntityField<InventoryReservationResponse>(x => x.ItemName, "Item", "ItemName"),
                new EntityField<InventoryReservationResponse>(x => x.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<InventoryReservationResponse>(x => x.ReservedQuantity, "Qty Reserved", "ReservedQuantity", typeof(decimal)),
                new EntityField<InventoryReservationResponse>(x => x.ReferenceType, "Type", "ReferenceType"),
                new EntityField<InventoryReservationResponse>(x => x.Status, "Status", "Status"),
                new EntityField<InventoryReservationResponse>(x => x.ReservationDate, "Reserved On", "ReservationDate", typeof(DateOnly)),
                new EntityField<InventoryReservationResponse>(x => x.ExpirationDate, "Expires", "ExpirationDate", typeof(DateOnly?))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var command = new SearchInventoryReservationsCommand
                {
                    ItemId = SearchItemId,
                    WarehouseId = SearchWarehouseId,
                    Status = SearchStatus,
                    ReservationType = SearchReservationType,
                    ReservationDateFrom = SearchReservationDateFrom,
                    ReservationDateTo = SearchReservationDateTo,
                    IsExpired = SearchIsExpired,
                    IsActive = SearchIsActive
                };
                
                var result = await Client.SearchInventoryReservationsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InventoryReservationResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateInventoryReservationEndpointAsync("1", viewModel.Adapt<CreateInventoryReservationCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteInventoryReservationEndpointAsync("1", id).ConfigureAwait(false));

        // Load items and warehouses for search filters
        Task.Run(async () =>
        {
            await LoadItemsAsync();
            await LoadWarehousesAsync();
        });
    }

    /// <summary>
    /// Loads items for the search filter dropdown.
    /// </summary>
    private async Task LoadItemsAsync()
    {
        try
        {
            var command = new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 500,
                OrderBy = ["Name"]
            };
            var result = await Client.SearchItemsEndpointAsync("1", command).ConfigureAwait(false);
            _items = result.Items?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load items: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Loads warehouses for the search filter dropdown.
    /// </summary>
    private async Task LoadWarehousesAsync()
    {
        try
        {
            var command = new SearchWarehousesRequest
            {
                PageNumber = 1,
                PageSize = 100,
                OrderBy = ["Name"]
            };
            var result = await Client.SearchWarehousesEndpointAsync("1", command).ConfigureAwait(false);
            _warehouses = result.Items?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load warehouses: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Views the full details of an inventory reservation in a dialog.
    /// </summary>
    private async Task ViewReservationDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<ReservationDetailsDialog>
        {
            { x => x.ReservationId, id }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium,
        };

        var dialog = await DialogService.ShowAsync<ReservationDetailsDialog>("Reservation Details", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Releases an active reservation, returning the quantity to available stock.
    /// </summary>
    private async Task ReleaseReservation(DefaultIdType id)
    {
        var parameters = new DialogParameters<ReleaseReservationDialog>
        {
            { x => x.ReservationId, id }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
        };

        var dialog = await DialogService.ShowAsync<ReleaseReservationDialog>("Release Reservation", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Checks if a reservation has expired based on its expiration date.
    /// </summary>
    private bool IsExpired(DateTime? expirationDate)
    {
        return expirationDate.HasValue && expirationDate.Value < DateTime.UtcNow;
    }

    /// <summary>
    /// Show inventory reservations help dialog.
    /// </summary>
    private async Task ShowInventoryReservationsHelp()
    {
        await DialogService.ShowAsync<InventoryReservationsHelpDialog>("Inventory Reservations Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel for Inventory Reservation add/edit operations.
/// Inherits from CreateInventoryReservationCommand (no update operation exists for reservations).
/// </summary>
public partial class InventoryReservationViewModel : CreateInventoryReservationCommand
{
    public DefaultIdType Id { get; set; } = DefaultIdType.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? ReservationDate { get; set; }
}

