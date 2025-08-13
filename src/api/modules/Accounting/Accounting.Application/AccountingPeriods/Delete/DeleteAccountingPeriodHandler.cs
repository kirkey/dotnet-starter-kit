using Accounting.Domain;
using Accounting.Application.AccountingPeriods.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.AccountingPeriods.Delete;

public sealed class DeleteAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<DeleteAccountingPeriodRequest>
{
    public async Task Handle(DeleteAccountingPeriodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var period = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (period == null) throw new AccountingPeriodNotFoundException(request.Id);

        await repository.DeleteAsync(period, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
