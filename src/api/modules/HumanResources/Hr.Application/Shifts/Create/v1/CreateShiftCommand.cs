namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Create.v1;

public sealed record CreateShiftCommand(
    [property: DefaultValue("Morning")] string ShiftName,
    [property: DefaultValue("08:00:00")] TimeSpan StartTime,
    [property: DefaultValue("17:00:00")] TimeSpan EndTime,
    [property: DefaultValue(false)] bool IsOvernight = false,
    [property: DefaultValue(30)] int BreakDurationMinutes = 30,
    [property: DefaultValue(null)] string? Description = null) : IRequest<CreateShiftResponse>;
