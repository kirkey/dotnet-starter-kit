namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Update.v1;

public sealed record UpdateShiftCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? ShiftName = null,
    [property: DefaultValue(null)] TimeSpan? StartTime = null,
    [property: DefaultValue(null)] TimeSpan? EndTime = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<UpdateShiftResponse>;

