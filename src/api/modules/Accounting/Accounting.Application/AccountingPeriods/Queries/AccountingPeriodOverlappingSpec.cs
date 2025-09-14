namespace Accounting.Application.AccountingPeriods.Queries;

public sealed class AccountingPeriodOverlappingSpec : Ardalis.Specification.Specification<Accounting.Domain.AccountingPeriod>
{
    public AccountingPeriodOverlappingSpec(DateTime startDate, DateTime endDate, DefaultIdType? excludeId = null)
    {
        // overlap condition: period.StartDate <= endDate && period.EndDate >= startDate
        Query.Where(p => p.StartDate <= endDate && p.EndDate >= startDate);

        if (excludeId != null)
            Query.Where(p => p.Id != excludeId.Value);
    }
}
