namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalRequests;

public class ApprovalRequestViewModel
{
    public string? RequestNumber { get; set; }
    public Guid WorkflowId { get; set; }
    public string? EntityType { get; set; }
    public Guid EntityId { get; set; }
    public int TotalLevels { get; set; } = 1;
    public Guid SubmittedById { get; set; }
    public decimal? Amount { get; set; }
    public Guid? BranchId { get; set; }
    public string? Comments { get; set; }
    public int? SlaHours { get; set; }
}
