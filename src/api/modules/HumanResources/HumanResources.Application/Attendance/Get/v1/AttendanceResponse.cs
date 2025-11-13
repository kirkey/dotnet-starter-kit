namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;

public sealed record AttendanceResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public DateTime AttendanceDate { get; init; }
    public TimeSpan? ClockInTime { get; init; }
    public TimeSpan? ClockOutTime { get; init; }
    public string? ClockInLocation { get; init; }
    public string? ClockOutLocation { get; init; }
    public decimal HoursWorked { get; init; }
    public string Status { get; init; } = default!;
    public int? MinutesLate { get; init; }
    public string? Reason { get; init; }
    public bool IsApproved { get; init; }
    public string? ManagerComment { get; init; }
    public bool IsActive { get; init; }
}

