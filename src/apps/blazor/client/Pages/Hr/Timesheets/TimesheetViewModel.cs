namespace FSH.Starter.Blazor.Client.Pages.Hr.Timesheets;

/// <summary>
/// ViewModel for Timesheet CRUD operations.
/// Represents employee timesheets for time tracking.
/// </summary>
public class TimesheetViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType EmployeeId { get; set; }
    public DateTime? StartDate { get; set; } = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start of current week
    public DateTime? EndDate { get; set; } = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek); // End of current week
    public string PeriodType { get; set; } = "Weekly";
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public decimal TotalHours { get; set; }
    public string Status { get; set; } = "Draft";
    public DefaultIdType? ApproverManagerId { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public bool IsLocked { get; set; }
    public bool IsApproved { get; set; }
}
