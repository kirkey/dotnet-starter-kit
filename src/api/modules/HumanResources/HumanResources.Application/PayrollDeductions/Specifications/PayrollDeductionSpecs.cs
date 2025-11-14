namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Specifications;

using Ardalis.Specification;
using Search.v1;

/// <summary>
/// Specification for getting payroll deduction by ID.
/// </summary>
public sealed class PayrollDeductionByIdSpec : Specification<PayrollDeduction>, ISingleResultSpecification<PayrollDeduction>
{
    public PayrollDeductionByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.PayComponent)
            .Include(x => x.Employee)
            .Include(x => x.OrganizationalUnit);
    }
}

/// <summary>
/// Specification for searching payroll deductions with filters.
/// </summary>
public sealed class SearchPayrollDeductionsSpec : Specification<PayrollDeduction>
{
    public SearchPayrollDeductionsSpec(SearchPayrollDeductionsRequest request)
    {
        Query
            .Include(x => x.PayComponent)
            .Include(x => x.Employee)
            .Include(x => x.OrganizationalUnit)
            .OrderByDescending(x => x.CreatedOn);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.OrganizationalUnitId.HasValue)
            Query.Where(x => x.OrganizationalUnitId == request.OrganizationalUnitId);

        if (request.PayComponentId.HasValue)
            Query.Where(x => x.PayComponentId == request.PayComponentId);

        if (!string.IsNullOrWhiteSpace(request.DeductionType))
            Query.Where(x => x.DeductionType == request.DeductionType);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive.Value);

        if (request.IsAuthorized.HasValue)
            Query.Where(x => x.IsAuthorized == request.IsAuthorized.Value);

        // Pagination
        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

/// <summary>
/// Specification for getting deductions by employee.
/// </summary>
public sealed class PayrollDeductionsByEmployeeSpec : Specification<PayrollDeduction>
{
    public PayrollDeductionsByEmployeeSpec(DefaultIdType employeeId, DateTime effectiveDate)
    {
        Query
            .Where(x => x.EmployeeId == employeeId)
            .Where(x => x.IsActive)
            .Where(x => x.StartDate <= effectiveDate)
            .Where(x => !x.EndDate.HasValue || x.EndDate >= effectiveDate)
            .Include(x => x.PayComponent)
            .OrderBy(x => x.PayComponent.ComponentName);
    }
}

/// <summary>
/// Specification for getting deductions by organizational unit.
/// </summary>
public sealed class PayrollDeductionsByOrganizationalUnitSpec : Specification<PayrollDeduction>
{
    public PayrollDeductionsByOrganizationalUnitSpec(DefaultIdType organizationalUnitId, DateTime effectiveDate)
    {
        Query
            .Where(x => x.OrganizationalUnitId == organizationalUnitId)
            .Where(x => x.IsActive)
            .Where(x => x.StartDate <= effectiveDate)
            .Where(x => !x.EndDate.HasValue || x.EndDate >= effectiveDate)
            .Include(x => x.PayComponent)
            .OrderBy(x => x.PayComponent.ComponentName);
    }
}

