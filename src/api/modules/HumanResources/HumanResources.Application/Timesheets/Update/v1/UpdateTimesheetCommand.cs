namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

public sealed record UpdateTimesheetCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] decimal? RegularHours = null,
    [property: DefaultValue(null)] decimal? OvertimeHours = null,
    [property: DefaultValue(null)] string? Status = null) : IRequest<UpdateTimesheetResponse>;

