using Accounting.Domain;
using Accounting.Application.AccountingPeriods.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.AccountingPeriods.Update;

public sealed class UpdateAccountingPeriodHandler(
    [FromKeyedServices("accounting")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<UpdateAccountingPeriodRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateAccountingPeriodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var period = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (period == null) throw new AccountingPeriodNotFoundException(request.Id);

        period.Update(request.Name, request.StartDate, request.EndDate, request.FiscalYear,
            request.PeriodType, request.IsAdjustmentPeriod,
            request.Description, request.Notes);

        await repository.UpdateAsync(period, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return period.Id;
    }
}
