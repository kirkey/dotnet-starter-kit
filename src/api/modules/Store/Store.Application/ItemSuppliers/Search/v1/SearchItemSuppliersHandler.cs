namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Search.v1;

/// <summary>
/// Handler for searching item-supplier relationships.
/// </summary>
public sealed class SearchItemSuppliersHandler(
    [FromKeyedServices("store:itemsuppliers")] IReadRepository<ItemSupplier> repository)
    : IRequestHandler<SearchItemSuppliersCommand, PagedList<ItemSupplierResponse>>
{
    public async Task<PagedList<ItemSupplierResponse>> Handle(SearchItemSuppliersCommand request, CancellationToken cancellationToken)
    {
        var spec = new Specs.SearchItemSuppliersSpec(request);

        var itemSuppliers = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var itemSupplierResponses = itemSuppliers.Select(itemSupplier => new ItemSupplierResponse
        {
            Id = itemSupplier.Id,
            ItemId = itemSupplier.ItemId,
            SupplierId = itemSupplier.SupplierId,
            IsPreferred = itemSupplier.IsPreferred,
            LeadTimeDays = itemSupplier.LeadTimeDays,
            MinimumOrderQuantity = itemSupplier.MinimumOrderQuantity,
            UnitCost = itemSupplier.UnitCost,
            Currency = itemSupplier.Currency,
            LastPriceUpdate = itemSupplier.LastPriceUpdate,
            IsActive = itemSupplier.IsActive
        }).ToList();

        return new PagedList<ItemSupplierResponse>(itemSupplierResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
