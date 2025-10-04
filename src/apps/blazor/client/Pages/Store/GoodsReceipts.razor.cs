namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class GoodsReceipts
{
    

    protected EntityServerTableContext<GoodsReceiptResponse, DefaultIdType, GoodsReceiptViewModel> Context { get; set; } = default!;
    private EntityTable<GoodsReceiptResponse, DefaultIdType, GoodsReceiptViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<GoodsReceiptResponse, DefaultIdType, GoodsReceiptViewModel>(
            entityName: "Goods Receipt",
            entityNamePlural: "Goods Receipts",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<GoodsReceiptResponse>(x => x.ReceiptNumber, "Receipt #", "ReceiptNumber"),
                new EntityField<GoodsReceiptResponse>(x => x.ReceivedDate, "Received", "ReceivedDate", typeof(DateTime)),
                new EntityField<GoodsReceiptResponse>(x => x.Status, "Status", "Status"),
                new EntityField<GoodsReceiptResponse>(x => x.ItemCount, "Items", "ItemCount", typeof(int)),
                new EntityField<GoodsReceiptResponse>(x => x.TotalLines, "Total Lines", "TotalLines", typeof(int)),
                new EntityField<GoodsReceiptResponse>(x => x.ReceivedLines, "Received Lines", "ReceivedLines", typeof(int))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetGoodsReceiptEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<GoodsReceiptViewModel>();
            },
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchGoodsReceiptsCommand>();
                var result = await Client.SearchGoodsReceiptsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<GoodsReceiptResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateGoodsReceiptEndpointAsync("1", viewModel.Adapt<CreateGoodsReceiptCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteGoodsReceiptEndpointAsync("1", id).ConfigureAwait(false));
        await Task.CompletedTask;
    }
}

/// <summary>
/// ViewModel for Goods Receipt add/edit operations.
/// Inherits from CreateGoodsReceiptCommand (no update operation exists for goods receipts).
/// </summary>
public partial class GoodsReceiptViewModel : CreateGoodsReceiptCommand
{
    public DateTime? ReceivedDate { get; set; } = default!;
}
