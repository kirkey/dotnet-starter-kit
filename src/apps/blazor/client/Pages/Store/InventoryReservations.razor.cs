namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class InventoryReservations
{
    [Inject] protected IClient Client { get; set; } = default!;

    private EntityServerTableContext<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel> Context { get; set; } = default!;
    private EntityTable<InventoryReservationResponse, DefaultIdType, InventoryReservationViewModel> _table = default!;

    protected override void OnInitialized() =>
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
}

public class InventoryReservationViewModel
{
    public DefaultIdType Id { get; set; }
    public string? ReservationNumber { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public int QuantityReserved { get; set; }
    public string? ReservationType { get; set; }
    public string? Status { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime? ReservationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? ReservedBy { get; set; }
}
