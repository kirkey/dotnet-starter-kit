namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Specifications;

/// <summary>
/// Specification to check for duplicate active assignments.
/// </summary>
public class ActiveDesignationAssignmentSpec : Specification<DesignationAssignment>
{
    public ActiveDesignationAssignmentSpec(DefaultIdType employeeId, DefaultIdType designationId)
    {
        Query
            .Where(a => a.EmployeeId == employeeId 
                && a.DesignationId == designationId 
                && a.IsActive 
                && a.EffectiveDate <= DateTime.UtcNow 
                && (a.EndDate == null || a.EndDate > DateTime.UtcNow));
    }
}

