namespace Accounting.Application.TaxCodes.Create.v1;

/// <summary>
/// Command to create a new tax code configuration.
/// Tax codes are used for calculating sales tax, VAT, GST, and other taxes on transactions.
/// </summary>
public class CreateTaxCodeCommand : BaseRequest, IRequest<DefaultIdType>
{
    /// <summary>
    /// Unique tax code identifier (e.g., "VAT-STD", "SALES-CA", "GST").
    /// Must be uppercase alphanumeric with hyphens/underscores only.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Tax type classification: SalesTax, VAT, GST, UseTax, Excise, Withholding, Property, Other.
    /// </summary>
    public string TaxType { get; set; } = string.Empty;

    /// <summary>
    /// Tax rate as decimal (e.g., 0.0825 for 8.25%).
    /// Must be between 0 and 1 (0% to 100%).
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Whether this tax is calculated on the subtotal plus other taxes (compound tax).
    /// Default: false.
    /// </summary>
    public bool IsCompound { get; set; }

    /// <summary>
    /// Jurisdiction or region this tax applies to (e.g., "California", "UK", "Ontario").
    /// Optional but recommended for multi-jurisdiction operations.
    /// </summary>
    public string? Jurisdiction { get; set; }

    /// <summary>
    /// Date when this tax rate becomes effective.
    /// Cannot be more than one day in the past.
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Optional date when this tax rate expires.
    /// Must be after the effective date if specified.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Account ID to record tax collected (liability account).
    /// Required for tracking tax remittance obligations.
    /// </summary>
    public DefaultIdType TaxCollectedAccountId { get; set; }

    /// <summary>
    /// Optional account ID to record tax paid on purchases (expense/asset account).
    /// Used for input tax credits or tax recovery.
    /// </summary>
    public DefaultIdType? TaxPaidAccountId { get; set; }

    /// <summary>
    /// Tax authority or agency to remit collected taxes to.
    /// Example: "IRS", "HMRC", "CRA", "State Board of Equalization".
    /// </summary>
    public string? TaxAuthority { get; set; }

    /// <summary>
    /// Registration/identification number with tax authority.
    /// Example: VAT number, GST registration number, sales tax permit.
    /// </summary>
    public string? TaxRegistrationNumber { get; set; }

    /// <summary>
    /// Optional reporting category for tax filings and classification.
    /// Used to group taxes for reporting purposes.
    /// </summary>
    public string? ReportingCategory { get; set; }

    /// <summary>
    /// Whether the tax code is active and available for use.
    /// Default: true.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
