namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Update.v1;

public sealed record UpdateShiftCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("Morning")] string? ShiftName = null,
    [property: DefaultValue(null)] TimeSpan? StartTime = null,
    [property: DefaultValue(null)] TimeSpan? EndTime = null,
    [property: DefaultValue(null)] int? BreakDurationMinutes = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<UpdateShiftResponse>;

