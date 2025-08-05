using Accounting.Domain;
using Accounting.Application.Currencies.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Currencies.Delete;

public sealed class DeleteCurrencyHandler(
    [FromKeyedServices("accounting")] IRepository<Currency> repository)
    : IRequestHandler<DeleteCurrencyRequest>
{
    public async Task Handle(DeleteCurrencyRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currency = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (currency == null) throw new CurrencyNotFoundException(request.Id);

        await repository.DeleteAsync(currency, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
