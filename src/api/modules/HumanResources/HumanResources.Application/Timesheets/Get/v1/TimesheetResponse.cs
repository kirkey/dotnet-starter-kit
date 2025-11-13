namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;

public sealed record TimesheetResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string PeriodType { get; init; } = default!;
    public decimal RegularHours { get; init; }
    public decimal OvertimeHours { get; init; }
    public decimal TotalHours { get; init; }
    public string Status { get; init; } = default!;
    public DefaultIdType? ApproverManagerId { get; init; }
    public DateTime? SubmittedDate { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public string? ManagerComment { get; init; }
    public string? RejectionReason { get; init; }
    public bool IsLocked { get; init; }
    public bool IsApproved { get; init; }
}

