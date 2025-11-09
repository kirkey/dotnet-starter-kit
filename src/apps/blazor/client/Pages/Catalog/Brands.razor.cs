namespace FSH.Starter.Blazor.Client.Pages.Catalog;

public partial class Brands
{
    [Inject]
    protected IClient _client { get; set; } = null!;

    protected EntityServerTableContext<BrandResponse, DefaultIdType, BrandViewModel> Context { get; set; } = null!;

    private EntityTable<BrandResponse, DefaultIdType, BrandViewModel> _table = null!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<BrandResponse, DefaultIdType, BrandViewModel>(
            entityName: "Brand",
            entityNamePlural: "Brands",
            entityResource: FshResources.Brands,
            fields:
            [
                new EntityField<BrandResponse>(response => response.Id, "Id", "Id"),
                new EntityField<BrandResponse>(response => response.Name, "Name", "Name"),
                new EntityField<BrandResponse>(response => response.Description, "Description", "Description")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id!.Value,
            searchFunc: async filter =>
            {
                var brandFilter = filter.Adapt<SearchBrandsCommand>();
                var result = await _client.SearchBrandsEndpointAsync("1", brandFilter);
                return result.Adapt<PaginationResponse<BrandResponse>>();
            },
            createFunc: async brand =>
            {
                await _client.CreateBrandEndpointAsync("1", brand.Adapt<CreateBrandCommand>());
            },
            updateFunc: async (id, brand) =>
            {
                await _client.UpdateBrandEndpointAsync("1", id, brand.Adapt<UpdateBrandCommand>());
            },
            deleteFunc: async id => await _client.DeleteBrandEndpointAsync("1", id));
}

public partial class BrandViewModel : UpdateBrandCommand;
