using Accounting.Domain.Entities;

namespace Accounting.Application.AccountingPeriods.Specs;

/// <summary>
/// Specification that finds any accounting period that overlaps a given date range.
/// </summary>
/// <remarks>
/// Used to detect overlapping periods when creating or updating accounting periods.
/// The overlap condition is: existing.StartDate <= newEndDate && existing.EndDate >= newStartDate.
/// </remarks>
public sealed class AccountingPeriodOverlappingSpec : Specification<AccountingPeriod>
{
    public AccountingPeriodOverlappingSpec(DateTime startDate, DateTime endDate, DefaultIdType? excludeId = null)
    {
        // overlap condition: period.StartDate <= endDate && period.EndDate >= startDate
        Query.Where(p => p.StartDate <= endDate && p.EndDate >= startDate);

        if (excludeId != null)
            Query.Where(p => p.Id != excludeId.Value);
    }
}
