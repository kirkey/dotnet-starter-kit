using FSH.Starter.WebApi.Store.Application.Suppliers.Specs;
using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

/// <summary>
/// Handles retrieval of a single Supplier by identifier.
/// </summary>
public sealed class GetSupplierHandler(
    [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> repository,
    ICacheService cache)
    : IRequestHandler<GetSupplierCommand, SupplierResponse>
{
    public async Task<SupplierResponse> Handle(GetSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"supplier:{request.Id}",
            async () =>
            {
                var spec = new GetSupplierSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
                              ?? throw new SupplierNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
