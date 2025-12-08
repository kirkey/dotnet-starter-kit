namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanOfficerTargets;

/// <summary>
/// View model for loan officer target creation.
/// </summary>
public class LoanOfficerTargetViewModel
{
    public Guid StaffId { get; set; }
    public string TargetType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Period { get; set; } = string.Empty;
    public DateTimeOffset PeriodStart { get; set; }
    public DateTimeOffset PeriodEnd { get; set; }
    public decimal TargetValue { get; set; }
    public string? MetricUnit { get; set; }
    public decimal? MinimumThreshold { get; set; }
    public decimal? StretchTarget { get; set; }
    public decimal Weight { get; set; } = 1.0m;
    public decimal? IncentiveAmount { get; set; }
    public decimal? StretchBonus { get; set; }
    public string? Notes { get; set; }
}
