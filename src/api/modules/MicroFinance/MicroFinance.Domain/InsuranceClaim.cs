using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an insurance claim filed against a policy.
/// Tracks the claim process from filing to settlement.
/// </summary>
public sealed class InsuranceClaim : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int ClaimNumber = 32;
        public const int ClaimType = 64;
        public const int Description = 4096;
        public const int RejectionReason = 1024;
        public const int PaymentReference = 64;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Claim type classification.
    /// </summary>
    public const string TypeDeath = "Death";
    public const string TypeDisability = "Disability";
    public const string TypeHospitalization = "Hospitalization";
    public const string TypeCropLoss = "CropLoss";
    public const string TypeLivestockLoss = "LivestockLoss";
    public const string TypePropertyDamage = "PropertyDamage";
    public const string TypeAccident = "Accident";
    public const string TypeLoanDefault = "LoanDefault";

    /// <summary>
    /// Claim status values.
    /// </summary>
    public const string StatusDraft = "Draft";
    public const string StatusFiled = "Filed";
    public const string StatusUnderReview = "UnderReview";
    public const string StatusPendingDocuments = "PendingDocuments";
    public const string StatusApproved = "Approved";
    public const string StatusPartiallyApproved = "PartiallyApproved";
    public const string StatusRejected = "Rejected";
    public const string StatusPaid = "Paid";
    public const string StatusClosed = "Closed";

    /// <summary>
    /// Reference to the insurance policy.
    /// </summary>
    public Guid InsurancePolicyId { get; private set; }

    /// <summary>
    /// Unique claim number.
    /// </summary>
    public string ClaimNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Type of claim.
    /// </summary>
    public string ClaimType { get; private set; } = string.Empty;

    /// <summary>
    /// Date of the incident/event.
    /// </summary>
    public DateOnly IncidentDate { get; private set; }

    /// <summary>
    /// Date when claim was filed.
    /// </summary>
    public DateOnly FiledDate { get; private set; }

    /// <summary>
    /// Description of the incident.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Amount being claimed.
    /// </summary>
    public decimal ClaimAmount { get; private set; }

    /// <summary>
    /// Approved claim amount.
    /// </summary>
    public decimal? ApprovedAmount { get; private set; }

    /// <summary>
    /// Amount paid out.
    /// </summary>
    public decimal? PaidAmount { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusDraft;

    /// <summary>
    /// Claim reviewer user ID.
    /// </summary>
    public Guid? ReviewedByUserId { get; private set; }

    /// <summary>
    /// Date reviewed.
    /// </summary>
    public DateTime? ReviewedAt { get; private set; }

    /// <summary>
    /// Claim approver user ID.
    /// </summary>
    public Guid? ApprovedByUserId { get; private set; }

    /// <summary>
    /// Date approved/rejected.
    /// </summary>
    public DateTime? DecisionAt { get; private set; }

    /// <summary>
    /// Rejection reason (if rejected).
    /// </summary>
    public string? RejectionReason { get; private set; }

    /// <summary>
    /// Payment reference number.
    /// </summary>
    public string? PaymentReference { get; private set; }

    /// <summary>
    /// Date payment was made.
    /// </summary>
    public DateOnly? PaymentDate { get; private set; }

    /// <summary>
    /// Supporting documents (JSON array of document references).
    /// </summary>
    public string? Documents { get; private set; }

    // Navigation properties
    public InsurancePolicy InsurancePolicy { get; private set; } = null!;

    private InsuranceClaim() { }

    /// <summary>
    /// Creates a new insurance claim.
    /// </summary>
    public static InsuranceClaim Create(
        Guid insurancePolicyId,
        string claimNumber,
        string claimType,
        DateOnly incidentDate,
        decimal claimAmount,
        string? description = null)
    {
        var claim = new InsuranceClaim
        {
            InsurancePolicyId = insurancePolicyId,
            ClaimNumber = claimNumber,
            ClaimType = claimType,
            IncidentDate = incidentDate,
            ClaimAmount = claimAmount,
            Description = description,
            FiledDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = StatusDraft
        };

        claim.QueueDomainEvent(new InsuranceClaimCreated(claim));
        return claim;
    }

    /// <summary>
    /// Files the claim.
    /// </summary>
    public void File()
    {
        if (Status != StatusDraft)
            throw new InvalidOperationException("Only draft claims can be filed.");

        FiledDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusFiled;
        QueueDomainEvent(new InsuranceClaimFiled(Id));
    }

    /// <summary>
    /// Starts review of the claim.
    /// </summary>
    public void StartReview(Guid reviewerUserId)
    {
        if (Status != StatusFiled)
            throw new InvalidOperationException("Only filed claims can be reviewed.");

        ReviewedByUserId = reviewerUserId;
        ReviewedAt = DateTime.UtcNow;
        Status = StatusUnderReview;
        QueueDomainEvent(new InsuranceClaimUnderReview(Id, reviewerUserId));
    }

    /// <summary>
    /// Requests additional documents.
    /// </summary>
    public void RequestDocuments(string? notes = null)
    {
        Status = StatusPendingDocuments;
        if (notes is not null) Notes = notes;
        QueueDomainEvent(new InsuranceClaimDocumentsRequested(Id));
    }

    /// <summary>
    /// Adds document reference.
    /// </summary>
    public void AddDocument(string documentReference)
    {
        Documents = string.IsNullOrEmpty(Documents)
            ? documentReference
            : $"{Documents},{documentReference}";
    }

    /// <summary>
    /// Approves the claim.
    /// </summary>
    public void Approve(Guid approverUserId, decimal approvedAmount)
    {
        if (Status != StatusUnderReview && Status != StatusPendingDocuments)
            throw new InvalidOperationException("Claim not ready for approval.");

        ApprovedByUserId = approverUserId;
        DecisionAt = DateTime.UtcNow;
        ApprovedAmount = approvedAmount;

        Status = approvedAmount < ClaimAmount ? StatusPartiallyApproved : StatusApproved;
        QueueDomainEvent(new InsuranceClaimApproved(Id, approverUserId, approvedAmount));
    }

    /// <summary>
    /// Rejects the claim.
    /// </summary>
    public void Reject(Guid approverUserId, string reason)
    {
        if (Status != StatusUnderReview && Status != StatusPendingDocuments)
            throw new InvalidOperationException("Claim not ready for decision.");

        ApprovedByUserId = approverUserId;
        DecisionAt = DateTime.UtcNow;
        RejectionReason = reason;
        Status = StatusRejected;
        QueueDomainEvent(new InsuranceClaimRejected(Id, approverUserId, reason));
    }

    /// <summary>
    /// Records payment of the claim.
    /// </summary>
    public void Pay(decimal amount, string paymentReference, DateOnly? paymentDate = null)
    {
        if (Status != StatusApproved && Status != StatusPartiallyApproved)
            throw new InvalidOperationException("Only approved claims can be paid.");

        PaidAmount = amount;
        PaymentReference = paymentReference;
        PaymentDate = paymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusPaid;
        QueueDomainEvent(new InsuranceClaimPaid(Id, amount, paymentReference));
    }

    /// <summary>
    /// Closes the claim.
    /// </summary>
    public void Close(string? notes = null)
    {
        if (Status != StatusPaid && Status != StatusRejected)
            throw new InvalidOperationException("Only paid or rejected claims can be closed.");

        if (notes is not null) Notes = notes;
        Status = StatusClosed;
        QueueDomainEvent(new InsuranceClaimClosed(Id));
    }
}
