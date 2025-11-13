using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Search.v1;

/// <summary>
/// Handler for searching employee designation history with temporal queries.
/// </summary>
public sealed class SearchEmployeeHistoryHandler(
    [FromKeyedServices("hr:designationassignments")] IReadRepository<DesignationAssignment> repository)
    : IRequestHandler<SearchEmployeeHistoryRequest, PagedList<EmployeeHistoryDto>>
{
    public async Task<PagedList<EmployeeHistoryDto>> Handle(
        SearchEmployeeHistoryRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create specification based on filters
        var spec = new SearchEmployeeHistorySpec(request);

        // Get all matching assignments with related data
        var allAssignments = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        // Group by employee and build DTOs
        var employeeGroups = allAssignments
            .GroupBy(a => a.EmployeeId)
            .Select(g => new EmployeeHistoryDto
            {
                EmployeeId = g.Key,
                EmployeeNumber = g.First().Employee.EmployeeNumber,
                FullName = g.First().Employee.FullName,
                CurrentDesignation = g.FirstOrDefault(a => a.IsCurrentlyEffective())?.Designation.Title ?? "Not Assigned",
                CurrentDesignationStart = g.FirstOrDefault(a => a.IsCurrentlyEffective())?.EffectiveDate,
                OrganizationalUnitName = g.First().Employee.OrganizationalUnit?.Name ?? "Unknown",
                TotalDesignationChanges = g.Count(),
                DesignationHistory = g
                    .OrderByDescending(a => a.EffectiveDate)
                    .Select(a => new DesignationHistoryDto
                    {
                        Designation = a.Designation.Title,
                        EffectiveDate = a.EffectiveDate,
                        EndDate = a.EndDate,
                        TenureMonths = (int)Math.Round(
                            ((a.EndDate ?? DateTime.UtcNow) - a.EffectiveDate).TotalDays / 30.44),
                        IsPlantilla = a.IsPlantilla,
                        IsActingAs = a.IsActingAs
                    })
                    .ToList()
            })
            .ToList();

        // Apply pagination
        var totalCount = employeeGroups.Count;
        var paginatedResults = employeeGroups
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedList<EmployeeHistoryDto>(
            paginatedResults,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}



