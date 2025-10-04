namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class StockLevels
{
    [Inject] protected IClient Client { get; set; } = default!;

    private EntityServerTableContext<StockLevelResponse, DefaultIdType, StockLevelViewModel> Context { get; set; } = default!;
    private EntityTable<StockLevelResponse, DefaultIdType, StockLevelViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<StockLevelResponse, DefaultIdType, StockLevelViewModel>(
            entityName: "Stock Level",
            entityNamePlural: "Stock Levels",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<StockLevelResponse>(x => x.ItemId, "Item", "ItemId"),
                new EntityField<StockLevelResponse>(x => x.WarehouseId, "Warehouse", "WarehouseId"),
                new EntityField<StockLevelResponse>(x => x.QuantityOnHand, "On Hand", "QuantityOnHand", typeof(int)),
                new EntityField<StockLevelResponse>(x => x.QuantityAvailable, "Available", "QuantityAvailable", typeof(int)),
                new EntityField<StockLevelResponse>(x => x.QuantityReserved, "Reserved", "QuantityReserved", typeof(int)),
                new EntityField<StockLevelResponse>(x => x.QuantityAllocated, "Allocated", "QuantityAllocated", typeof(int)),
                new EntityField<StockLevelResponse>(x => x.LastCountDate, "Last Count", "LastCountDate", typeof(DateTime?)),
                new EntityField<StockLevelResponse>(x => x.LastMovementDate, "Last Movement", "LastMovementDate", typeof(DateTime?))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchStockLevelsCommand>();
                var result = await Client.SearchStockLevelsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<StockLevelResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateStockLevelEndpointAsync("1", viewModel.Adapt<CreateStockLevelCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateStockLevelEndpointAsync("1", id, viewModel.Adapt<UpdateStockLevelCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteStockLevelEndpointAsync("1", id).ConfigureAwait(false),
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetStockLevelEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<StockLevelViewModel>();
            });
}

public class StockLevelViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DefaultIdType? SerialNumberId { get; set; }
    public int QuantityOnHand { get; set; }
    public int QuantityAvailable { get; set; }
    public int QuantityReserved { get; set; }
    public int QuantityAllocated { get; set; }
    public DateTime? LastCountDate { get; set; }
    public DateTime? LastMovementDate { get; set; }
}
