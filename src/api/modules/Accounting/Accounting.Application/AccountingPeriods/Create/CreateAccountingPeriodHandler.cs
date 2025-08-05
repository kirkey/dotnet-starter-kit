using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.AccountingPeriods.Create;

public sealed class CreateAccountingPeriodHandler(
    [FromKeyedServices("accounting")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<CreateAccountingPeriodRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateAccountingPeriodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var period = AccountingPeriod.Create(
            request.Name,
            request.StartDate,
            request.EndDate,
            request.FiscalYear,
            request.PeriodType,
            request.IsAdjustmentPeriod,
            request.Description,
            request.Notes);

        await repository.AddAsync(period, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return period.Id;
    }
}
