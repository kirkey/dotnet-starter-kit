namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalWorkflows;

public class ApprovalWorkflowViewModel
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? EntityType { get; set; }
    public int NumberOfLevels { get; set; } = 1;
    public string? Description { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public bool IsSequential { get; set; } = true;
    public int Priority { get; set; } = 1;
}
