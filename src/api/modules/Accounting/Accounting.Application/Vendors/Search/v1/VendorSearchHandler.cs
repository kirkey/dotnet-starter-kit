namespace Accounting.Application.Vendors.Search.v1;
public sealed class VendorSearchHandler(
    [FromKeyedServices("accounting:vendors")] IReadRepository<Vendor> repository)
    : IRequestHandler<VendorSearchQuery, PagedList<VendorSearchResponse>>
{
    public async Task<PagedList<VendorSearchResponse>> Handle(VendorSearchQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var spec = new VendorSearchSpecs(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);
        return new PagedList<VendorSearchResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
