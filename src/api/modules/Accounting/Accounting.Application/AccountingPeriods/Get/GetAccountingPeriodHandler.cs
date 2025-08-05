using Accounting.Domain;
using Accounting.Application.AccountingPeriods.Dtos;
using Accounting.Application.AccountingPeriods.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.AccountingPeriods.Get;

public sealed class GetAccountingPeriodHandler(
    [FromKeyedServices("accounting")] IReadRepository<AccountingPeriod> repository)
    : IRequestHandler<GetAccountingPeriodRequest, AccountingPeriodDto>
{
    public async Task<AccountingPeriodDto> Handle(GetAccountingPeriodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var period = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (period == null) throw new AccountingPeriodNotFoundException(request.Id);

        return new AccountingPeriodDto(
            period.Id,
            period.Name!,
            period.StartDate,
            period.EndDate,
            period.IsClosed,
            period.IsAdjustmentPeriod,
            period.FiscalYear,
            period.PeriodType,
            period.Description,
            period.Notes);
    }
}
