namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Specifications;

using Ardalis.Specification;
using Search.v1;
using Domain.Entities;

/// <summary>
/// Specification for searching benefit allocations with filters.
/// </summary>
public sealed class SearchBenefitAllocationsSpec : Specification<BenefitAllocation>
{
    public SearchBenefitAllocationsSpec(SearchBenefitAllocationsRequest request)
    {
        Query.OrderByDescending(x => x.AllocationDate)
            .Include(x => x.Enrollment)
                .ThenInclude(e => e.Employee)
            .Include(x => x.Enrollment)
                .ThenInclude(e => e.Benefit);

        if (request.EnrollmentId.HasValue)
            Query.Where(x => x.EnrollmentId == request.EnrollmentId);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.Enrollment.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.Status))
            Query.Where(x => x.Status == request.Status);

        if (!string.IsNullOrWhiteSpace(request.AllocationType))
            Query.Where(x => x.AllocationType == request.AllocationType);

        if (request.FromDate.HasValue)
            Query.Where(x => x.AllocationDate >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            Query.Where(x => x.AllocationDate <= request.ToDate.Value);

        // Pagination
        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

