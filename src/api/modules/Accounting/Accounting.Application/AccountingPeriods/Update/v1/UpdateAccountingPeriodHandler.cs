using Accounting.Application.AccountingPeriods.Exceptions;
using Accounting.Application.AccountingPeriods.Queries;
using AccountingPeriodNotFoundException = Accounting.Application.AccountingPeriods.Exceptions.AccountingPeriodNotFoundException;

namespace Accounting.Application.AccountingPeriods.Update.v1;

public sealed class UpdateAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<UpdateAccountingPeriodRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateAccountingPeriodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var period = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (period == null) throw new AccountingPeriodNotFoundException(request.Id);

        var name = request.Name?.Trim();
        var periodType = request.PeriodType?.Trim();

        // Check duplicate name (exclude current)
        if (!string.IsNullOrWhiteSpace(name))
        {
            var existingByName = await repository.FirstOrDefaultAsync(
                new AccountingPeriodByNameSpec(name), cancellationToken);
            if (existingByName != null && existingByName.Id != request.Id)
                throw new AccountingPeriodNameAlreadyExistsException(name);
        }

        // Check fiscal year + type conflict (exclude current)
        if (request.FiscalYear.HasValue && !string.IsNullOrWhiteSpace(periodType))
        {
            var existingByFyType = await repository.FirstOrDefaultAsync(
                new AccountingPeriodByFiscalYearTypeSpec(request.FiscalYear.Value, periodType, request.Id), cancellationToken);
            if (existingByFyType != null && existingByFyType.Id != request.Id)
                throw new AccountingPeriodAlreadyExistsException(request.FiscalYear.Value, periodType);
        }

        // Check overlapping periods (exclude current)
        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            var overlapping = await repository.FirstOrDefaultAsync(
                new AccountingPeriodOverlappingSpec(request.StartDate.Value, request.EndDate.Value, request.Id), cancellationToken);
            if (overlapping != null && overlapping.Id != request.Id)
                throw new AccountingPeriodOverlappingException(request.StartDate.Value, request.EndDate.Value);
        }

        period.Update(name, request.StartDate, request.EndDate, request.FiscalYear,
            periodType, request.IsAdjustmentPeriod,
            request.Description, request.Notes);

        await repository.UpdateAsync(period, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return period.Id;
    }
}
