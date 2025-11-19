namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a tax master configuration for various tax types.
/// Defines tax codes, rates, jurisdictions, and accounting impacts.
/// Used for sales tax (VAT, GST), excise taxes, withholding taxes, and other tax types.
/// </summary>
public class TaxMaster : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Private parameterless constructor for EF Core.
    /// </summary>
    private TaxMaster() { }

    /// <summary>
    /// Private constructor used by factory methods.
    /// </summary>
    private TaxMaster(
        string code,
        string name,
        string taxType,
        decimal rate,
        DefaultIdType taxCollectedAccountId)
    {
        Code = code;
        Name = name;
        TaxType = taxType;
        Rate = rate;
        TaxCollectedAccountId = taxCollectedAccountId;
        IsActive = true;
        EffectiveDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique tax code identifier (e.g., "VAT-STD", "SALES-CA", "EXCISE-FUEL").
    /// Must be uppercase alphanumeric with hyphens/underscores only.
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Gets the descriptive name of the tax.
    /// Example: "Standard VAT Rate", "California Sales Tax", "Excise Tax - Fuel".
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// Gets the tax type classification.
    /// Valid values: SalesTax, VAT, GST, UseTax, Excise, Withholding, Property, Other.
    /// </summary>
    public string TaxType { get; private set; } = default!;

    /// <summary>
    /// Gets the tax rate as a decimal (e.g., 0.0825 for 8.25%).
    /// Must be between 0 and 1 (0% to 100%).
    /// </summary>
    public decimal Rate { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this tax is calculated on the subtotal plus other taxes (compound tax).
    /// Default: false. When true, tax is calculated on the amount including other taxes.
    /// </summary>
    public bool IsCompound { get; private set; }

    /// <summary>
    /// Gets the jurisdiction or region this tax applies to.
    /// Example: "California", "UK", "Ontario", "Federal", "State".
    /// Optional but recommended for multi-jurisdiction operations.
    /// </summary>
    public string? Jurisdiction { get; private set; }

    /// <summary>
    /// Gets the date when this tax rate becomes effective.
    /// Supports temporal tax rate changes (e.g., VAT rate increase on specific date).
    /// </summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>
    /// Gets the optional date when this tax rate expires.
    /// When set, the tax becomes inactive after this date.
    /// Must be after the effective date if specified.
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }

    /// <summary>
    /// Gets the general ledger account ID to record tax collected (liability account).
    /// Required for tracking tax remittance obligations.
    /// Example: Account 2300 - Sales Tax Payable.
    /// </summary>
    public DefaultIdType TaxCollectedAccountId { get; private set; }

    /// <summary>
    /// Gets the optional general ledger account ID to record tax paid on purchases.
    /// Used for input tax credits or tax recovery in VAT/GST systems.
    /// Example: Account 1205 - Sales Tax Paid on Purchases.
    /// </summary>
    public DefaultIdType? TaxPaidAccountId { get; private set; }

    /// <summary>
    /// Gets the tax authority or agency to remit collected taxes to.
    /// Example: "IRS", "HMRC", "CRA", "State Board of Equalization".
    /// </summary>
    public string? TaxAuthority { get; private set; }

    /// <summary>
    /// Gets the registration or identification number with the tax authority.
    /// Example: VAT number, GST registration number, sales tax permit.
    /// </summary>
    public string? TaxRegistrationNumber { get; private set; }

    /// <summary>
    /// Gets the reporting category for tax filings and classification.
    /// Used to group taxes for reporting purposes (e.g., "Federal", "State", "Local").
    /// </summary>
    public string? ReportingCategory { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the tax code is active and available for use.
    /// Default: true. When false, the tax cannot be applied to new transactions.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new tax master configuration.
    /// </summary>
    /// <param name="code">Unique tax code identifier.</param>
    /// <param name="name">Descriptive tax name.</param>
    /// <param name="taxType">Type of tax (SalesTax, VAT, GST, etc.).</param>
    /// <param name="rate">Tax rate as decimal (0-1).</param>
    /// <param name="taxCollectedAccountId">GL account for tax collected (liability).</param>
    /// <param name="effectiveDate">Date when tax becomes effective. Defaults to now.</param>
    /// <param name="isCompound">Whether tax is compounded on other taxes. Defaults to false.</param>
    /// <param name="jurisdiction">Geographic jurisdiction. Optional.</param>
    /// <param name="expiryDate">Date when tax expires. Optional, must be after effective date.</param>
    /// <param name="taxPaidAccountId">GL account for tax paid on purchases. Optional.</param>
    /// <param name="taxAuthority">Tax authority to remit to. Optional.</param>
    /// <param name="taxRegistrationNumber">Tax registration number. Optional.</param>
    /// <param name="reportingCategory">Category for reporting. Optional.</param>
    /// <returns>New TaxMaster instance.</returns>
    /// <exception cref="ArgumentException">Thrown when code or name is empty, rate is invalid, or dates are invalid.</exception>
    public static TaxMaster Create(
        string code,
        string name,
        string taxType,
        decimal rate,
        DefaultIdType taxCollectedAccountId,
        DateTime? effectiveDate = null,
        bool isCompound = false,
        string? jurisdiction = null,
        DateTime? expiryDate = null,
        DefaultIdType? taxPaidAccountId = null,
        string? taxAuthority = null,
        string? taxRegistrationNumber = null,
        string? reportingCategory = null)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Tax code cannot be empty", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tax name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(taxType))
            throw new ArgumentException("Tax type cannot be empty", nameof(taxType));
        if (rate < 0 || rate > 1)
            throw new ArgumentException("Tax rate must be between 0 and 1", nameof(rate));
        
        var effectiveDateValue = effectiveDate ?? DateTime.UtcNow;
        if (expiryDate.HasValue && expiryDate <= effectiveDateValue)
            throw new ArgumentException("Expiry date must be after effective date", nameof(expiryDate));

        var tax = new TaxMaster(code, name, taxType, rate, taxCollectedAccountId)
        {
            IsCompound = isCompound,
            Jurisdiction = jurisdiction,
            EffectiveDate = effectiveDateValue,
            ExpiryDate = expiryDate,
            TaxPaidAccountId = taxPaidAccountId,
            TaxAuthority = taxAuthority,
            TaxRegistrationNumber = taxRegistrationNumber,
            ReportingCategory = reportingCategory
        };

        return tax;
    }

    /// <summary>
    /// Updates the tax master configuration.
    /// Only non-null parameters are updated, allowing partial updates.
    /// </summary>
    /// <param name="name">New tax name.</param>
    /// <param name="taxType">New tax type.</param>
    /// <param name="rate">New tax rate.</param>
    /// <param name="isCompound">New compound flag.</param>
    /// <param name="jurisdiction">New jurisdiction.</param>
    /// <param name="expiryDate">New expiry date.</param>
    /// <param name="taxPaidAccountId">New tax paid account ID.</param>
    /// <param name="taxAuthority">New tax authority.</param>
    /// <param name="taxRegistrationNumber">New tax registration number.</param>
    /// <param name="reportingCategory">New reporting category.</param>
    /// <returns>This instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when updated rate is invalid or dates are invalid.</exception>
    public TaxMaster Update(
        string? name = null,
        string? taxType = null,
        decimal? rate = null,
        bool? isCompound = null,
        string? jurisdiction = null,
        DateTime? expiryDate = null,
        DefaultIdType? taxPaidAccountId = null,
        string? taxAuthority = null,
        string? taxRegistrationNumber = null,
        string? reportingCategory = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;
        
        if (!string.IsNullOrWhiteSpace(taxType))
            TaxType = taxType;
        
        if (rate.HasValue)
        {
            if (rate < 0 || rate > 1)
                throw new ArgumentException("Tax rate must be between 0 and 1", nameof(rate));
            Rate = rate.Value;
        }
        
        if (isCompound.HasValue)
            IsCompound = isCompound.Value;
        
        if (jurisdiction != null)
            Jurisdiction = jurisdiction;
        
        if (expiryDate.HasValue)
        {
            if (expiryDate <= EffectiveDate)
                throw new ArgumentException("Expiry date must be after effective date", nameof(expiryDate));
            ExpiryDate = expiryDate;
        }
        
        if (taxPaidAccountId.HasValue)
            TaxPaidAccountId = taxPaidAccountId;
        
        if (taxAuthority != null)
            TaxAuthority = taxAuthority;
        
        if (taxRegistrationNumber != null)
            TaxRegistrationNumber = taxRegistrationNumber;
        
        if (reportingCategory != null)
            ReportingCategory = reportingCategory;

        return this;
    }

    /// <summary>
    /// Activates the tax, making it available for use in transactions.
    /// </summary>
    public void Activate() => IsActive = true;

    /// <summary>
    /// Deactivates the tax, preventing it from being used in new transactions.
    /// </summary>
    public void Deactivate() => IsActive = false;
}

