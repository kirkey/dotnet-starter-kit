namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

public sealed record UpdateTimesheetCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] DefaultIdType? ApproverManagerId = null,
    [property: DefaultValue(null)] string? ManagerComment = null,
    [property: DefaultValue(null)] string? RejectionReason = null,
    [property: DefaultValue("Draft")] string? Status = null) : IRequest<UpdateTimesheetResponse>;

