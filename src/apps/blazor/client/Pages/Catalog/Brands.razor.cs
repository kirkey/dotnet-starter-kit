using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Pages.Catalog;

public partial class Brands
{
    [Inject]
    protected IApiClient _client { get; set; } = default!;

    protected EntityServerTableContext<BrandResponse, DefaultIdType, BrandViewModel> Context { get; set; } = default!;

    private EntityTable<BrandResponse, DefaultIdType, BrandViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<BrandResponse, DefaultIdType, BrandViewModel>(
            entityName: "Brand",
            entityNamePlural: "Brands",
            entityResource: FshResources.Brands,
            fields:
            [
                new EntityField<BrandResponse>(brand => brand.Id, "Id", "Id"),
                new EntityField<BrandResponse>(brand => brand.Name, "Name", "Name"),
                new EntityField<BrandResponse>(brand => brand.Description, "Description", "Description")
            ],
            enableAdvancedSearch: true,
            idFunc: brand => brand.Id!.Value,
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

public partial class BrandViewModel : UpdateBrandCommand
{
}
