using Ardalis.Specification;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Specifications;

/// <summary>
/// Specification to get all active employees on a specific date with their designations.
/// </summary>
public class ActiveEmployeesOnDateSpec : Specification<DesignationAssignment>
{
    public ActiveEmployeesOnDateSpec(DateTime asOfDate)
    {
        Query
            .Where(a => a.IsPlantilla
                && a.IsActive
                && a.EffectiveDate <= asOfDate
                && (a.EndDate == null || a.EndDate > asOfDate))
            .Include(a => a.Employee)
            .Include(a => a.Designation)
            .OrderBy(a => a.Employee.EmployeeNumber);
    }
}

