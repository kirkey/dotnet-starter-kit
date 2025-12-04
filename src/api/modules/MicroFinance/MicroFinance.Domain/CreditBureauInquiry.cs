using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a credit bureau inquiry made for a member.
/// Tracks when and why credit checks were performed.
/// </summary>
public sealed class CreditBureauInquiry : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int InquiryNumber = 64;
        public const int BureauName = 128;
        public const int Purpose = 128;
        public const int RequestedBy = 128;
        public const int ReferenceNumber = 64;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Inquiry purpose.
    /// </summary>
    public const string PurposeLoanApplication = "LoanApplication";
    public const string PurposeLoanRenewal = "LoanRenewal";
    public const string PurposeLoanReview = "LoanReview";
    public const string PurposeCollections = "Collections";
    public const string PurposeAccountReview = "AccountReview";
    public const string PurposeRiskAssessment = "RiskAssessment";

    /// <summary>
    /// Inquiry status.
    /// </summary>
    public const string StatusPending = "Pending";
    public const string StatusCompleted = "Completed";
    public const string StatusFailed = "Failed";
    public const string StatusNoMatch = "NoMatch";

    /// <summary>
    /// Reference to the member.
    /// </summary>
    public Guid MemberId { get; private set; }

    /// <summary>
    /// Reference to the loan application (if applicable).
    /// </summary>
    public Guid? LoanId { get; private set; }

    /// <summary>
    /// Unique inquiry tracking number.
    /// </summary>
    public string InquiryNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Name of the credit bureau.
    /// </summary>
    public string BureauName { get; private set; } = string.Empty;

    /// <summary>
    /// Purpose of the inquiry.
    /// </summary>
    public string Purpose { get; private set; } = PurposeLoanApplication;

    /// <summary>
    /// Date and time of the inquiry.
    /// </summary>
    public DateTime InquiryDate { get; private set; }

    /// <summary>
    /// User who requested the inquiry.
    /// </summary>
    public string? RequestedBy { get; private set; }

    /// <summary>
    /// User ID of the requestor.
    /// </summary>
    public Guid? RequestedByUserId { get; private set; }

    /// <summary>
    /// Bureau reference number returned.
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Current status of the inquiry.
    /// </summary>
    public string Status { get; private set; } = StatusPending;

    /// <summary>
    /// Date and time when response was received.
    /// </summary>
    public DateTime? ResponseReceivedAt { get; private set; }

    /// <summary>
    /// Credit score returned (if available).
    /// </summary>
    public int? CreditScore { get; private set; }

    /// <summary>
    /// Reference to the created credit report.
    /// </summary>
    public Guid? CreditReportId { get; private set; }

    /// <summary>
    /// Cost of the inquiry.
    /// </summary>
    public decimal? InquiryCost { get; private set; }

    /// <summary>
    /// Error message if inquiry failed.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Member Member { get; private set; } = null!;
    public Loan? Loan { get; private set; }
    public CreditBureauReport? CreditReport { get; private set; }

    private CreditBureauInquiry() { }

    /// <summary>
    /// Creates a new credit bureau inquiry.
    /// </summary>
    public static CreditBureauInquiry Create(
        Guid memberId,
        string inquiryNumber,
        string bureauName,
        string purpose,
        Guid? loanId = null,
        string? requestedBy = null,
        Guid? requestedByUserId = null,
        decimal? inquiryCost = null)
    {
        var inquiry = new CreditBureauInquiry
        {
            MemberId = memberId,
            InquiryNumber = inquiryNumber,
            BureauName = bureauName,
            Purpose = purpose,
            LoanId = loanId,
            InquiryDate = DateTime.UtcNow,
            RequestedBy = requestedBy,
            RequestedByUserId = requestedByUserId,
            InquiryCost = inquiryCost,
            Status = StatusPending
        };

        inquiry.QueueDomainEvent(new CreditBureauInquiryCreated(inquiry));
        return inquiry;
    }

    /// <summary>
    /// Records the successful completion of the inquiry.
    /// </summary>
    public void Complete(string referenceNumber, int? creditScore = null, Guid? creditReportId = null)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException("Only pending inquiries can be completed.");

        ReferenceNumber = referenceNumber;
        CreditScore = creditScore;
        CreditReportId = creditReportId;
        ResponseReceivedAt = DateTime.UtcNow;
        Status = StatusCompleted;

        QueueDomainEvent(new CreditBureauInquiryCompleted(Id, referenceNumber, creditScore));
    }

    /// <summary>
    /// Marks the inquiry as failed.
    /// </summary>
    public void Fail(string errorMessage)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException("Only pending inquiries can be marked as failed.");

        ErrorMessage = errorMessage;
        ResponseReceivedAt = DateTime.UtcNow;
        Status = StatusFailed;

        QueueDomainEvent(new CreditBureauInquiryFailed(Id, errorMessage));
    }

    /// <summary>
    /// Marks the inquiry as no match found.
    /// </summary>
    public void MarkNoMatch()
    {
        if (Status != StatusPending)
            throw new InvalidOperationException("Only pending inquiries can be marked as no match.");

        ResponseReceivedAt = DateTime.UtcNow;
        Status = StatusNoMatch;

        QueueDomainEvent(new CreditBureauInquiryNoMatch(Id));
    }
}
