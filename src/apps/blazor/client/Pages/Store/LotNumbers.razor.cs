namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class LotNumbers
{
    [Inject] protected IClient Client { get; set; } = default!;

    private EntityServerTableContext<LotNumberResponse, DefaultIdType, LotNumberViewModel> Context { get; set; } = default!;
    private EntityTable<LotNumberResponse, DefaultIdType, LotNumberViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<LotNumberResponse, DefaultIdType, LotNumberViewModel>(
            entityName: "Lot Number",
            entityNamePlural: "Lot Numbers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<LotNumberResponse>(x => x.LotCode, "Lot Code", "LotCode"),
                new EntityField<LotNumberResponse>(x => x.ItemId, "Item", "ItemId"),
                new EntityField<LotNumberResponse>(x => x.QuantityReceived, "Qty Received", "QuantityReceived", typeof(int)),
                new EntityField<LotNumberResponse>(x => x.QuantityRemaining, "Qty Remaining", "QuantityRemaining", typeof(int)),
                new EntityField<LotNumberResponse>(x => x.ManufactureDate, "Mfg Date", "ManufactureDate", typeof(DateTime?)),
                new EntityField<LotNumberResponse>(x => x.ExpirationDate, "Exp Date", "ExpirationDate", typeof(DateTime?)),
                new EntityField<LotNumberResponse>(x => x.Status, "Status", "Status")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchLotNumbersCommand>();
                var result = await Client.SearchLotNumbersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LotNumberResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateLotNumberEndpointAsync("1", viewModel.Adapt<CreateLotNumberCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateLotNumberEndpointAsync("1", id, viewModel.Adapt<UpdateLotNumberCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteLotNumberEndpointAsync("1", id).ConfigureAwait(false),
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetLotNumberEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<LotNumberViewModel>();
            });
}

public class LotNumberViewModel
{
    public DefaultIdType Id { get; set; }
    public string? LotCode { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType? SupplierId { get; set; }
    public int QuantityReceived { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? ReceiptDate { get; set; }
    public string? Status { get; set; }
    public string? QualityNotes { get; set; }
}
