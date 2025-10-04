namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class InventoryReservations
{
    

    private EntityServerTableContext<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel> Context { get; set; } = default!;
    private EntityTable<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
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
                new EntityField<InventoryReservationResponse>(x => x.ReservedQuantity, "Qty Reserved", "ReservedQuantity", typeof(double)),
                new EntityField<InventoryReservationResponse>(x => x.ReferenceType, "Type", "ReferenceType"),
                new EntityField<InventoryReservationResponse>(x => x.Status, "Status", "Status"),
                new EntityField<InventoryReservationResponse>(x => x.ReservationDate, "Reserved On", "ReservationDate", typeof(DateTime?)),
                new EntityField<InventoryReservationResponse>(x => x.ExpirationDate, "Expires", "ExpirationDate", typeof(DateTime?))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchInventoryReservationsCommand>();
                var result = await Client.SearchInventoryReservationsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InventoryReservationResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateInventoryReservationEndpointAsync("1", viewModel.Adapt<CreateInventoryReservationCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteInventoryReservationEndpointAsync("1", id).ConfigureAwait(false),
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetInventoryReservationEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<InventoryReservationViewModel>();
            });
        
        await Task.CompletedTask;
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
