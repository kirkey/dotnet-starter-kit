namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Specifications;

/// <summary>
/// Specification for getting an enrollment by ID.
/// </summary>
public class EnrollmentByIdSpec : Specification<BenefitEnrollment>, ISingleResultSpecification<BenefitEnrollment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollmentByIdSpec"/> class.
    /// </summary>
    public EnrollmentByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee)
            .Include(x => x.Benefit);
    }
}

/// <summary>
/// Specification for searching enrollments with filters.
/// </summary>
public class SearchEnrollmentsSpec : Specification<BenefitEnrollment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchEnrollmentsSpec"/> class.
    /// </summary>
    public SearchEnrollmentsSpec(Search.v1.SearchEnrollmentsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .Include(x => x.Benefit)
            .OrderBy(x => x.EnrollmentDate)
            .ThenBy(x => x.EmployeeId);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.BenefitId.HasValue)
            Query.Where(x => x.BenefitId == request.BenefitId);

        if (!string.IsNullOrWhiteSpace(request.CoverageLevel))
            Query.Where(x => x.CoverageLevel == request.CoverageLevel);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);

        if (request.EnrollmentDateFrom.HasValue)
            Query.Where(x => x.EnrollmentDate >= request.EnrollmentDateFrom);

        if (request.EnrollmentDateTo.HasValue)
            Query.Where(x => x.EnrollmentDate <= request.EnrollmentDateTo);
    }
}

