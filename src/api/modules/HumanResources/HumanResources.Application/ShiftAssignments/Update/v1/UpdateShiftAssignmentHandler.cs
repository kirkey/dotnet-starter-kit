namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Update.v1;

using Domain.Exceptions;

/// <summary>
/// Handler for updating shift assignments.
/// </summary>
public sealed class UpdateShiftAssignmentHandler(
    ILogger<UpdateShiftAssignmentHandler> logger,
    [FromKeyedServices("hr:shiftassignments")] IRepository<ShiftAssignment> repository)
    : IRequestHandler<UpdateShiftAssignmentCommand, UpdateShiftAssignmentResponse>
{
    public async Task<UpdateShiftAssignmentResponse> Handle(
        UpdateShiftAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assignment = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (assignment is null)
            throw new ShiftAssignmentNotFoundException(request.Id);

        // Update dates if provided
        if (request.StartDate.HasValue || request.EndDate.HasValue)
        {
            assignment.UpdateDates(
                request.StartDate ?? assignment.StartDate,
                request.EndDate ?? assignment.EndDate);
        }

        // Update recurring settings if provided
        if (request is { IsRecurring: true, RecurringDayOfWeek: not null })
        {
            assignment.SetRecurring(request.RecurringDayOfWeek.Value);
        }

        // Update notes if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            assignment.AddNotes(request.Notes);
        }

        await repository.UpdateAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Shift assignment {AssignmentId} updated", assignment.Id);

        return new UpdateShiftAssignmentResponse(assignment.Id);
    }
}

