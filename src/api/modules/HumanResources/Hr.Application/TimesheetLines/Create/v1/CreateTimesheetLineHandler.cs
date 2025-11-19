using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Create.v1;

/// <summary>
/// Handler for creating a timesheet line.
/// </summary>
public sealed class CreateTimesheetLineHandler(
    ILogger<CreateTimesheetLineHandler> logger,
    [FromKeyedServices("hr:timesheetlines")] IRepository<TimesheetLine> lineRepository,
    [FromKeyedServices("hr:timesheets")] IReadRepository<Timesheet> timesheetRepository)
    : IRequestHandler<CreateTimesheetLineCommand, CreateTimesheetLineResponse>
{
    /// <summary>
    /// Handles the request to create a timesheet line.
    /// </summary>
    public async Task<CreateTimesheetLineResponse> Handle(
        CreateTimesheetLineCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify timesheet exists
        var timesheet = await timesheetRepository
            .GetByIdAsync(request.TimesheetId, cancellationToken)
            .ConfigureAwait(false);

        if (timesheet is null)
            throw new NotFoundException($"Timesheet with ID '{request.TimesheetId}' was not found.");

        // Validate work date within period
        if (request.WorkDate.Date < timesheet.StartDate.Date || request.WorkDate.Date > timesheet.EndDate.Date)
            throw new BadRequestException("Work date must be within the timesheet period.");

        // Check for duplicate entry (only one line per day per timesheet)
        var existingLine = await lineRepository
            .AnyAsync(
                new TimesheetLineByTimesheetAndDateSpec(request.TimesheetId, request.WorkDate),
                cancellationToken)
            .ConfigureAwait(false);

        if (existingLine)
            throw new ConflictException("A timesheet line for the same date already exists in this timesheet.");

        // Create the line
        var line = TimesheetLine.Create(
            request.TimesheetId,
            request.WorkDate,
            request.RegularHours,
            request.OvertimeHours,
            request.ProjectId,
            request.TaskDescription);

        // Set billing information if provided
        if (request.BillingRate.HasValue)
            line.MarkAsBillable(request.BillingRate.Value);

        await lineRepository.AddAsync(line, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Timesheet line {LineId} created for timesheet {TimesheetId} on {WorkDate}",
            line.Id,
            timesheet.Id,
            line.WorkDate.Date);

        return new CreateTimesheetLineResponse(line.Id);
    }
}
