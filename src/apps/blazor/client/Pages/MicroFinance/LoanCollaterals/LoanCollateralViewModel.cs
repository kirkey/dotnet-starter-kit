namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanCollaterals;

/// <summary>
/// ViewModel used by the LoanCollaterals page for add operations.
/// Mirrors the shape of the API's CreateLoanCollateralCommand so Mapster/Adapt can map between them.
/// </summary>
public class LoanCollateralViewModel
{
    /// <summary>
    /// Primary identifier of the loan collateral.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The loan this collateral is pledged for. Required.
    /// </summary>
    public DefaultIdType LoanId { get; set; }

    /// <summary>
    /// Type of collateral (e.g., RealEstate, Vehicle, Equipment).
    /// </summary>
    public string? CollateralType { get; set; } = "RealEstate";

    /// <summary>
    /// Description of the collateral.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Estimated market value of the collateral.
    /// </summary>
    public decimal EstimatedValue { get; set; } = 100000m;

    /// <summary>
    /// Forced sale value (liquidation value).
    /// </summary>
    public decimal? ForcedSaleValue { get; set; }

    /// <summary>
    /// Date of the valuation.
    /// </summary>
    public DateTimeOffset? ValuationDate { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Location of the collateral.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Document reference (e.g., title number, VIN).
    /// </summary>
    public string? DocumentReference { get; set; }

    /// <summary>
    /// Notes about the collateral.
    /// </summary>
    public string? Notes { get; set; }
}
