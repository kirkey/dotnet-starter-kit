namespace FSH.Starter.Blazor.Client.Pages.Store.StockLevels;

public partial class StockLevels
{
    private EntityServerTableContext<StockLevelResponse, DefaultIdType, StockLevelViewModel> Context { get; set; } = null!;
    private EntityTable<StockLevelResponse, DefaultIdType, StockLevelViewModel> _table = null!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<StockLevelResponse, DefaultIdType, StockLevelViewModel>(
            entityName: "Stock Level",
            entityNamePlural: "Stock Levels",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<StockLevelResponse>(x => x.ItemName, "Item", "ItemName"),
                new EntityField<StockLevelResponse>(x => x.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<StockLevelResponse>(x => x.QuantityOnHand, "On Hand", "QuantityOnHand", typeof(int)),
                new EntityField<StockLevelResponse>(x => x.QuantityAvailable, "Available", "QuantityAvailable", typeof(int)),
                new EntityField<StockLevelResponse>(x => x.QuantityReserved, "Reserved", "QuantityReserved", typeof(int)),
                new EntityField<StockLevelResponse>(x => x.QuantityAllocated, "Allocated", "QuantityAllocated", typeof(int)),
                new EntityField<StockLevelResponse>(x => x.LastCountDate, "Last Count", "LastCountDate", typeof(DateTime?)),
                new EntityField<StockLevelResponse>(x => x.LastMovementDate, "Last Movement", "LastMovementDate", typeof(DateTime?))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetStockLevelEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<StockLevelViewModel>();
            // }
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
            deleteFunc: async id => await Client.DeleteStockLevelEndpointAsync("1", id).ConfigureAwait(false));
        
        await Task.CompletedTask;
    }

    /// <summary>
    /// Show stock levels help dialog.
    /// </summary>
    private async Task ShowStockLevelsHelp()
    {
        await DialogService.ShowAsync<StockLevelsHelpDialog>("Stock Levels Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

public class StockLevelViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public DefaultIdType WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
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
