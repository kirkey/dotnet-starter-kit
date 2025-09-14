using Ardalis.Specification;

namespace Accounting.Application.Accruals.Queries;

public sealed class AccrualByNumberSpec : Specification<Accounting.Domain.Accrual>, ISingleResultSpecification<Accounting.Domain.Accrual>
{
    public AccrualByNumberSpec(string accrualNumber, DefaultIdType? excludeId = null)
    {
        var num = accrualNumber?.Trim() ?? string.Empty;
        Query.Where(a => a.AccrualNumber == num);
        if (excludeId != null)
            Query.Where(a => a.Id != excludeId.Value);
    }
}

