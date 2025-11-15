namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;

using Domain.Exceptions;

/// <summary>
/// Handler for retrieving shift assignment details.
/// </summary>
public sealed class GetShiftAssignmentHandler(
    ILogger<GetShiftAssignmentHandler> logger,
    [FromKeyedServices("hr:shiftassignments")] IReadRepository<ShiftAssignment> repository)
    : IRequestHandler<GetShiftAssignmentRequest, ShiftAssignmentResponse>
{
    public async Task<ShiftAssignmentResponse> Handle(
        GetShiftAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assignment = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (assignment is null)
            throw new ShiftAssignmentNotFoundException(request.Id);

        logger.LogInformation("Retrieved shift assignment {AssignmentId}", assignment.Id);

        return new ShiftAssignmentResponse
        {
            Id = assignment.Id,
            EmployeeId = assignment.EmployeeId,
            EmployeeName = assignment.Employee?.FullName,
            ShiftId = assignment.ShiftId,
            ShiftName = assignment.Shift?.ShiftName,
            ShiftStartTime = assignment.Shift?.StartTime,
            ShiftEndTime = assignment.Shift?.EndTime,
            StartDate = assignment.StartDate,
            EndDate = assignment.EndDate,
            IsRecurring = assignment.IsRecurring,
            RecurringDayOfWeek = assignment.RecurringDayOfWeek,
            Notes = assignment.Notes,
            IsActive = assignment.IsActive
        };
    }
}

