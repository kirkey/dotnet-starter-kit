namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanOfficerAssignments;

/// <summary>
/// View model for loan officer assignment creation (assignments typically do not support updates via UI).
/// </summary>
public class LoanOfficerAssignmentViewModel
{
    public Guid StaffId { get; set; }
    public string AssignmentType { get; set; } = string.Empty;
    public Guid? MemberId { get; set; }
    public Guid? MemberGroupId { get; set; }
    public Guid? LoanId { get; set; }
    public Guid? BranchId { get; set; }
    public DateTimeOffset AssignmentDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? EndDate { get; set; }
    public Guid? PreviousStaffId { get; set; }
    public string? Reason { get; set; }
    public bool IsPrimary { get; set; }
}
