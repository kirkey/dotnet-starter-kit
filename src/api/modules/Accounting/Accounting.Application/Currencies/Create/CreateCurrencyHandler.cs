using Accounting.Domain;
using Accounting.Application.Currencies.Exceptions;
using Accounting.Application.Currencies.Queries;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Currencies.Create;

public sealed class CreateCurrencyHandler(
    [FromKeyedServices("accounting:currencies")] IRepository<Currency> repository)
    : IRequestHandler<CreateCurrencyRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateCurrencyRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var existingByCode = await repository.FirstOrDefaultAsync(
            new CurrencyByCodeSpec(request.CurrencyCode), cancellationToken);
        if (existingByCode != null)
        {
            throw new CurrencyCodeAlreadyExistsException(request.CurrencyCode);
        }

        var existingByName = await repository.FirstOrDefaultAsync(
            new CurrencyByNameSpec(request.Name), cancellationToken);
        if (existingByName != null)
        {
            throw new CurrencyNameAlreadyExistsException(request.Name);
        }

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
