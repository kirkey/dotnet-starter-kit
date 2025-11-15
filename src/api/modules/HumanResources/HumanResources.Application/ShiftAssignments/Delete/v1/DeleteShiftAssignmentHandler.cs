namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Delete.v1;

using Domain.Exceptions;

/// <summary>
/// Handler for deleting shift assignments.
/// </summary>
public sealed class DeleteShiftAssignmentHandler(
    ILogger<DeleteShiftAssignmentHandler> logger,
    [FromKeyedServices("hr:shiftassignments")] IRepository<ShiftAssignment> repository)
    : IRequestHandler<DeleteShiftAssignmentCommand, DeleteShiftAssignmentResponse>
{
    public async Task<DeleteShiftAssignmentResponse> Handle(
        DeleteShiftAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assignment = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (assignment is null)
            throw new ShiftAssignmentNotFoundException(request.Id);

        await repository.DeleteAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Shift assignment {AssignmentId} deleted", assignment.Id);

        return new DeleteShiftAssignmentResponse(assignment.Id);
    }
}

