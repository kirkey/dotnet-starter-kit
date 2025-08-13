using Accounting.Domain;
using Accounting.Application.Currencies.Exceptions;
using Accounting.Application.Currencies.Queries;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Currencies.Update;

public sealed class UpdateCurrencyHandler(
    [FromKeyedServices("accounting:currencies")] IRepository<Currency> repository)
    : IRequestHandler<UpdateCurrencyRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateCurrencyRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currency = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (currency == null) throw new CurrencyNotFoundException(request.Id);

        // Check for duplicate currency code (excluding current currency)
        if (!string.IsNullOrEmpty(request.CurrencyCode) && request.CurrencyCode != currency.CurrencyCode)
        {
            var existingByCode = await repository.FirstOrDefaultAsync(
                new CurrencyByCodeSpec(request.CurrencyCode), cancellationToken);
            if (existingByCode != null)
            {
                throw new CurrencyCodeAlreadyExistsException(request.CurrencyCode);
            }
        }
        
        // Check for duplicate currency name (excluding current currency)
        if (!string.IsNullOrEmpty(request.Name) && request.Name != currency.Name)
        {
            var existingByName = await repository.FirstOrDefaultAsync(
                new CurrencyByNameSpec(request.Name), cancellationToken);
            if (existingByName != null)
            {
                throw new CurrencyNameAlreadyExistsException(request.Name);
            }
        }

        currency.Update(request.CurrencyCode, request.Name, request.Symbol, request.DecimalPlaces, request.IsActive,
            request.Description, request.Notes);

        await repository.UpdateAsync(currency, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return currency.Id;
    }
}
