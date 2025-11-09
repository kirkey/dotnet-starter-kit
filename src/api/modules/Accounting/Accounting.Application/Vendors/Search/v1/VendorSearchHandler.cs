namespace Accounting.Application.Vendors.Search.v1;

/// <summary>
/// Handler for searching vendors.
/// </summary>
public sealed class VendorSearchHandler(
    [FromKeyedServices("accounting:vendors")] IReadRepository<Vendor> repository)
    : IRequestHandler<VendorSearchRequest, PagedList<VendorSearchResponse>>
{
    public async Task<PagedList<VendorSearchResponse>> Handle(VendorSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var spec = new VendorSearchSpecs(request);
        var vendors = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);
        
        return new PagedList<VendorSearchResponse>(vendors, request.PageNumber, request.PageSize, totalCount);
    }
}
