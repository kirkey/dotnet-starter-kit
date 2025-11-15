namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Update.v1;

/// <summary>
/// Command to update a timesheet line.
/// </summary>
public sealed record UpdateTimesheetLineCommand(
    DefaultIdType Id,
    decimal? RegularHours = null,
    decimal? OvertimeHours = null,
    string? ProjectId = null,
    string? TaskDescription = null,
    bool? IsBillable = null,
    decimal? BillingRate = null) : IRequest<UpdateTimesheetLineResponse>;

