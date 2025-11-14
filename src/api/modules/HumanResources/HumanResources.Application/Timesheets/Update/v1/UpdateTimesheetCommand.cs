namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

public sealed record UpdateTimesheetCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Status = null,
    [property: DefaultValue(null)] string? ManagerComment = null,
    [property: DefaultValue(null)] DefaultIdType? ApproverId = null) : IRequest<UpdateTimesheetResponse>;

