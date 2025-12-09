using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an insurance product offered by the MFI.
/// Includes loan protection, life insurance, crop insurance, and other microinsurance products.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define insurance products to protect members and loan portfolios.
/// - Configure premium calculation methods (flat, percentage-based, age-based).
/// - Set coverage amounts, terms, and exclusions.
/// - Link mandatory insurance to loan products for credit life coverage.
/// - Manage third-party insurance provider relationships.
/// 
/// Default values and constraints:
/// - Code: Unique product identifier.
/// - Name: Display name for the insurance product.
/// - ProductType: CreditLife, Crop, Health, Livestock, Property.
/// - PremiumMethod: Flat, PercentOfLoan, PercentOfCoverage, AgeBasedTable.
/// - CoverageAmount: Maximum coverage provided.
/// - PremiumRate: Rate for percentage-based calculations.
/// - IsActive: Whether product is currently offered.
/// 
/// Business rules:
/// - Essential for protecting vulnerable populations and MFI portfolios.
/// - Credit Life: Clears loan balance if borrower dies.
/// - Crop Insurance: Protects against crop failure for agricultural loans.
/// - Health Insurance: Reduces medical emergency default risk.
/// - MFIs often act as intermediaries collecting premiums.
/// - Some MFIs develop in-house programs with reserve funds.
/// </remarks>
/// <seealso cref="InsurancePolicy"/>
/// <seealso cref="InsuranceClaim"/>
/// <seealso cref="Loan"/>
/// <seealso cref="Member"/>
/// <example>
/// <para><strong>Example: Creating a credit life insurance product</strong></para>
/// <code>
/// POST /api/microfinance/insurance-products
/// {
///   "code": "CL-001",
///   "name": "Credit Life Protection",
///   "insuranceType": "LoanProtection",
///   "provider": "National Insurance Co.",
///   "premiumCalculationType": "PercentOfLoan",
///   "premiumRate": 0.5,
///   "coveragePercentage": 100,
///   "maxCoverageAmount": 10000000,
///   "waitingPeriodDays": 30,
///   "isMandatory": true,
///   "status": "Active"
/// }
/// </code>
/// </example>
public sealed class InsuranceProduct : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int Code = 32;
        public const int InsuranceType = 64;
        public const int Provider = 128;
        public const int TermsConditions = 8192;
    }

    /// <summary>
    /// Insurance type classification.
    /// </summary>
    public const string TypeLoanProtection = "LoanProtection";
    public const string TypeLifeInsurance = "LifeInsurance";
    public const string TypeHealthInsurance = "HealthInsurance";
    public const string TypeCropInsurance = "CropInsurance";
    public const string TypeLivestockInsurance = "LivestockInsurance";
    public const string TypePropertyInsurance = "PropertyInsurance";
    public const string TypeAccidentInsurance = "AccidentInsurance";
    public const string TypeGroupInsurance = "GroupInsurance";

    /// <summary>
    /// Premium calculation method.
    /// </summary>
    public const string PremiumFlat = "Flat";
    public const string PremiumPercentOfLoan = "PercentOfLoan";
    public const string PremiumPercentOfCoverage = "PercentOfCoverage";
    public const string PremiumAgeBasedTable = "AgeBasedTable";

    /// <summary>
    /// Status values.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusDiscontinued = "Discontinued";

    /// <summary>
    /// Unique product code.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Type of insurance.
    /// </summary>
    public string InsuranceType { get; private set; } = TypeLoanProtection;

    /// <summary>
    /// Insurance provider name.
    /// </summary>
    public string? Provider { get; private set; }

    /// <summary>
    /// Minimum coverage amount.
    /// </summary>
    public decimal MinCoverage { get; private set; }

    /// <summary>
    /// Maximum coverage amount.
    /// </summary>
    public decimal MaxCoverage { get; private set; }

    /// <summary>
    /// Premium calculation method.
    /// </summary>
    public string PremiumCalculation { get; private set; } = PremiumFlat;

    /// <summary>
    /// Premium rate (flat amount or percentage based on calculation method).
    /// </summary>
    public decimal PremiumRate { get; private set; }

    /// <summary>
    /// Premium rate table for age-based calculation (JSON).
    /// </summary>
    public string? PremiumRateTable { get; private set; }

    /// <summary>
    /// Minimum age for coverage.
    /// </summary>
    public int? MinAge { get; private set; }

    /// <summary>
    /// Maximum age for coverage.
    /// </summary>
    public int? MaxAge { get; private set; }

    /// <summary>
    /// Waiting period in days before claims can be filed.
    /// </summary>
    public int WaitingPeriodDays { get; private set; }

    /// <summary>
    /// Whether premium is collected upfront.
    /// </summary>
    public bool PremiumUpfront { get; private set; } = true;

    /// <summary>
    /// Whether insurance is mandatory with loans.
    /// </summary>
    public bool MandatoryWithLoan { get; private set; }

    /// <summary>
    /// Covered events in JSON format.
    /// </summary>
    public string? CoveredEvents { get; private set; }

    /// <summary>
    /// Exclusions in JSON format.
    /// </summary>
    public string? Exclusions { get; private set; }

    /// <summary>
    /// Terms and conditions.
    /// </summary>
    public string? TermsConditions { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    // Navigation properties
    public ICollection<InsurancePolicy> Policies { get; private set; } = new List<InsurancePolicy>();

    private InsuranceProduct() { }

    /// <summary>
    /// Creates a new insurance product.
    /// </summary>
    public static InsuranceProduct Create(
        string code,
        string name,
        string insuranceType,
        decimal minCoverage,
        decimal maxCoverage,
        string premiumCalculation,
        decimal premiumRate,
        string? description = null,
        string? provider = null,
        int waitingPeriodDays = 0,
        bool premiumUpfront = true,
        bool mandatoryWithLoan = false,
        int? minAge = null,
        int? maxAge = null)
    {
        var product = new InsuranceProduct
        {
            Code = code,
            InsuranceType = insuranceType,
            MinCoverage = minCoverage,
            MaxCoverage = maxCoverage,
            PremiumCalculation = premiumCalculation,
            PremiumRate = premiumRate,
            Provider = provider,
            WaitingPeriodDays = waitingPeriodDays,
            PremiumUpfront = premiumUpfront,
            MandatoryWithLoan = mandatoryWithLoan,
            MinAge = minAge,
            MaxAge = maxAge,
            Status = StatusActive
        };
        product.Name = name;
        product.Description = description;

        product.QueueDomainEvent(new InsuranceProductCreated(product));
        return product;
    }

    /// <summary>
    /// Updates product details.
    /// </summary>
    public InsuranceProduct Update(
        string? name,
        string? description,
        decimal? premiumRate,
        decimal? minCoverage,
        decimal? maxCoverage,
        int? waitingPeriodDays,
        string? coveredEvents,
        string? exclusions,
        string? termsConditions,
        string? notes)
    {
        if (name is not null) this.Name = name;
        if (description is not null) this.Description = description;
        if (premiumRate.HasValue) PremiumRate = premiumRate.Value;
        if (minCoverage.HasValue) MinCoverage = minCoverage.Value;
        if (maxCoverage.HasValue) MaxCoverage = maxCoverage.Value;
        if (waitingPeriodDays.HasValue) WaitingPeriodDays = waitingPeriodDays.Value;
        if (coveredEvents is not null) CoveredEvents = coveredEvents;
        if (exclusions is not null) Exclusions = exclusions;
        if (termsConditions is not null) TermsConditions = termsConditions;
        if (notes is not null) this.Notes = notes;

        QueueDomainEvent(new InsuranceProductUpdated(this));
        return this;
    }

    /// <summary>
    /// Activates the product.
    /// </summary>
    public void Activate()
    {
        if (Status == StatusActive) return;
        Status = StatusActive;
        QueueDomainEvent(new InsuranceProductActivated(Id));
    }

    /// <summary>
    /// Deactivates the product.
    /// </summary>
    public void Deactivate()
    {
        Status = StatusInactive;
        QueueDomainEvent(new InsuranceProductDeactivated(Id));
    }

    /// <summary>
    /// Discontinues the product.
    /// </summary>
    public void Discontinue()
    {
        Status = StatusDiscontinued;
        QueueDomainEvent(new InsuranceProductDiscontinued(Id));
    }

    /// <summary>
    /// Calculates premium for a given coverage amount.
    /// </summary>
    public decimal CalculatePremium(decimal coverageAmount, int? age = null)
    {
        return PremiumCalculation switch
        {
            PremiumFlat => PremiumRate,
            PremiumPercentOfCoverage => coverageAmount * (PremiumRate / 100),
            PremiumPercentOfLoan => coverageAmount * (PremiumRate / 100),
            _ => PremiumRate
        };
    }
}
