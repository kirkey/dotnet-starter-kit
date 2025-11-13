using Ardalis.Specification;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Specifications;

/// <summary>
/// Specification for searching employee designation history with filters.
/// </summary>
public class SearchEmployeeHistorySpec : Specification<DesignationAssignment>
{
    public SearchEmployeeHistorySpec(SearchEmployeeHistoryRequest request)
    {
        // Include related entities
        Query
            .Include(a => a.Employee)
                .ThenInclude(e => e.OrganizationalUnit)
            .Include(a => a.Designation);

        // Base filters
        Query.Where(a => a.IsActive);
        Query.Where(a => a.IsPlantilla || request.IncludeActingDesignations);

        // Apply organizational unit filter
        if (request.OrganizationalUnitId.HasValue)
        {
            Query.Where(a => a.Employee.OrganizationalUnitId == request.OrganizationalUnitId);
        }

        // Apply designation filter
        if (request.DesignationId.HasValue)
        {
            Query.Where(a => a.DesignationId == request.DesignationId);
        }

        // Apply date range filters
        if (request.FromDate.HasValue)
        {
            Query.Where(a => a.EffectiveDate >= request.FromDate);
        }

        if (request.ToDate.HasValue)
        {
            Query.Where(a => a.EffectiveDate <= request.ToDate);
        }

        // Apply point-in-time filter
        if (request.PointInTimeDate.HasValue)
        {
            var date = request.PointInTimeDate.Value;
            Query.Where(a => a.EffectiveDate <= date && (a.EndDate == null || a.EndDate > date));
        }

        // Order by effective date descending
        Query.OrderByDescending(a => a.EffectiveDate);
    }
}

