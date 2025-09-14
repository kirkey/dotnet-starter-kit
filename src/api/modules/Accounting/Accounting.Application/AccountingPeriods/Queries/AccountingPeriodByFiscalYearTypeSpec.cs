namespace Accounting.Application.AccountingPeriods.Queries;

public sealed class AccountingPeriodByFiscalYearTypeSpec : Ardalis.Specification.Specification<Accounting.Domain.AccountingPeriod>, Ardalis.Specification.ISingleResultSpecification<Accounting.Domain.AccountingPeriod>
{
    public AccountingPeriodByFiscalYearTypeSpec(int fiscalYear, string periodType, DefaultIdType? excludeId = null)
    {
        Query.Where(p => p.FiscalYear == fiscalYear && p.PeriodType == periodType);
        if (excludeId != null)
            Query.Where(p => p.Id != excludeId.Value);
    }
}
