using Accounting.Domain;
using Accounting.Application.Currencies.Dtos;
using Accounting.Application.Currencies.Exceptions;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Currencies.Get;

public sealed class GetCurrencyHandler(
    [FromKeyedServices("accounting:currencies")] IReadRepository<Currency> repository,
    ICacheService cache)
    : IRequestHandler<GetCurrencyRequest, CurrencyDto>
{
    public async Task<CurrencyDto> Handle(GetCurrencyRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync<CurrencyDto>(
            $"currency:{request.Id}",
            async () =>
            {
                var currency = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (currency == null) throw new CurrencyNotFoundException(request.Id);
                return currency.Adapt<CurrencyDto>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
