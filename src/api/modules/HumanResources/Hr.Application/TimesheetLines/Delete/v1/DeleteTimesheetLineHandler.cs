using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Delete.v1;

/// <summary>
/// Handler for deleting a timesheet line.
/// </summary>
public sealed class DeleteTimesheetLineHandler(
    ILogger<DeleteTimesheetLineHandler> logger,
    [FromKeyedServices("hr:timesheetlines")] IRepository<TimesheetLine> repository)
    : IRequestHandler<DeleteTimesheetLineCommand, DeleteTimesheetLineResponse>
{
    /// <summary>
    /// Handles the request to delete a timesheet line.
    /// </summary>
    public async Task<DeleteTimesheetLineResponse> Handle(
        DeleteTimesheetLineCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
            throw new NotFoundException($"Timesheet line with ID '{request.Id}' was not found.");

        await repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Timesheet line {LineId} deleted", entity.Id);

        return new DeleteTimesheetLineResponse(entity.Id);
    }
}

