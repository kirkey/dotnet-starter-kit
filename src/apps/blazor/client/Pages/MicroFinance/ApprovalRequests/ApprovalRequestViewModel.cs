namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalRequests;

public class ApprovalRequestViewModel
{
    public string? RequestNumber { get; set; }
    public DefaultIdType WorkflowId { get; set; }
    public string? EntityType { get; set; }
    public DefaultIdType EntityId { get; set; }
    public int TotalLevels { get; set; } = 1;
    public DefaultIdType SubmittedById { get; set; }
    public decimal? Amount { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public string? Comments { get; set; }
    public int? SlaHours { get; set; }
}
