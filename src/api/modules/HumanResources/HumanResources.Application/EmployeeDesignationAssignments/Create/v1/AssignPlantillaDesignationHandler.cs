using FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Create.v1;

/// <summary>
/// Handler for assigning a plantilla (primary) designation to an employee.
/// </summary>
public sealed class AssignPlantillaDesignationHandler(
    ILogger<AssignPlantillaDesignationHandler> logger,
    [FromKeyedServices("hr:designationassignments")] IRepository<DesignationAssignment> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:designationassignments")] IReadRepository<DesignationAssignment> readRepository)
    : IRequestHandler<AssignPlantillaDesignationCommand, AssignDesignationResponse>
{
    public async Task<AssignDesignationResponse> Handle(AssignPlantillaDesignationCommand request, CancellationToken cancellationToken)
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

        // Check if employee already has an active plantilla designation
        var existingPlantilla = await readRepository
            .FirstOrDefaultAsync(
                new ActivePlantillaDesignationSpec(request.EmployeeId),
                cancellationToken)
            .ConfigureAwait(false);

        if (existingPlantilla is not null && request.PreviousAssignmentId != existingPlantilla.Id)
        {
            // If replacing, end the previous one
            if (request.PreviousAssignmentId.HasValue && request.PreviousAssignmentId == existingPlantilla.Id)
            {
                existingPlantilla.SetEndDate(request.EffectiveDate.AddDays(-1));
                await repository.UpdateAsync(existingPlantilla, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                throw new MultipleActivePlantillaException(request.EmployeeId);
            }
        }

        // Create new plantilla assignment
        var assignment = DesignationAssignment.CreatePlantilla(
            request.EmployeeId,
            request.DesignationId,
            request.EffectiveDate,
            request.Reason);

        await repository.AddAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Plantilla designation assigned to employee {EmployeeId}, Designation {DesignationId}, Effective {EffectiveDate}",
            request.EmployeeId,
            request.DesignationId,
            request.EffectiveDate);

        return new AssignDesignationResponse(assignment.Id);
    }
}

