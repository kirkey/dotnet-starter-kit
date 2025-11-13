namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;

public sealed record GetTimesheetRequest(DefaultIdType Id) : IRequest<TimesheetResponse>;

