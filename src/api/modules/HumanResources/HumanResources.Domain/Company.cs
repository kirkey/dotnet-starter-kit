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
/// 
/// Default values:
/// - IsActive: true (new companies are active by default)
/// 
/// Business rules:
/// - CompanyCode must be unique within tenant
/// - Cannot delete company with active employees
/// - Name field from AuditableEntity contains the company name
/// </remarks>
public class Company : AuditableEntity, IAggregateRoot
{
    private Company() { }

    private Company(
        DefaultIdType id,
        string companyCode,
        string name,
        string? tin)
    {
        Id = id;
        CompanyCode = companyCode;
        Name = name;
        Tin = tin;
        IsActive = true;

        QueueDomainEvent(new CompanyCreated { Company = this });
    }

    /// <summary>
    /// Unique company code for identification.
    /// Example: "COMP-001", "EC-001", "BRANCH-02"
    /// </summary>
    public string CompanyCode { get; private set; } = default!;

    /// <summary>
    /// Tax Identification Number (TIN).
    /// Example: "123-456-789-000"
    /// </summary>
    public string? Tin { get; private set; }

    /// <summary>
    /// Complete company address.
    /// Example: "123 Main Street, Barangay Centro, Municipality"
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Postal or ZIP code.
    /// Example: "4400"
    /// </summary>
    public string? ZipCode { get; private set; }


    /// <summary>
    /// Primary phone number.
    /// Example: "+63-912-345-6789"
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
    /// Creates a new company with required information.
    /// </summary>
    public static Company Create(
        string companyCode,
        string name,
        string? tin = null)
    {
        return new Company(
            DefaultIdType.NewGuid(),
            companyCode,
            name,
            tin);
    }

    /// <summary>
    /// Updates company information.
    /// </summary>
    public Company Update(
        string? name,
        string? tin)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Tin, tin, StringComparison.OrdinalIgnoreCase))
        {
            Tin = tin;
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
        string? zipCode)
    {
        Address = address;
        ZipCode = zipCode;

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
    /// Updates the company logo URL.
    /// </summary>
    public Company UpdateLogo(string? logoUrl)
    {
        LogoUrl = logoUrl;
        QueueDomainEvent(new CompanyUpdated { Company = this });
        return this;
    }
}

