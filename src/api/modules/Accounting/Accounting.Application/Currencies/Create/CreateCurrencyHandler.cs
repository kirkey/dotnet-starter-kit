using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Currencies.Create;

public sealed class CreateCurrencyHandler(
    [FromKeyedServices("accounting")] IRepository<Currency> repository)
    : IRequestHandler<CreateCurrencyRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateCurrencyRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currency = Currency.Create(
            request.CurrencyCode,
            request.Name,
            request.Symbol,
            request.DecimalPlaces,
            request.Description,
            request.Notes);

        await repository.AddAsync(currency, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return currency.Id;
    }
}
