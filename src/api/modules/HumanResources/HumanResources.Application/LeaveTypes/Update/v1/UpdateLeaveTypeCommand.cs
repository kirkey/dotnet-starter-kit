namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Update.v1;

public sealed record UpdateLeaveTypeCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] decimal? AnnualAllowance = null,
    [property: DefaultValue(null)] decimal? MaxCarryoverDays = null,
    [property: DefaultValue(null)] bool? IsActive = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<UpdateLeaveTypeResponse>;

