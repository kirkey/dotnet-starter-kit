namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class ItemSuppliers
{


    private EntityServerTableContext<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel> Context { get; set; } =
        default!;

    private EntityTable<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel>(
            entityName: "Item Supplier",
            entityNamePlural: "Item Suppliers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<ItemSupplierResponse>(x => x.ItemId, "Item", "ItemId"),
                new EntityField<ItemSupplierResponse>(x => x.SupplierId, "Supplier", "SupplierId"),
                new EntityField<ItemSupplierResponse>(x => x.UnitCost, "Unit Cost", "UnitCost", typeof(decimal)),
                new EntityField<ItemSupplierResponse>(x => x.LeadTimeDays, "Lead Time", "LeadTimeDays", typeof(int)),
                new EntityField<ItemSupplierResponse>(x => x.IsPreferred, "Preferred", "IsPreferred", typeof(bool)),
                new EntityField<ItemSupplierResponse>(x => x.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<ItemSupplierResponse>(x => x.ReliabilityRating, "Rating", "ReliabilityRating",
                    typeof(decimal))
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
                await Client.CreateItemSupplierEndpointAsync("1", viewModel.Adapt<CreateItemSupplierCommand>())
                    .ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateItemSupplierEndpointAsync("1", id, viewModel.Adapt<UpdateItemSupplierCommand>())
                    .ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteItemSupplierEndpointAsync("1", id).ConfigureAwait(false),
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetItemSupplierEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<ItemSupplierViewModel>();
            });
    }
}

/// <summary>
/// ViewModel for Item Supplier add/edit operations.
/// Inherits from UpdateItemSupplierCommand to ensure proper mapping with the API.
/// </summary>
public partial class ItemSupplierViewModel : UpdateItemSupplierCommand;
