namespace Accounting.Application.Vendors.Get.v1;

/// <summary>
/// Handler for retrieving a vendor by ID.
/// </summary>
public sealed class VendorGetHandler(
    [FromKeyedServices("accounting:vendors")] IReadRepository<Vendor> repository,
    ICacheService cache)
    : IRequestHandler<VendorGetRequest, VendorGetResponse>
{
    public async Task<VendorGetResponse> Handle(VendorGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"vendor:{request.Id}",
            async () =>
            {
                var spec = new VendorGetSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ??
                               throw new Exception($"Vendor with id {request.Id} not found");
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
