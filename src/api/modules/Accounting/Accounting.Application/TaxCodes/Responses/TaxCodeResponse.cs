namespace Accounting.Application.TaxCodes.Responses;

/// <summary>
/// Response DTO for tax code queries.
/// Contains all tax code information including rate configuration, jurisdiction, and audit fields.
/// </summary>
public class TaxCodeResponse
{
    /// <summary>
    /// Unique identifier for the tax code.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique tax code identifier (e.g., "VAT-STD", "SALES-CA", "GST").
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Tax code name/description (e.g., "Standard VAT", "California Sales Tax").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Tax type classification (SalesTax, VAT, GST, UseTax, Excise, Withholding, Property, Other).
    /// </summary>
    public string TaxType { get; set; } = string.Empty;

    /// <summary>
    /// Tax rate as decimal (e.g., 0.0825 for 8.25%).
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Whether this tax is calculated on the subtotal plus other taxes (compound tax).
    /// </summary>
    public bool IsCompound { get; set; }

    /// <summary>
    /// Jurisdiction or region this tax applies to (e.g., "California", "UK", "Ontario").
    /// </summary>
    public string? Jurisdiction { get; set; }

    /// <summary>
    /// Date when this tax rate becomes effective.
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Optional date when this tax rate expires.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Whether the tax code is active and available for use.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Account ID to record tax collected (liability account).
    /// </summary>
    public DefaultIdType TaxCollectedAccountId { get; set; }

    /// <summary>
    /// Optional account ID to record tax paid on purchases (expense/asset account).
    /// </summary>
    public DefaultIdType? TaxPaidAccountId { get; set; }

    /// <summary>
    /// Tax authority or agency to remit collected taxes to.
    /// </summary>
    public string? TaxAuthority { get; set; }

    /// <summary>
    /// Registration/identification number with tax authority.
    /// </summary>
    public string? TaxRegistrationNumber { get; set; }

    /// <summary>
    /// Optional reporting category for tax returns.
    /// </summary>
    public string? ReportingCategory { get; set; }

    /// <summary>
    /// Optional description or usage notes.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Timestamp when the tax code was created.
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// User ID who created the tax code.
    /// </summary>
    public DefaultIdType? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the tax code was last modified.
    /// </summary>
    public DateTimeOffset? LastModifiedOn { get; set; }

    /// <summary>
    /// User ID who last modified the tax code.
    /// </summary>
    public DefaultIdType? LastModifiedBy { get; set; }
}
