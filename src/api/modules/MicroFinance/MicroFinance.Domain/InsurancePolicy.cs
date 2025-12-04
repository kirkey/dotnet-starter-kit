using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an insurance policy issued to a member.
/// Tracks coverage, premiums, and policy lifecycle.
/// </summary>
public sealed class InsurancePolicy : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int PolicyNumber = 32;
        public const int BeneficiaryName = 128;
        public const int BeneficiaryRelation = 64;
        public const int BeneficiaryContact = 64;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Policy status values.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusLapsed = "Lapsed";
    public const string StatusCancelled = "Cancelled";
    public const string StatusExpired = "Expired";
    public const string StatusClaimed = "Claimed";
    public const string StatusMatured = "Matured";

    /// <summary>
    /// Reference to the insurance product.
    /// </summary>
    public Guid InsuranceProductId { get; private set; }

    /// <summary>
    /// Reference to the member (policyholder).
    /// </summary>
    public Guid MemberId { get; private set; }

    /// <summary>
    /// Reference to associated loan (if loan protection).
    /// </summary>
    public Guid? LoanId { get; private set; }

    /// <summary>
    /// Unique policy number.
    /// </summary>
    public string PolicyNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Policy start date.
    /// </summary>
    public DateOnly StartDate { get; private set; }

    /// <summary>
    /// Policy end date.
    /// </summary>
    public DateOnly EndDate { get; private set; }

    /// <summary>
    /// Coverage amount.
    /// </summary>
    public decimal CoverageAmount { get; private set; }

    /// <summary>
    /// Premium amount.
    /// </summary>
    public decimal PremiumAmount { get; private set; }

    /// <summary>
    /// Total premium paid.
    /// </summary>
    public decimal TotalPremiumPaid { get; private set; }

    /// <summary>
    /// Premium due date (for periodic premiums).
    /// </summary>
    public DateOnly? NextPremiumDue { get; private set; }

    /// <summary>
    /// Name of primary beneficiary.
    /// </summary>
    public string? BeneficiaryName { get; private set; }

    /// <summary>
    /// Relationship to policyholder.
    /// </summary>
    public string? BeneficiaryRelation { get; private set; }

    /// <summary>
    /// Beneficiary contact information.
    /// </summary>
    public string? BeneficiaryContact { get; private set; }

    /// <summary>
    /// End of waiting period date.
    /// </summary>
    public DateOnly? WaitingPeriodEnd { get; private set; }

    /// <summary>
    /// Whether policy is in waiting period.
    /// </summary>
    public bool IsInWaitingPeriod => WaitingPeriodEnd.HasValue && DateOnly.FromDateTime(DateTime.UtcNow) < WaitingPeriodEnd.Value;

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>
    /// Date policy was cancelled (if applicable).
    /// </summary>
    public DateOnly? CancelledDate { get; private set; }

    /// <summary>
    /// Reason for cancellation.
    /// </summary>
    public string? CancellationReason { get; private set; }

    // Navigation properties
    public InsuranceProduct InsuranceProduct { get; private set; } = null!;
    public Member Member { get; private set; } = null!;
    public Loan? Loan { get; private set; }
    public ICollection<InsuranceClaim> Claims { get; private set; } = new List<InsuranceClaim>();

    private InsurancePolicy() { }

    /// <summary>
    /// Creates a new insurance policy.
    /// </summary>
    public static InsurancePolicy Create(
        Guid insuranceProductId,
        Guid memberId,
        string policyNumber,
        DateOnly startDate,
        DateOnly endDate,
        decimal coverageAmount,
        decimal premiumAmount,
        int waitingPeriodDays = 0,
        Guid? loanId = null,
        string? beneficiaryName = null,
        string? beneficiaryRelation = null,
        string? beneficiaryContact = null)
    {
        var policy = new InsurancePolicy
        {
            InsuranceProductId = insuranceProductId,
            MemberId = memberId,
            PolicyNumber = policyNumber,
            StartDate = startDate,
            EndDate = endDate,
            CoverageAmount = coverageAmount,
            PremiumAmount = premiumAmount,
            TotalPremiumPaid = 0,
            LoanId = loanId,
            BeneficiaryName = beneficiaryName,
            BeneficiaryRelation = beneficiaryRelation,
            BeneficiaryContact = beneficiaryContact,
            WaitingPeriodEnd = waitingPeriodDays > 0 ? startDate.AddDays(waitingPeriodDays) : null,
            Status = StatusActive
        };

        policy.QueueDomainEvent(new InsurancePolicyCreated(policy));
        return policy;
    }

    /// <summary>
    /// Records premium payment.
    /// </summary>
    public void RecordPremiumPayment(decimal amount, DateOnly? nextDueDate = null)
    {
        TotalPremiumPaid += amount;
        NextPremiumDue = nextDueDate;
        QueueDomainEvent(new InsurancePremiumPaid(Id, amount, TotalPremiumPaid));
    }

    /// <summary>
    /// Updates beneficiary information.
    /// </summary>
    public void UpdateBeneficiary(string name, string? relation = null, string? contact = null)
    {
        BeneficiaryName = name;
        BeneficiaryRelation = relation;
        BeneficiaryContact = contact;
        QueueDomainEvent(new InsuranceBeneficiaryUpdated(Id, name));
    }

    /// <summary>
    /// Marks policy as lapsed due to non-payment.
    /// </summary>
    public void Lapse()
    {
        if (Status != StatusActive)
            throw new InvalidOperationException("Only active policies can lapse.");

        Status = StatusLapsed;
        QueueDomainEvent(new InsurancePolicyLapsed(Id));
    }

    /// <summary>
    /// Reinstates a lapsed policy.
    /// </summary>
    public void Reinstate()
    {
        if (Status != StatusLapsed)
            throw new InvalidOperationException("Only lapsed policies can be reinstated.");

        Status = StatusActive;
        QueueDomainEvent(new InsurancePolicyReinstated(Id));
    }

    /// <summary>
    /// Cancels the policy.
    /// </summary>
    public void Cancel(string reason)
    {
        if (Status != StatusActive && Status != StatusLapsed)
            throw new InvalidOperationException("Policy cannot be cancelled.");

        CancelledDate = DateOnly.FromDateTime(DateTime.UtcNow);
        CancellationReason = reason;
        Status = StatusCancelled;

        QueueDomainEvent(new InsurancePolicyCancelled(Id, reason));
    }

    /// <summary>
    /// Marks policy as expired.
    /// </summary>
    public void MarkExpired()
    {
        if (DateOnly.FromDateTime(DateTime.UtcNow) < EndDate)
            throw new InvalidOperationException("Policy has not yet reached end date.");

        Status = StatusExpired;
        QueueDomainEvent(new InsurancePolicyExpired(Id));
    }

    /// <summary>
    /// Marks policy as claimed (full payout made).
    /// </summary>
    public void MarkClaimed()
    {
        Status = StatusClaimed;
        QueueDomainEvent(new InsurancePolicyClaimed(Id));
    }

    /// <summary>
    /// Marks policy as matured.
    /// </summary>
    public void MarkMatured()
    {
        Status = StatusMatured;
        QueueDomainEvent(new InsurancePolicyMatured(Id));
    }

    /// <summary>
    /// Checks if policy can accept claims.
    /// </summary>
    public bool CanClaim()
    {
        if (Status != StatusActive) return false;
        if (IsInWaitingPeriod) return false;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return today >= StartDate && today <= EndDate;
    }
}
