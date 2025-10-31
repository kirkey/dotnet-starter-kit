namespace Accounting.Application.RetainedEarnings.Queries;

/// <summary>
/// Specification to find retained earnings by fiscal year.
/// </summary>
public class RetainedEarningsByFiscalYearSpec : Specification<Accounting.Domain.Entities.RetainedEarnings>
{
    public RetainedEarningsByFiscalYearSpec(int fiscalYear)
    {
        Query.Where(re => re.FiscalYear == fiscalYear);
    }
}

/// <summary>
/// Specification to find retained earnings by ID.
/// </summary>
public class RetainedEarningsByIdSpec : Specification<Accounting.Domain.Entities.RetainedEarnings>
{
    public RetainedEarningsByIdSpec(DefaultIdType id)
    {
        Query.Where(re => re.Id == id);
    }
}

/// <summary>
/// Specification for searching retained earnings with filters.
/// </summary>
public class RetainedEarningsSearchSpec : Specification<Accounting.Domain.Entities.RetainedEarnings>
{
    public RetainedEarningsSearchSpec(
        int? fiscalYear = null,
        string? status = null,
        bool? isClosed = null,
        int? fromYear = null,
        int? toYear = null)
    {
        if (fiscalYear.HasValue)
        {
            Query.Where(re => re.FiscalYear == fiscalYear.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Query.Where(re => re.Status == status);
        }

        if (isClosed.HasValue)
        {
            Query.Where(re => re.IsClosed == isClosed.Value);
        }

        if (fromYear.HasValue)
        {
            Query.Where(re => re.FiscalYear >= fromYear.Value);
        }

        if (toYear.HasValue)
        {
            Query.Where(re => re.FiscalYear <= toYear.Value);
        }

        Query.OrderByDescending(re => re.FiscalYear);
    }
}

