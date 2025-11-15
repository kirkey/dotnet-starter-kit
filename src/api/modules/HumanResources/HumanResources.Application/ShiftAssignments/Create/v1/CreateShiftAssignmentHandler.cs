namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Create.v1;

using Domain.Exceptions;

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
        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        // Verify shift exists
        var shift = await shiftRepository
            .GetByIdAsync(request.ShiftId, cancellationToken)
            .ConfigureAwait(false);

        if (shift is null)
            throw new ShiftNotFoundException(request.ShiftId);

        // Create the assignment
        var assignment = ShiftAssignment.Create(
            request.EmployeeId,
            request.ShiftId,
            request.StartDate,
            request.EndDate,
            request.IsRecurring);

        // Configure recurring if applicable
        if (request is { IsRecurring: true, RecurringDayOfWeek: not null })
            assignment.SetRecurring(request.RecurringDayOfWeek.Value);

        // Add notes if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
            assignment.AddNotes(request.Notes);

        await repository.AddAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Shift assignment created with ID {AssignmentId} for employee {EmployeeId} to shift {ShiftId}",
            assignment.Id,
            employee.Id,
            shift.Id);

        return new CreateShiftAssignmentResponse(assignment.Id);
    }
}

