namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Specifications;

/// <summary>
/// Specification to get designation assignment by ID.
/// </summary>
public class DesignationAssignmentByIdSpec : Specification<EmployeeDesignationAssignment>
{
    public DesignationAssignmentByIdSpec(DefaultIdType id)
    {
        Query
            .Where(a => a.Id == id)
            .Include(a => a.Employee)
            .Include(a => a.Designation);
    }
}

