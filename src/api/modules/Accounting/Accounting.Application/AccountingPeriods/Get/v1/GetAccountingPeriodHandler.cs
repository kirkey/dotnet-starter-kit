using Accounting.Application.AccountingPeriods.Dtos;
using AccountingPeriodNotFoundException = Accounting.Application.AccountingPeriods.Exceptions.AccountingPeriodNotFoundException;

namespace Accounting.Application.AccountingPeriods.Get.v1;

public sealed class GetAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IReadRepository<AccountingPeriod> repository,
    ICacheService cache)
    : IRequestHandler<GetAccountingPeriodRequest, AccountingPeriodDto>
{
    public async Task<AccountingPeriodDto> Handle(GetAccountingPeriodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"period:{request.Id}",
            async () =>
            {
                var period = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
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
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
