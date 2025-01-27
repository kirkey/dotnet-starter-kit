﻿using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Pages.Catalog;

public partial class Brands
{
    [Inject]
    protected IApiClient _client { get; set; } = default!;

    protected EntityServerTableContext<BrandResponse, Guid, BrandViewModel> Context { get; set; } = default!;

    private EntityTable<BrandResponse, Guid, BrandViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<BrandResponse, Guid, BrandViewModel>(
            entityName: "Brand",
            entityNamePlural: "Brands",
            entityResource: FshResources.Brands,
            fields: new List<EntityField<BrandResponse>>
            {
                new(brand => brand.Id, "Id", "Id"),
                new(brand => brand.Name, "Name", "Name"),
                new(brand => brand.Description, "Description", "Description")
            },
            enableAdvancedSearch: true,
            idFunc: brand => brand.Id!.Value,
            searchFunc: async filter =>
            {
                var brandFilter = filter.Adapt<SearchBrandsCommand>();
                var result = await _client.SearchBrandsEndpointAsync("1", brandFilter).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<BrandResponse>>();
            },
            createFunc: async brand =>
            {
                await _client.CreateBrandEndpointAsync("1", brand.Adapt<CreateBrandCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, brand) =>
            {
                await _client.UpdateBrandEndpointAsync("1", id, brand.Adapt<UpdateBrandCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await _client.DeleteBrandEndpointAsync("1", id).ConfigureAwait(false));
}

public class BrandViewModel : UpdateBrandCommand
{
}
