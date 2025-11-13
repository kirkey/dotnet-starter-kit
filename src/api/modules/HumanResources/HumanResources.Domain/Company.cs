using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain;

/// <summary>
/// Represents a company or legal entity for multi-entity support.
/// </summary>
/// <remarks>
/// Use cases:
/// - Multi-entity accounting and consolidation
/// - Separate legal entities per tenant
/// - Enterprise customer support with multiple companies
/// - Holding company structures
/// 
/// Default values:
/// - IsActive: true (new companies are active by default)
/// - BaseCurrency: "USD" (default currency)
/// - FiscalYearEnd: 12 (December)
/// 
/// Business rules:
/// - CompanyCode must be unique within tenant
/// - BaseCurrency is required for multi-currency support
/// - Cannot delete company with active employees
/// - Parent company must exist if specified
/// </remarks>
public class Company : AuditableEntity, IAggregateRoot
{
    private Company() { }

    private Company(
        DefaultIdType id,
        string companyCode,
        string legalName,
        string? tradeName,
        string? taxId,
        string baseCurrency,
        int fiscalYearEnd,
        string? description,
        string? notes)
    {
        Id = id;
        CompanyCode = companyCode;
        LegalName = legalName;
        TradeName = tradeName;
        TaxId = taxId;
        BaseCurrency = baseCurrency;
        FiscalYearEnd = fiscalYearEnd;
        Description = description;
        Notes = notes;
        IsActive = true;

        QueueDomainEvent(new CompanyCreated { Company = this });
    }

    /// <summary>
    /// Unique company code for identification.
    /// Example: "COMP-001", "US-EAST", "EMEA-HQ"
    /// </summary>
    public string CompanyCode { get; private set; } = default!;

    /// <summary>
    /// Official registered legal name of the company.
    /// Example: "ABC Corporation Inc.", "XYZ Limited"
    /// </summary>
    public string LegalName { get; private set; } = default!;

    /// <summary>
    /// Trading name or "doing business as" name.
    /// Example: "ABC Corp" (when legal name is "ABC Corporation Inc.")
    /// </summary>
    public string? TradeName { get; private set; }

    /// <summary>
    /// Tax identification number (EIN, VAT, etc.).
    /// Example: "12-3456789" (US EIN)
    /// </summary>
    public string? TaxId { get; private set; }

    /// <summary>
    /// Base currency code for this company (ISO 4217).
    /// Example: "USD", "EUR", "GBP"
    /// </summary>
    public string BaseCurrency { get; private set; } = "USD";

    /// <summary>
    /// Fiscal year end month (1-12).
    /// Example: 12 for December 31 year-end, 6 for June 30
    /// </summary>
    public int FiscalYearEnd { get; private set; } = 12;

    /// <summary>
    /// Company address line 1.
    /// Example: "123 Main Street"
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// City name.
    /// Example: "New York"
    /// </summary>
    public string? City { get; private set; }

    /// <summary>
    /// State or province.
    /// Example: "NY", "California"
    /// </summary>
    public string? State { get; private set; }

    /// <summary>
    /// Postal or ZIP code.
    /// Example: "10001", "SW1A 1AA"
    /// </summary>
    public string? ZipCode { get; private set; }

    /// <summary>
    /// Country name or code.
    /// Example: "United States", "US"
    /// </summary>
    public string? Country { get; private set; }

    /// <summary>
    /// Primary phone number.
    /// Example: "+1-555-123-4567"
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Primary email address.
    /// Example: "info@company.com"
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Company website URL.
    /// Example: "https://www.company.com"
    /// </summary>
    public string? Website { get; private set; }

    /// <summary>
    /// URL to company logo.
    /// Example: "/files/logos/company-logo.png"
    /// </summary>
    public string? LogoUrl { get; private set; }

    /// <summary>
    /// Whether the company is active and operational.
    /// Default: true
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Parent company ID for holding company structures.
    /// Null if this is a top-level company.
    /// </summary>
    public DefaultIdType? ParentCompanyId { get; private set; }

    /// <summary>
    /// Creates a new company with required information.
    /// </summary>
    public static Company Create(
        string companyCode,
        string legalName,
        string? tradeName,
        string? taxId,
        string baseCurrency,
        int fiscalYearEnd,
        string? description = null,
        string? notes = null)
    {
        return new Company(
            DefaultIdType.NewGuid(),
            companyCode,
            legalName,
            tradeName,
            taxId,
            baseCurrency,
            fiscalYearEnd,
            description,
            notes);
    }

    /// <summary>
    /// Updates company information.
    /// </summary>
    public Company Update(
        string? legalName,
        string? tradeName,
        string? taxId,
        string? baseCurrency,
        int? fiscalYearEnd,
        string? description,
        string? notes)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(legalName) && !string.Equals(LegalName, legalName, StringComparison.OrdinalIgnoreCase))
        {
            LegalName = legalName;
            isUpdated = true;
        }

        if (!string.Equals(TradeName, tradeName, StringComparison.OrdinalIgnoreCase))
        {
            TradeName = tradeName;
            isUpdated = true;
        }

        if (!string.Equals(TaxId, taxId, StringComparison.OrdinalIgnoreCase))
        {
            TaxId = taxId;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(baseCurrency) && !string.Equals(BaseCurrency, baseCurrency, StringComparison.OrdinalIgnoreCase))
        {
            BaseCurrency = baseCurrency;
            isUpdated = true;
        }

        if (fiscalYearEnd.HasValue && fiscalYearEnd.Value != FiscalYearEnd)
        {
            FiscalYearEnd = fiscalYearEnd.Value;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CompanyUpdated { Company = this });
        }

        return this;
    }

    /// <summary>
    /// Updates company address information.
    /// </summary>
    public Company UpdateAddress(
        string? address,
        string? city,
        string? state,
        string? zipCode,
        string? country)
    {
        Address = address;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;

        QueueDomainEvent(new CompanyUpdated { Company = this });
        return this;
    }

    /// <summary>
    /// Updates company contact information.
    /// </summary>
    public Company UpdateContact(
        string? phone,
        string? email,
        string? website)
    {
        Phone = phone;
        Email = email;
        Website = website;

        QueueDomainEvent(new CompanyUpdated { Company = this });
        return this;
    }

    /// <summary>
    /// Activates the company.
    /// </summary>
    public Company Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new CompanyActivated { CompanyId = Id });
        }
        return this;
    }

    /// <summary>
    /// Deactivates the company.
    /// </summary>
    public Company Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new CompanyDeactivated { CompanyId = Id });
        }
        return this;
    }

    /// <summary>
    /// Sets the parent company for holding company structures.
    /// </summary>
    public Company SetParentCompany(DefaultIdType? parentCompanyId)
    {
        if (ParentCompanyId != parentCompanyId)
        {
            ParentCompanyId = parentCompanyId;
            QueueDomainEvent(new CompanyUpdated { Company = this });
        }
        return this;
    }

    /// <summary>
    /// Updates the company logo URL.
    /// </summary>
    public Company UpdateLogo(string? logoUrl)
    {
        LogoUrl = logoUrl;
        QueueDomainEvent(new CompanyUpdated { Company = this });
        return this;
    }
}

