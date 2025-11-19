using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1;

/// <summary>
/// Handler for getting a timesheet line.
/// </summary>
public sealed class GetTimesheetLineHandler(
    ILogger<GetTimesheetLineHandler> logger,
    [FromKeyedServices("hr:timesheetlines")] IReadRepository<TimesheetLine> repository)
    : IRequestHandler<GetTimesheetLineRequest, TimesheetLineResponse>
{
    /// <summary>
    /// Handles the request to get a timesheet line.
    /// </summary>
    public async Task<TimesheetLineResponse> Handle(
        GetTimesheetLineRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
            throw new NotFoundException($"Timesheet line with ID '{request.Id}' was not found.");

        logger.LogInformation("Retrieved timesheet line {LineId}", entity.Id);

        return new TimesheetLineResponse
        {
            Id = entity.Id,
            TimesheetId = entity.TimesheetId,
            WorkDate = entity.WorkDate,
            RegularHours = entity.RegularHours,
            OvertimeHours = entity.OvertimeHours,
            TotalHours = entity.TotalHours,
            ProjectId = entity.ProjectId,
            TaskDescription = entity.TaskDescription,
            IsBillable = entity.IsBillable,
            BillingRate = entity.BillingRate
        };
    }
}

