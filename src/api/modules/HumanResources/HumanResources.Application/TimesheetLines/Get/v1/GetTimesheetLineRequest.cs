namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1;

/// <summary>
/// Request to get a timesheet line.
/// </summary>
public sealed record GetTimesheetLineRequest(DefaultIdType Id) : IRequest<TimesheetLineResponse>;

