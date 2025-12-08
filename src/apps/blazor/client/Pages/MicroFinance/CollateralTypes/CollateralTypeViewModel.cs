namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralTypes;

/// <summary>
/// ViewModel used by the CollateralTypes page for add/edit operations.
/// Mirrors the shape of the API's CreateCollateralTypeCommand and UpdateCollateralTypeCommand.
/// </summary>
public class CollateralTypeViewModel
{
    /// <summary>
    /// Primary identifier of the collateral type.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique collateral type code. Example: "RE", "VH", "EQ".
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Collateral type name. Example: "Real Estate", "Vehicle", "Equipment".
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category of collateral. Values: "RealEstate", "Vehicle", "Equipment", "Inventory", "Securities", "Deposit", "Other".
    /// </summary>
    public string Category { get; set; } = "RealEstate";

    /// <summary>
    /// Detailed description of the collateral type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Default loan-to-value percentage for this collateral type.
    /// Represents the standard percentage of collateral value that can be loaned.
    /// </summary>
    public decimal DefaultLtvPercent { get; set; } = 70;

    /// <summary>
    /// Maximum loan-to-value percentage allowed for this collateral type.
    /// </summary>
    public decimal MaxLtvPercent { get; set; } = 80;

    /// <summary>
    /// Default expected useful life of this collateral type in years.
    /// Used for depreciation calculations.
    /// </summary>
    public int DefaultUsefulLifeYears { get; set; } = 10;

    /// <summary>
    /// Annual depreciation rate as a percentage.
    /// Used to calculate collateral value reduction over time.
    /// </summary>
    public decimal AnnualDepreciationRate { get; set; } = 10;

    /// <summary>
    /// Whether this collateral type requires insurance coverage.
    /// </summary>
    public bool RequiresInsurance { get; set; }

    /// <summary>
    /// Whether this collateral type requires professional appraisal/valuation.
    /// </summary>
    public bool RequiresAppraisal { get; set; }
}
