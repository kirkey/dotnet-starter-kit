namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;

/// <summary>
/// Request to get a timesheet by its identifier.
/// </summary>
public sealed record GetTimesheetRequest(DefaultIdType Id) : IRequest<TimesheetResponse>;

