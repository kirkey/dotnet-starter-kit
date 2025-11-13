namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;

public sealed record ShiftResponse
{
    public DefaultIdType Id { get; init; }
    public string ShiftName { get; init; } = default!;
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public bool IsOvernight { get; init; }
    public int BreakDurationMinutes { get; init; }
    public decimal WorkingHours { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}

