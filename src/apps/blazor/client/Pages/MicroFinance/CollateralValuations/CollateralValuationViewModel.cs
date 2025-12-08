namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralValuations;

/// <summary>
/// View model for collateral valuation creation and update.
/// </summary>
public class CollateralValuationViewModel
{
    public Guid CollateralId { get; set; }
    public DateTimeOffset ValuationDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? ExpiryDate { get; set; }
    public string ValuationMethod { get; set; } = string.Empty;
    public string? AppraiserName { get; set; }
    public string? AppraiserCompany { get; set; }
    public string? AppraiserLicense { get; set; }
    public decimal MarketValue { get; set; }
    public decimal ForcedSaleValue { get; set; }
    public decimal InsurableValue { get; set; }
    public string? Condition { get; set; }
    public string? Notes { get; set; }
    public string? DocumentPath { get; set; }
}
