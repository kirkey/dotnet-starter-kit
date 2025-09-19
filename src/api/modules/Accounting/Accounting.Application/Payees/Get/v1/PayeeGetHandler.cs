namespace Accounting.Application.Payees.Get.v1;
public sealed class PayeeGetHandler(
    [FromKeyedServices("accounting:payees")] IReadRepository<Payee> repository,
    ICacheService cache)
    : IRequestHandler<PayeeGetQuery, PayeeResponse>
{
    public async Task<PayeeResponse> Handle(PayeeGetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"payee:{request.Id}",
            async () =>
            {
                var spec = new PayeeGetSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ?? 
                               throw new PayeeNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
