using Accounting.Domain.Entities;

namespace Accounting.Application.AccountingPeriods.Create.v1;

using Exceptions;
using Specs;

/// <summary>
/// Handles <see cref="CreateAccountingPeriodCommand"/> to create a new <see cref="AccountingPeriod"/> aggregate.
/// Performs domain-level checks (duplicate name, duplicate fiscal-year+type, overlapping period) and persists the entity.
/// </summary>
public sealed class CreateAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<CreateAccountingPeriodCommand, DefaultIdType>
{
    /// <summary>
    /// Creates a new accounting period after running all business validations.
    /// </summary>
    /// <param name="request">The create command containing period metadata.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The identifier of the created accounting period.</returns>
    public async Task<DefaultIdType> Handle(CreateAccountingPeriodCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var name = request.Name?.Trim() ?? string.Empty;
        var periodType = request.PeriodType?.Trim() ?? string.Empty;

        // Duplicate name
        var existingByName = await repository.FirstOrDefaultAsync(
            new AccountingPeriodByNameSpec(name), cancellationToken);
        if (existingByName != null)
            throw new AccountingPeriodNameAlreadyExistsException(name);

        // Duplicate fiscal year + period type
        var existingByFyType = await repository.FirstOrDefaultAsync(
            new AccountingPeriodByFiscalYearTypeSpec(request.FiscalYear, periodType), cancellationToken);
        if (existingByFyType != null)
            throw new AccountingPeriodAlreadyExistsException(request.FiscalYear, periodType);

        // Overlapping periods
        var overlapping = await repository.FirstOrDefaultAsync(
            new AccountingPeriodOverlappingSpec(request.StartDate, request.EndDate), cancellationToken);
        if (overlapping != null)
            throw new AccountingPeriodOverlappingException(request.StartDate, request.EndDate);

        var period = AccountingPeriod.Create(
            name,
            request.StartDate,
            request.EndDate,
            request.FiscalYear,
            periodType,
            request.IsAdjustmentPeriod,
            request.Description,
            request.Notes);

        await repository.AddAsync(period, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return period.Id;
    }
}
