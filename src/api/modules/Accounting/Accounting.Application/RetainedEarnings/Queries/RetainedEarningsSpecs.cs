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
public class RetainedEarningsSearchSpec : EntitiesByPaginationFilterSpec<Accounting.Domain.Entities.RetainedEarnings, Responses.RetainedEarningsResponse>
{
    public RetainedEarningsSearchSpec(Search.v1.SearchRetainedEarningsRequest request)
        : base(request)
    {
        if (request.FiscalYear.HasValue)
        {
            Query.Where(re => re.FiscalYear == request.FiscalYear.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(re => re.Status == request.Status);
        }

        var isClosed = request.OnlyOpen ? false : request.IsClosed;
        if (isClosed.HasValue)
        {
            Query.Where(re => re.IsClosed == isClosed.Value);
        }


        Query.OrderByDescending(re => re.FiscalYear);
    }
}

