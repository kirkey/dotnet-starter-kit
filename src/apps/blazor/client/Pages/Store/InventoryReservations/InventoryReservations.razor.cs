namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryReservations;

/// <summary>
/// Inventory Reservations page logic. Provides CRUD and search over InventoryReservation entities using the generated API client.
/// Supports viewing reservation details and releasing active reservations to return inventory to available stock.
/// </summary>
public partial class InventoryReservations
{
    private EntityServerTableContext<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel> Context { get; set; } = default!;
    private EntityTable<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel> _table = default!;

    private List<ItemResponse> _items = new();
    private List<WarehouseResponse> _warehouses = new();

    // Search filter properties - bound to UI controls in the razor file
    private DefaultIdType? SearchItemId { get; set; }
    private DefaultIdType? SearchWarehouseId { get; set; }
    private string? SearchStatus { get; set; }
    private string? SearchReservationType { get; set; }
    private DateTime? SearchReservationDateFrom { get; set; }
    private DateTime? SearchReservationDateTo { get; set; }
    private bool? SearchIsExpired { get; set; }
    private bool? SearchIsActive { get; set; }

    protected override void OnInitialized()
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
                new EntityField<InventoryReservationResponse>(x => x.ReservationDate, "Reserved On", "ReservationDate", typeof(DateTime)),
                new EntityField<InventoryReservationResponse>(x => x.ExpirationDate, "Expires", "ExpirationDate", typeof(DateTime?))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var command = new SearchInventoryReservationsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy ?? ["ReservationNumber"],
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
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadItemsAsync();
        await LoadWarehousesAsync();
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
            _items = result.Items?.ToList() ?? new List<ItemResponse>();
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
            var command = new SearchWarehousesCommand
            {
                PageNumber = 1,
                PageSize = 500,
                OrderBy = ["Name"]
            };
            var result = await Client.SearchWarehousesEndpointAsync("1", command).ConfigureAwait(false);
            _warehouses = result.Items?.ToList() ?? new List<WarehouseResponse>();
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

