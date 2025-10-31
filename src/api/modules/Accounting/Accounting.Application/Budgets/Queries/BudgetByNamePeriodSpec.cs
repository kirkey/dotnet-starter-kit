namespace Accounting.Application.Budgets.Queries;

public sealed class BudgetByNamePeriodSpec : Specification<Budget>, ISingleResultSpecification<Budget>
{
    public BudgetByNamePeriodSpec(string name, DefaultIdType periodId, DefaultIdType? excludeId = null)
    {
        var n = name?.Trim() ?? string.Empty;
        Query.Where(b => b.Name == n && b.PeriodId == periodId);
        if (excludeId != null)
            Query.Where(b => b.Id != excludeId.Value);
    }
}

