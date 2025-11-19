namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Specifications;

/// <summary>
/// Specification for getting a payroll line by ID.
/// </summary>
public class PayrollLineByIdSpec : Specification<PayrollLine>, ISingleResultSpecification<PayrollLine>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PayrollLineByIdSpec"/> class.
    /// </summary>
    public PayrollLineByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Payroll)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for searching payroll lines with filters.
/// </summary>
public class SearchPayrollLinesSpec : Specification<PayrollLine>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPayrollLinesSpec"/> class.
    /// </summary>
    public SearchPayrollLinesSpec(Search.v1.SearchPayrollLinesRequest request)
    {
        Query
            .Include(x => x.Payroll)
            .Include(x => x.Employee)
            .OrderBy(x => x.EmployeeId);

        if (request.PayrollId.HasValue)
            Query.Where(x => x.PayrollId == request.PayrollId);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.MinNetPay.HasValue)
            Query.Where(x => x.NetPay >= request.MinNetPay);

        if (request.MaxNetPay.HasValue)
            Query.Where(x => x.NetPay <= request.MaxNetPay);
    }
}

