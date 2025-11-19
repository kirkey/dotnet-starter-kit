using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Create.v1;

/// <summary>
/// Handler for assigning an "Acting As" (temporary) designation to an employee.
/// </summary>
public sealed class AssignActingAsDesignationHandler(
    ILogger<AssignActingAsDesignationHandler> logger,
    [FromKeyedServices("hr:designationassignments")] IRepository<DesignationAssignment> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:designationassignments")] IReadRepository<DesignationAssignment> readRepository)
    : IRequestHandler<AssignActingAsDesignationCommand, AssignDesignationResponse>
{
    public async Task<AssignDesignationResponse> Handle(AssignActingAsDesignationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify employee exists
        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(request.EmployeeId);
        }

        // Validate dates
        if (request.EndDate.HasValue && request.EndDate <= request.EffectiveDate)
        {
            throw new InvalidDesignationAssignmentDatesException(
                "End date must be after effective date.");
        }

        // Check for duplicate active assignments
        var existingAssignment = await readRepository
            .FirstOrDefaultAsync(
                new ActiveDesignationAssignmentSpec(request.EmployeeId, request.DesignationId),
                cancellationToken)
            .ConfigureAwait(false);

        if (existingAssignment is not null)
        {
            throw new DuplicateDesignationAssignmentException(request.EmployeeId, request.DesignationId);
        }

        // Create new acting assignment
        var assignment = DesignationAssignment.CreateActingAs(
            request.EmployeeId,
            request.DesignationId,
            request.EffectiveDate,
            request.EndDate,
            request.AdjustedSalary,
            request.Reason);

        await repository.AddAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Acting As designation assigned to employee {EmployeeId}, Designation {DesignationId}, Effective {EffectiveDate} to {EndDate}",
            request.EmployeeId,
            request.DesignationId,
            request.EffectiveDate,
            request.EndDate ?? DateTime.MaxValue);

        return new AssignDesignationResponse(assignment.Id);
    }
}

