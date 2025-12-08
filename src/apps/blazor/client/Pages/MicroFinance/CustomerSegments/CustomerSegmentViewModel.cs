namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerSegments;

/// <summary>
/// ViewModel for Customer Segment add/edit operations.
/// </summary>
public class CustomerSegmentViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? SegmentType { get; set; }
    public string? SegmentCriteria { get; set; }
    public int Priority { get; set; } = 1;
    public decimal? MinIncomeLevel { get; set; }
    public decimal? MaxIncomeLevel { get; set; }
    public string? RiskLevel { get; set; }
    public decimal? DefaultInterestModifier { get; set; }
    public decimal? DefaultFeeModifier { get; set; }
}
