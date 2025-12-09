using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an insurance policy issued to a member.
/// Tracks coverage amounts, premiums, beneficiaries, and policy lifecycle.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Issue insurance policies linked to loans or as standalone products</description></item>
///   <item><description>Track premium payments and payment schedules</description></item>
///   <item><description>Manage policy renewals and expirations</description></item>
///   <item><description>Record beneficiary details for claims</description></item>
///   <item><description>Process policy cancellations and lapses</description></item>
///   <item><description>Link policies to loans for credit life coverage</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Insurance policies protect members and the MFI's loan portfolio:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Credit Life</strong>: Clears loan balance on borrower death/disability</description></item>
///   <item><description><strong>Member Protection</strong>: Provides safety net for vulnerable populations</description></item>
///   <item><description><strong>Premium Collection</strong>: Often deducted from loan disbursement or savings</description></item>
///   <item><description><strong>Third-Party Integration</strong>: Policies may be with external insurers</description></item>
/// </list>
/// <para><strong>Policy Status:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Active</strong>: Policy in force, premiums current</description></item>
///   <item><description><strong>Lapsed</strong>: Premium not paid, coverage suspended</description></item>
///   <item><description><strong>Cancelled</strong>: Policy terminated before term end</description></item>
///   <item><description><strong>Expired</strong>: Policy term completed naturally</description></item>
///   <item><description><strong>Claimed</strong>: Claim filed and processed</description></item>
///   <item><description><strong>Matured</strong>: Endowment policy reached maturity</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Member"/> - Policy holder</description></item>
///   <item><description><see cref="InsuranceProduct"/> - Product template</description></item>
///   <item><description><see cref="InsuranceClaim"/> - Claims against this policy</description></item>
///   <item><description><see cref="Loan"/> - Associated loan for credit life</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Issuing a credit life policy</strong></para>
/// <code>
/// POST /api/microfinance/insurance-policies
/// {
///   "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "insuranceProductId": "a1b2c3d4-5e6f-7890-abcd-ef1234567890",
///   "loanId": "b2c3d4e5-6f78-9012-bcde-f12345678901",
///   "coverageAmount": 500000,
///   "premiumAmount": 2500,
///   "effectiveDate": "2024-01-15",
///   "expiryDate": "2025-01-14",
///   "beneficiaryName": "Marie Mukiza",
///   "beneficiaryRelation": "Spouse",
///   "beneficiaryContact": "+250788111222"
/// }
/// </code>
/// </example>
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
