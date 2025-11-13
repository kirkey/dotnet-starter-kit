using Ardalis.Specification;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Specifications;

/// <summary>
/// Specification to get employee's current designation as of a specific date.
/// </summary>
public class EmployeeCurrentDesignationSpec : Specification<DesignationAssignment>
{
    public EmployeeCurrentDesignationSpec(DefaultIdType employeeId, DateTime? asOfDate = null)
    {
        var checkDate = asOfDate ?? DateTime.UtcNow;

        Query
            .Where(a => a.EmployeeId == employeeId
                && a.IsPlantilla
                && a.IsActive
                && a.EffectiveDate <= checkDate
                && (a.EndDate == null || a.EndDate > checkDate))
            .Include(a => a.Designation)
            .OrderByDescending(a => a.EffectiveDate)
            .Take(1);
    }
}

