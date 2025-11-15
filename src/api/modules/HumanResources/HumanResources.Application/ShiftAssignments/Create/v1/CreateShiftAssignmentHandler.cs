namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Create.v1;

/// <summary>
/// Handler for creating shift assignments.
/// </summary>
public sealed class CreateShiftAssignmentHandler(
    ILogger<CreateShiftAssignmentHandler> logger,
    [FromKeyedServices("hr:shiftassignments")] IRepository<ShiftAssignment> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:shifts")] IReadRepository<Shift> shiftRepository)
    : IRequestHandler<CreateShiftAssignmentCommand, CreateShiftAssignmentResponse>
{
    public async Task<CreateShiftAssignmentResponse> Handle(
        CreateShiftAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify employee exists
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee == null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        // Verify shift exists
        var shift = await shiftRepository.GetByIdAsync(request.ShiftId, cancellationToken)
            .ConfigureAwait(false);

        if (shift == null)
            throw new ShiftNotFoundException(request.ShiftId);

        // Create shift assignment
        var shiftAssignment = ShiftAssignment.Create(
            request.EmployeeId,
            request.ShiftId,
            request.StartDate,
            request.EndDate,
            request.IsRecurring);

        await repository.AddAsync(shiftAssignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Shift assignment created with ID {AssignmentId} for Employee {EmployeeId} to Shift {ShiftId}",
            shiftAssignment.Id,
            request.EmployeeId,
            request.ShiftId);

        return new CreateShiftAssignmentResponse(shiftAssignment.Id);
    }
}

