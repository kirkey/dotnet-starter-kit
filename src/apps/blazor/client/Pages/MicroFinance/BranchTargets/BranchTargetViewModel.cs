namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.BranchTargets;

/// <summary>
/// ViewModel for Branch Target add/edit operations.
/// </summary>
public class BranchTargetViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType BranchId { get; set; }
    public string? TargetType { get; set; }
    public string? Description { get; set; }
    public string? Period { get; set; }
    public DateTime? PeriodStartDate { get; set; }
    public DateTime? PeriodEndDate { get; set; }
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public decimal TargetValue { get; set; }
    public string? MetricUnit { get; set; }
    public decimal? MinimumThreshold { get; set; }
    public decimal? StretchTarget { get; set; }
    public decimal Weight { get; set; } = 100;
    public string? Notes { get; set; }
}
