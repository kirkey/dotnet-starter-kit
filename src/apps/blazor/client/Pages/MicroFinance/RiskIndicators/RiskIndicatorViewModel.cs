namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.RiskIndicators;

public class RiskIndicatorViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType RiskCategoryId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Formula { get; set; }
    public string? Unit { get; set; }
    public string? Direction { get; set; }
    public string? Frequency { get; set; }
    public string? DataSource { get; set; }
    public decimal? TargetValue { get; set; }
    public decimal? GreenThreshold { get; set; }
    public decimal? YellowThreshold { get; set; }
    public decimal? OrangeThreshold { get; set; }
    public decimal? RedThreshold { get; set; }
    public decimal WeightFactor { get; set; } = 1.0m;
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
