using Accounting.Domain;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Vendors.Get.v1;
public sealed class VendorGetHandler(
    [FromKeyedServices("accounting:vendors")] IReadRepository<Vendor> repository,
    ICacheService cache)
    : IRequestHandler<VendorGetQuery, VendorGetResponse>
{
    public async Task<VendorGetResponse> Handle(VendorGetQuery request, CancellationToken cancellationToken)
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
