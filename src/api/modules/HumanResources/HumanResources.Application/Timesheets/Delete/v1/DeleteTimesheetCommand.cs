namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Delete.v1;

public sealed record DeleteTimesheetCommand(DefaultIdType Id) : IRequest<DeleteTimesheetResponse>;

