namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceProducts;

/// <summary>
/// ViewModel used by the InsuranceProducts page for add/edit operations.
/// Mirrors the shape of the API's CreateInsuranceProductCommand and UpdateInsuranceProductCommand.
/// </summary>
public class InsuranceProductViewModel
{
    /// <summary>
    /// Primary identifier of the insurance product.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique product code. Example: "INS-LIFE-001".
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Product name. Required.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description of the insurance product.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Type of insurance: "Life", "Health", "Crop", "Livestock", "Property".
    /// </summary>
    public string? InsuranceType { get; set; }

    /// <summary>
    /// Insurance provider or underwriter.
    /// </summary>
    public string? Provider { get; set; }

    /// <summary>
    /// Minimum coverage amount.
    /// </summary>
    public decimal MinCoverage { get; set; }

    /// <summary>
    /// Maximum coverage amount.
    /// </summary>
    public decimal MaxCoverage { get; set; }

    /// <summary>
    /// Premium rate as a percentage of coverage.
    /// </summary>
    public decimal PremiumRate { get; set; }

    /// <summary>
    /// Premium frequency: "Monthly", "Quarterly", "Annually", "Single".
    /// </summary>
    public string? PremiumFrequency { get; set; }

    /// <summary>
    /// Waiting period in days before coverage becomes effective.
    /// </summary>
    public int WaitingPeriodDays { get; set; }

    /// <summary>
    /// Whether this insurance is mandatory with loan products.
    /// </summary>
    public bool MandatoryWithLoan { get; set; }

    /// <summary>
    /// Currency code. Default: "PHP".
    /// </summary>
    public string CurrencyCode { get; set; } = "PHP";
}
