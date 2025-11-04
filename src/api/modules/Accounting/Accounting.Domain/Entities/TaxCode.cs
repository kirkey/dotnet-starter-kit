using Accounting.Domain.Events.TaxCode;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a tax code configuration for sales tax, VAT, GST and other tax calculations.
/// </summary>
/// <remarks>
/// Use cases:
/// - Configure tax rates for different jurisdictions and tax types.
/// - Support multiple tax components (state, county, city, district taxes).
/// - Calculate tax amounts on invoices, bills, and other taxable transactions.
/// - Track tax collected and tax paid for reporting and remittance.
/// - Enable automatic tax calculation based on transaction types and locations.
/// - Support compound taxes and tax-on-tax scenarios.
/// - Maintain historical tax rates with effective date tracking.
/// 
/// Default values:
/// - TaxType: Sales Tax (most common in general accounting)
/// - Rate: required (decimal percentage, e.g., 0.0825 for 8.25%)
/// - IsActive: true (tax code is active and available)
/// - IsCompound: false (tax is not calculated on other taxes)
/// - EffectiveDate: required (when this rate becomes active)
/// - ExpiryDate: null (rate continues indefinitely)
/// - TaxCollectedAccountId: required (liability account for tax collected)
/// - TaxPaidAccountId: null (expense account for tax paid, for purchase tax)
/// 
/// Business rules:
/// - Tax rate must be between 0 and 1 (0% to 100%)
/// - Effective date cannot be in the past when creating new rates
/// - Cannot modify tax code once transactions have used it
/// - Compound taxes calculate on subtotal plus other non-compound taxes
/// - Must specify collection account for remittance tracking
/// - Can have multiple rates for same tax code (different effective dates)
/// - Jurisdiction determines applicability of tax code
/// </remarks>
public class TaxCode : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique tax code identifier (e.g., "VAT-STD", "SALES-CA", "GST").
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    // Name property inherited from AuditableEntity base class

    /// <summary>
    /// Tax type: SalesTax, VAT, GST, UseTax, Excise, Withholding.
    /// </summary>
    public TaxType TaxType { get; private set; }

    /// <summary>
    /// Tax rate as decimal (e.g., 0.0825 for 8.25%).
    /// </summary>
    public decimal Rate { get; private set; }

    /// <summary>
    /// Whether this tax is calculated on the subtotal plus other taxes (compound tax).
    /// </summary>
    public bool IsCompound { get; private set; }

    /// <summary>
    /// Jurisdiction or region this tax applies to (e.g., "California", "UK", "Ontario").
    /// </summary>
    public string? Jurisdiction { get; private set; }

    /// <summary>
    /// Date when this tax rate becomes effective.
    /// </summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>
    /// Optional date when this tax rate expires.
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }

    /// <summary>
    /// Whether the tax code is active and available for use.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Account to record tax collected (liability account).
    /// </summary>
    public DefaultIdType TaxCollectedAccountId { get; private set; }

    /// <summary>
    /// Optional account to record tax paid on purchases (expense/asset account).
    /// </summary>
    public DefaultIdType? TaxPaidAccountId { get; private set; }

    /// <summary>
    /// Tax authority or agency to remit collected taxes to.
    /// </summary>
    public string? TaxAuthority { get; private set; }

    /// <summary>
    /// Registration/identification number with tax authority.
    /// </summary>
    public string? TaxRegistrationNumber { get; private set; }

    /// <summary>
    /// Optional reporting category for tax returns.
    /// </summary>
    public string? ReportingCategory { get; private set; }

    private TaxCode(
        string code,
        string name,
        TaxType taxType,
        decimal rate,
        DefaultIdType taxCollectedAccountId,
        DateTime effectiveDate,
        bool isCompound = false,
        string? jurisdiction = null,
        DateTime? expiryDate = null,
        DefaultIdType? taxPaidAccountId = null,
        string? taxAuthority = null,
        string? taxRegistrationNumber = null,
        string? reportingCategory = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Tax code is required", nameof(code));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tax name is required", nameof(name));

        if (rate < 0 || rate > 1)
            throw new ArgumentException("Tax rate must be between 0 and 1 (0% to 100%)", nameof(rate));

        if (expiryDate.HasValue && expiryDate.Value < effectiveDate)
            throw new ArgumentException("Expiry date cannot be before effective date", nameof(expiryDate));

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        TaxType = taxType;
        Rate = rate;
        IsCompound = isCompound;
        Jurisdiction = jurisdiction?.Trim();
        EffectiveDate = effectiveDate;
        ExpiryDate = expiryDate;
        IsActive = true;
        TaxCollectedAccountId = taxCollectedAccountId;
        TaxPaidAccountId = taxPaidAccountId;
        TaxAuthority = taxAuthority?.Trim();
        TaxRegistrationNumber = taxRegistrationNumber?.Trim();
        ReportingCategory = reportingCategory?.Trim();
        Description = description?.Trim();

        QueueDomainEvent(new TaxCodeCreated(Id, Code, Name, TaxType, Rate, EffectiveDate));
    }

    /// <summary>
    /// Create a new tax code configuration.
    /// </summary>
    public static TaxCode Create(
        string code,
        string name,
        TaxType taxType,
        decimal rate,
        DefaultIdType taxCollectedAccountId,
        DateTime effectiveDate,
        bool isCompound = false,
        string? jurisdiction = null,
        DateTime? expiryDate = null,
        DefaultIdType? taxPaidAccountId = null,
        string? taxAuthority = null,
        string? taxRegistrationNumber = null,
        string? reportingCategory = null,
        string? description = null)
    {
        return new TaxCode(code, name, taxType, rate, taxCollectedAccountId, effectiveDate,
            isCompound, jurisdiction, expiryDate, taxPaidAccountId, taxAuthority,
            taxRegistrationNumber, reportingCategory, description);
    }

    /// <summary>
    /// Update tax code details.
    /// </summary>
    public void Update(
        string? name = null,
        string? jurisdiction = null,
        string? taxAuthority = null,
        string? taxRegistrationNumber = null,
        string? reportingCategory = null,
        string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();

        Jurisdiction = jurisdiction?.Trim();
        TaxAuthority = taxAuthority?.Trim();
        TaxRegistrationNumber = taxRegistrationNumber?.Trim();
        ReportingCategory = reportingCategory?.Trim();
        Description = description?.Trim();

        QueueDomainEvent(new TaxCodeUpdated(Id, Name));
    }

    /// <summary>
    /// Update the tax rate (creates new effective rate).
    /// </summary>
    public void UpdateRate(decimal newRate, DateTime effectiveDate)
    {
        if (newRate < 0 || newRate > 1)
            throw new ArgumentException("Tax rate must be between 0 and 1 (0% to 100%)", nameof(newRate));

        if (effectiveDate < DateTime.UtcNow.Date)
            throw new ArgumentException("Effective date cannot be in the past", nameof(effectiveDate));

        Rate = newRate;
        EffectiveDate = effectiveDate;

        QueueDomainEvent(new TaxCodeRateUpdated(Id, newRate, effectiveDate));
    }

    /// <summary>
    /// Activate the tax code.
    /// </summary>
    public void Activate()
    {
        if (IsActive)
            throw new TaxCodeAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new TaxCodeActivated(Id));
    }

    /// <summary>
    /// Deactivate the tax code.
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
            throw new TaxCodeAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new TaxCodeDeactivated(Id));
    }

    /// <summary>
    /// Calculate tax amount for a given base amount.
    /// </summary>
    public decimal CalculateTax(decimal baseAmount)
    {
        if (!IsActive)
            throw new TaxCodeInactiveException(Id);

        if (baseAmount < 0)
            throw new ArgumentException("Base amount cannot be negative", nameof(baseAmount));

        return Math.Round(baseAmount * Rate, 2);
    }
}

/// <summary>
/// Tax type classifications.
/// </summary>
public enum TaxType
{
    SalesTax,
    VAT,
    GST,
    UseTax,
    Excise,
    Withholding,
    Property,
    Other
}
