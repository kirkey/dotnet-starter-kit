namespace Accounting.Application.AccountingPeriods.Specs;

public sealed class AccountingPeriodByFiscalYearTypeSpec : Specification<AccountingPeriod>, ISingleResultSpecification<AccountingPeriod>
{
    public AccountingPeriodByFiscalYearTypeSpec(int fiscalYear, string periodType, DefaultIdType? excludeId = null)
    {
        Query.Where(p => p.FiscalYear == fiscalYear && p.PeriodType == periodType);
        if (excludeId != null)
            Query.Where(p => p.Id != excludeId.Value);
    }
}
