namespace FSH.Starter.Blazor.Client.Pages.Accounting.TaxCodes;

/// <summary>
/// ViewModel used by the Tax Codes page for add/edit operations.
/// Mirrors the shape of the API's CreateTaxCodeCommand so Mapster/Adapt can map between them.
/// </summary>
public class TaxCodeViewModel
{
    /// <summary>
    /// Primary identifier of the tax code.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique tax code identifier.
    /// Example: "VAT-STD", "SALES-CA", "GST", "PST-BC".
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Tax code name/description.
    /// Example: "Standard VAT", "California Sales Tax", "Federal GST".
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Tax type classification.
    /// Values: "SalesTax", "VAT", "GST", "UseTax", "Excise", "Withholding".
    /// </summary>
    public string TaxType { get; set; } = "SalesTax";

    /// <summary>
    /// Tax rate as decimal percentage.
    /// Example: 8.25 for 8.25%, 20.0 for 20% VAT.
    /// Stored as decimal between 0 and 100.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Whether this is a compound tax (calculated on subtotal plus other taxes).
    /// Default: false.
    /// </summary>
    public bool IsCompound { get; set; } = false;

    /// <summary>
    /// Jurisdiction or region this tax applies to.
    /// Example: "California", "United Kingdom", "Ontario", "Federal".
    /// </summary>
    public string? Jurisdiction { get; set; }

    /// <summary>
    /// Date when this tax rate becomes effective.
    /// Required field.
    /// </summary>
    public DateTime? EffectiveDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Optional date when this tax rate expires.
    /// Leave null for indefinite duration.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Whether the tax code is active and available for use.
    /// Default: true.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Account ID to record tax collected (liability account).
    /// Required for tracking tax remittance obligations.
    /// </summary>
    public DefaultIdType? TaxCollectedAccountId { get; set; }

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
    /// Reporting category for tax filings and classification.
    /// Used to group taxes for reporting purposes.
    /// </summary>
    public string? ReportingCategory { get; set; }

    /// <summary>
    /// Detailed description of the tax code, its applicability, and any special rules.
    /// </summary>
    public string? Description { get; set; }
}

