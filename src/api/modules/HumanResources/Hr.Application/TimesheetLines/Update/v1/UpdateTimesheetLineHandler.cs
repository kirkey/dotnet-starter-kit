using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Update.v1;

/// <summary>
/// Handler for updating a timesheet line.
/// </summary>
public sealed class UpdateTimesheetLineHandler(
    ILogger<UpdateTimesheetLineHandler> logger,
    [FromKeyedServices("hr:timesheetlines")] IRepository<TimesheetLine> repository)
    : IRequestHandler<UpdateTimesheetLineCommand, UpdateTimesheetLineResponse>
{
    /// <summary>
    /// Handles the request to update a timesheet line.
    /// </summary>
    public async Task<UpdateTimesheetLineResponse> Handle(
        UpdateTimesheetLineCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
            throw new NotFoundException($"Timesheet line with ID '{request.Id}' was not found.");

        // Update hours if provided
        if (request.RegularHours.HasValue || request.OvertimeHours.HasValue)
        {
            entity.UpdateHours(
                request.RegularHours ?? entity.RegularHours,
                request.OvertimeHours ?? entity.OvertimeHours);
        }

        // Update project information if provided
        if (!string.IsNullOrWhiteSpace(request.ProjectId) || !string.IsNullOrWhiteSpace(request.TaskDescription))
        {
            entity.SetProject(
                request.ProjectId ?? entity.ProjectId ?? string.Empty,
                request.TaskDescription ?? entity.TaskDescription);
        }

        // Update billing information if provided
        if (request.IsBillable.HasValue)
        {
            if (request.IsBillable.Value)
            {
                var rate = request.BillingRate ?? entity.BillingRate ?? 0;
                entity.MarkAsBillable(rate);
            }
            else
            {
                entity.MarkAsNonBillable();
            }
        }
        else if (request.BillingRate.HasValue && entity.IsBillable)
        {
            entity.MarkAsBillable(request.BillingRate.Value);
        }

        await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Timesheet line {LineId} updated", entity.Id);

        return new UpdateTimesheetLineResponse(entity.Id);
    }
}

