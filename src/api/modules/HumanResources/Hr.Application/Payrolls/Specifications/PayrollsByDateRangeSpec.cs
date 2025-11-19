namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Specifications;

/// <summary>
/// Specification for filtering payrolls by date range.
/// </summary>
public sealed class PayrollsByDateRangeSpec : Specification<Payroll>
{
    /// <summary>
    /// Initializes the specification with date range filter.
    /// </summary>
    public PayrollsByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.EndDate >= fromDate && x.StartDate <= toDate)
            .OrderByDescending(x => x.EndDate);
    }
}

