namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Specifications;

/// <summary>
/// Specification to get active plantilla designation for an employee.
/// </summary>
public class ActivePlantillaDesignationSpec : Specification<EmployeeDesignationAssignment>
{
    public ActivePlantillaDesignationSpec(DefaultIdType employeeId)
    {
        Query
            .Where(a => a.EmployeeId == employeeId 
                && a.IsPlantilla 
                && a.IsActive 
                && a.EffectiveDate <= DateTime.UtcNow 
                && (a.EndDate == null || a.EndDate > DateTime.UtcNow));
    }
}

