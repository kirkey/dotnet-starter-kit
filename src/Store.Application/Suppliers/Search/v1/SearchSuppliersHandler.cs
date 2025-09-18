using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;

public sealed class SearchSuppliersHandler(
    [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> repository)
    : IRequestHandler<SearchSuppliersCommand, PagedList<SupplierResponse>>
{
    public async Task<PagedList<SupplierResponse>> Handle(SearchSuppliersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSuppliersSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SupplierResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
