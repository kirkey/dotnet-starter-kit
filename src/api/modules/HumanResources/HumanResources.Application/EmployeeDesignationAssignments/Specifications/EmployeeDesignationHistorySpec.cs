using Ardalis.Specification;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Specifications;

/// <summary>
/// Specification to get employee designation history (all past and current designations).
/// </summary>
public class EmployeeDesignationHistorySpec : Specification<DesignationAssignment>
{
    public EmployeeDesignationHistorySpec(DefaultIdType employeeId)
    {
        Query
            .Where(a => a.EmployeeId == employeeId && a.IsActive)
            .Include(a => a.Designation)
            .OrderByDescending(a => a.EffectiveDate);
    }
}

