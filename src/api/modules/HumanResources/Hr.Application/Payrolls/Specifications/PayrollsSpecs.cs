namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Specifications;

/// <summary>
/// Specification for getting a payroll by ID.
/// </summary>
public class PayrollByIdSpec : Specification<Payroll>, ISingleResultSpecification<Payroll>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PayrollByIdSpec"/> class.
    /// </summary>
    public PayrollByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Lines);
    }
}

/// <summary>
/// Specification for searching payroll with filters.
/// </summary>
public class SearchPayrollsSpec : Specification<Payroll>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPayrollsSpec"/> class.
    /// </summary>
    public SearchPayrollsSpec(Search.v1.SearchPayrollsRequest request)
    {
        Query
            .Include(x => x.Lines)
            .OrderByDescending(x => x.EndDate);

        if (request.StartDate.HasValue)
            Query.Where(x => x.EndDate >= request.StartDate);

        if (request.EndDate.HasValue)
            Query.Where(x => x.StartDate <= request.EndDate);

        if (!string.IsNullOrWhiteSpace(request.PayFrequency))
            Query.Where(x => x.PayFrequency == request.PayFrequency);

        if (!string.IsNullOrWhiteSpace(request.Status))
            Query.Where(x => x.Status == request.Status);
    }
}

