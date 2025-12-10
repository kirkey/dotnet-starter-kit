namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanOfficerAssignments;

/// <summary>
/// View model for loan officer assignment creation (assignments typically do not support updates via UI).
/// </summary>
public class LoanOfficerAssignmentViewModel
{
    public DefaultIdType StaffId { get; set; }
    public string AssignmentType { get; set; } = string.Empty;
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? MemberGroupId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public DateTimeOffset AssignmentDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? EndDate { get; set; }
    public DefaultIdType? PreviousStaffId { get; set; }
    public string? Reason { get; set; }
    public bool IsPrimary { get; set; }
}
