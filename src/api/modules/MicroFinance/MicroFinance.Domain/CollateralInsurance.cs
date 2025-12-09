using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents insurance coverage for collateral.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
/// <item>Track insurance policies protecting pledged collateral assets</item>
/// <item>Monitor policy expiration and trigger renewal alerts</item>
/// <item>File insurance claims when collateral is damaged or lost</item>
/// <item>Ensure adequate coverage relative to loan outstanding balance</item>
/// <item>Verify third-party insurance provider credentials and ratings</item>
/// <item>Calculate premium allocation to borrower's loan costs</item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Collateral insurance protects the MFI's secured loan portfolio against asset loss.
/// Policies may cover fire, theft, flood, comprehensive damage, or all-risk scenarios.
/// Insurance requirements are often mandatory for vehicle, property, and equipment loans.
/// The MFI may require assignment of policy benefits to ensure claim proceeds cover
/// outstanding loan amounts. Regular coverage reviews ensure policy values match
/// current collateral valuations.
/// </para>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
/// <item><see cref="LoanCollateral"/> - The insured collateral asset</item>
/// <item><see cref="Loan"/> - Associated loan requiring insurance</item>
/// <item><see cref="CollateralValuation"/> - Current asset value for coverage adequacy</item>
/// <item><see cref="InsuranceClaim"/> - Claims filed against the policy</item>
/// </list>
/// </remarks>
public sealed class CollateralInsurance : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int PolicyNumberMaxLength = 64;
    public const int InsurerMaxLength = 128;
    public const int TypeMaxLength = 64;
    public const int StatusMaxLength = 32;
    public const int ContactMaxLength = 128;
    public const int NotesMaxLength = 512;
    
    // Insurance Status
    public const string StatusActive = "Active";
    public const string StatusExpired = "Expired";
    public const string StatusCancelled = "Cancelled";
    public const string StatusPendingRenewal = "PendingRenewal";
    public const string StatusClaimFiled = "ClaimFiled";
    
    // Insurance Types
    public const string TypeComprehensive = "Comprehensive";
    public const string TypeFire = "Fire";
    public const string TypeTheft = "Theft";
    public const string TypeFlood = "Flood";
    public const string TypeAllRisk = "AllRisk";
    public const string TypeProperty = "Property";
    public const string TypeVehicle = "Vehicle";

    public Guid CollateralId { get; private set; }
    public string PolicyNumber { get; private set; } = default!;
    public string InsurerName { get; private set; } = default!;
    public string InsuranceType { get; private set; } = default!;
    public string Status { get; private set; } = StatusActive;
    public decimal CoverageAmount { get; private set; }
    public decimal PremiumAmount { get; private set; }
    public decimal Deductible { get; private set; }
    public DateOnly EffectiveDate { get; private set; }
    public DateOnly ExpiryDate { get; private set; }
    public DateOnly? RenewalDate { get; private set; }
    public string? InsurerContact { get; private set; }
    public string? InsurerPhone { get; private set; }
    public string? InsurerEmail { get; private set; }
    public bool IsMfiAsBeneficiary { get; private set; }
    public string? BeneficiaryName { get; private set; }
    public string? PolicyDocumentPath { get; private set; }
    public DateOnly? LastPremiumPaidDate { get; private set; }
    public DateOnly? NextPremiumDueDate { get; private set; }
    public string? Notes { get; private set; }
    public int RenewalReminderDays { get; private set; } = 30;
    public bool AutoRenewal { get; private set; }

    private CollateralInsurance() { }

    public static CollateralInsurance Create(
        Guid collateralId,
        string policyNumber,
        string insurerName,
        string insuranceType,
        decimal coverageAmount,
        decimal premiumAmount,
        decimal deductible,
        DateOnly effectiveDate,
        DateOnly expiryDate,
        bool isMfiAsBeneficiary = true)
    {
        var insurance = new CollateralInsurance
        {
            CollateralId = collateralId,
            PolicyNumber = policyNumber,
            InsurerName = insurerName,
            InsuranceType = insuranceType,
            CoverageAmount = coverageAmount,
            PremiumAmount = premiumAmount,
            Deductible = deductible,
            EffectiveDate = effectiveDate,
            ExpiryDate = expiryDate,
            IsMfiAsBeneficiary = isMfiAsBeneficiary,
            Status = StatusActive
        };

        insurance.QueueDomainEvent(new CollateralInsuranceCreated(insurance));
        return insurance;
    }

    public CollateralInsurance RecordPremiumPayment(DateOnly paymentDate, DateOnly nextDueDate)
    {
        LastPremiumPaidDate = paymentDate;
        NextPremiumDueDate = nextDueDate;
        QueueDomainEvent(new CollateralInsurancePremiumPaid(Id, CollateralId, PremiumAmount));
        return this;
    }

    public CollateralInsurance Renew(DateOnly newExpiryDate, decimal? newPremium = null)
    {
        ExpiryDate = newExpiryDate;
        RenewalDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (newPremium.HasValue) PremiumAmount = newPremium.Value;
        Status = StatusActive;
        QueueDomainEvent(new CollateralInsuranceRenewed(Id, CollateralId, newExpiryDate));
        return this;
    }

    public CollateralInsurance MarkPendingRenewal()
    {
        Status = StatusPendingRenewal;
        QueueDomainEvent(new CollateralInsuranceRenewalDue(Id, CollateralId, ExpiryDate));
        return this;
    }

    public CollateralInsurance Expire()
    {
        Status = StatusExpired;
        QueueDomainEvent(new CollateralInsuranceExpired(Id, CollateralId));
        return this;
    }

    public CollateralInsurance Cancel(string reason)
    {
        Status = StatusCancelled;
        Notes = reason;
        return this;
    }

    public CollateralInsurance FileClaim()
    {
        Status = StatusClaimFiled;
        QueueDomainEvent(new CollateralInsuranceClaimFiled(Id, CollateralId));
        return this;
    }

    public CollateralInsurance Update(
        string? insurerContact = null,
        string? insurerPhone = null,
        string? insurerEmail = null,
        string? beneficiaryName = null,
        string? policyDocumentPath = null,
        string? notes = null,
        int? renewalReminderDays = null,
        bool? autoRenewal = null)
    {
        if (insurerContact is not null) InsurerContact = insurerContact;
        if (insurerPhone is not null) InsurerPhone = insurerPhone;
        if (insurerEmail is not null) InsurerEmail = insurerEmail;
        if (beneficiaryName is not null) BeneficiaryName = beneficiaryName;
        if (policyDocumentPath is not null) PolicyDocumentPath = policyDocumentPath;
        if (notes is not null) Notes = notes;
        if (renewalReminderDays.HasValue) RenewalReminderDays = renewalReminderDays.Value;
        if (autoRenewal.HasValue) AutoRenewal = autoRenewal.Value;

        QueueDomainEvent(new CollateralInsuranceUpdated(this));
        return this;
    }
}
