namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Delete.v1;

/// <summary>
/// Command to delete a timesheet line.
/// </summary>
public sealed record DeleteTimesheetLineCommand(DefaultIdType Id) : IRequest<DeleteTimesheetLineResponse>;

