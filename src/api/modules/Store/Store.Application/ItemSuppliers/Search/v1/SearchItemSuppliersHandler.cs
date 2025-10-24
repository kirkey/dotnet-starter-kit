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
            Name = itemSupplier.Name,
            Description = itemSupplier.Description,
            Notes = itemSupplier.Notes,
            ItemId = itemSupplier.ItemId,
            ItemName = itemSupplier.Item.Name,
            ItemSku = itemSupplier.Item.Sku,
            SupplierId = itemSupplier.SupplierId,
            SupplierName = itemSupplier.Supplier.Name,
            SupplierCode = itemSupplier.Supplier.Code,
            SupplierPartNumber = itemSupplier.SupplierPartNumber,
            IsPreferred = itemSupplier.IsPreferred,
            LeadTimeDays = itemSupplier.LeadTimeDays,
            MinimumOrderQuantity = itemSupplier.MinimumOrderQuantity,
            UnitCost = itemSupplier.UnitCost,
            LastPriceUpdate = itemSupplier.LastPriceUpdate,
            IsActive = itemSupplier.IsActive
        }).ToList();

        return new PagedList<ItemSupplierResponse>(itemSupplierResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
