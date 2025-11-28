namespace FSH.Starter.Blazor.Client.Pages.Hr.LeaveRequests;

/// <summary>
/// ViewModel for LeaveRequest CRUD operations.
/// Represents employee leave requests with approval workflow.
/// </summary>
public class LeaveRequestViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType EmployeeId { get; set; }
    public DefaultIdType LeaveTypeId { get; set; }
    public DateTime? StartDate { get; set; } = DateTime.Today;
    public DateTime? EndDate { get; set; } = DateTime.Today;
    public decimal NumberOfDays { get; set; } = 1;
    public string? Reason { get; set; }
    public string Status { get; set; } = "Draft";
    public DefaultIdType? ApproverManagerId { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public string? ApproverComment { get; set; }
    public bool IsActive { get; set; } = true;
}
