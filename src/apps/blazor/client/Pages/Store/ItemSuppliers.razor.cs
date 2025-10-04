namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class ItemSuppliers
{
    [Inject] protected IClient Client { get; set; } = default!;

    private EntityServerTableContext<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel> Context { get; set; } = default!;
    private EntityTable<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel>(
            entityName: "Item Supplier",
            entityNamePlural: "Item Suppliers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<ItemSupplierResponse>(x => x.ItemId, "Item", "ItemId"),
                new EntityField<ItemSupplierResponse>(x => x.SupplierId, "Supplier", "SupplierId"),
                new EntityField<ItemSupplierResponse>(x => x.UnitCost, "Unit Cost", "UnitCost", typeof(double)),
                new EntityField<ItemSupplierResponse>(x => x.LeadTimeDays, "Lead Time", "LeadTimeDays", typeof(int)),
                new EntityField<ItemSupplierResponse>(x => x.IsPreferred, "Preferred", "IsPreferred", typeof(bool)),
                new EntityField<ItemSupplierResponse>(x => x.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<ItemSupplierResponse>(x => x.ReliabilityRating, "Rating", "ReliabilityRating", typeof(double))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchItemSuppliersCommand>();
                var result = await Client.SearchItemSuppliersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ItemSupplierResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateItemSupplierEndpointAsync("1", viewModel.Adapt<CreateItemSupplierCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateItemSupplierEndpointAsync("1", id, viewModel.Adapt<UpdateItemSupplierCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteItemSupplierEndpointAsync("1", id).ConfigureAwait(false),
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetItemSupplierEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<ItemSupplierViewModel>();
            });
}

public class ItemSupplierViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType? SupplierId { get; set; }
    public decimal UnitCost { get; set; }
    public string? CurrencyCode { get; set; } = "USD";
    public int LeadTimeDays { get; set; }
    public int MinimumOrderQuantity { get; set; } = 1;
    public int PackagingQuantity { get; set; } = 1;
    public string? SupplierPartNumber { get; set; }
    public decimal ReliabilityRating { get; set; }
    public bool IsPreferred { get; set; }
    public bool IsActive { get; set; } = true;
}
