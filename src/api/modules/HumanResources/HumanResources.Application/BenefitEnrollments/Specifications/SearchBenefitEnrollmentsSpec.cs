namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Specifications;

using Ardalis.Specification;
using Search.v1;
using Domain.Entities;

/// <summary>
/// Specification for searching benefit enrollments with filters.
/// </summary>
public sealed class SearchBenefitEnrollmentsSpec : Specification<BenefitEnrollment>
{
    public SearchBenefitEnrollmentsSpec(SearchBenefitEnrollmentsRequest request)
    {
        Query.OrderByDescending(x => x.EnrollmentDate)
            .Include(x => x.Employee)
            .Include(x => x.Benefit);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.BenefitId.HasValue)
            Query.Where(x => x.BenefitId == request.BenefitId);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.CoverageLevel))
            Query.Where(x => x.CoverageLevel == request.CoverageLevel);

        // Pagination
        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

