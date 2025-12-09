using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan application before it becomes an approved and disbursed loan.
/// Tracks the application process from submission through underwriting to approval or rejection.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Submit new loan requests from members</description></item>
///   <item><description>Conduct credit appraisal and risk assessment</description></item>
///   <item><description>Route applications through approval workflows</description></item>
///   <item><description>Track pending documents and conditions</description></item>
///   <item><description>Record approval/rejection decisions with reasons</description></item>
///   <item><description>Convert approved applications to active loans</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// The loan application process is the front door of microfinance lending:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Member Eligibility</strong>: Verify membership status, savings requirements</description></item>
///   <item><description><strong>Credit History</strong>: Review past loan performance with the MFI</description></item>
///   <item><description><strong>Capacity Assessment</strong>: Analyze income and debt-to-income ratio</description></item>
///   <item><description><strong>Collateral Review</strong>: Evaluate pledged assets and guarantors</description></item>
///   <item><description><strong>Character Check</strong>: Community references, group input for solidarity loans</description></item>
/// </list>
/// <para><strong>Application Status Flow:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Draft</strong> → <strong>Submitted</strong>: Member completes and submits</description></item>
///   <item><description><strong>Submitted</strong> → <strong>UnderReview</strong>: Loan officer begins assessment</description></item>
///   <item><description><strong>UnderReview</strong> → <strong>PendingDocuments</strong>: Awaiting member documents</description></item>
///   <item><description><strong>UnderReview</strong> → <strong>PendingApproval</strong>: Sent for approval decision</description></item>
///   <item><description><strong>PendingApproval</strong> → <strong>Approved</strong>: Approved for disbursement</description></item>
///   <item><description><strong>PendingApproval</strong> → <strong>Rejected</strong>: Application declined</description></item>
///   <item><description><strong>Approved</strong> → <strong>Disbursed</strong>: Loan created and disbursed</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Member"/> - Applicant</description></item>
///   <item><description><see cref="LoanProduct"/> - Product being applied for</description></item>
///   <item><description><see cref="Loan"/> - Created when application is approved and disbursed</description></item>
///   <item><description><see cref="ApprovalRequest"/> - Approval workflow request</description></item>
///   <item><description><see cref="Document"/> - Application supporting documents</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Submitting a new loan application</strong></para>
/// <code>
/// POST /api/microfinance/loan-applications
/// {
///   "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "loanProductId": "a1b2c3d4-5e6f-7890-abcd-ef1234567890",
///   "requestedAmount": 500000,
///   "requestedTerm": 12,
///   "purpose": "Business expansion - inventory purchase",
///   "businessType": "Retail Trade",
///   "monthlyIncome": 150000,
///   "monthlyExpenses": 80000
/// }
/// </code>
/// </example>
public sealed class LoanApplication : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int ApplicationNumber = 32;
        public const int Purpose = 512;
        public const int BusinessType = 128;
        public const int RejectionReason = 1024;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Application status values.
    /// </summary>
    public const string StatusDraft = "Draft";
    public const string StatusSubmitted = "Submitted";
    public const string StatusUnderReview = "UnderReview";
    public const string StatusPendingDocuments = "PendingDocuments";
    public const string StatusPendingApproval = "PendingApproval";
    public const string StatusApproved = "Approved";
    public const string StatusConditionallyApproved = "ConditionallyApproved";
    public const string StatusRejected = "Rejected";
    public const string StatusWithdrawn = "Withdrawn";
    public const string StatusDisbursed = "Disbursed";
    public const string StatusExpired = "Expired";

    /// <summary>
    /// Unique application number.
    /// </summary>
    public string ApplicationNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Reference to the member applying.
    /// </summary>
    public Guid MemberId { get; private set; }

    /// <summary>
    /// Reference to the loan product.
    /// </summary>
    public Guid LoanProductId { get; private set; }

    /// <summary>
    /// Reference to the member group (for group loans).
    /// </summary>
    public Guid? MemberGroupId { get; private set; }

    /// <summary>
    /// Date of application submission.
    /// </summary>
    public DateOnly ApplicationDate { get; private set; }

    /// <summary>
    /// Requested loan amount.
    /// </summary>
    public decimal RequestedAmount { get; private set; }

    /// <summary>
    /// Approved loan amount (may differ from requested).
    /// </summary>
    public decimal? ApprovedAmount { get; private set; }

    /// <summary>
    /// Requested term in months.
    /// </summary>
    public int RequestedTermMonths { get; private set; }

    /// <summary>
    /// Approved term in months.
    /// </summary>
    public int? ApprovedTermMonths { get; private set; }

    /// <summary>
    /// Purpose of the loan.
    /// </summary>
    public string? Purpose { get; private set; }

    /// <summary>
    /// Type of business (if business loan).
    /// </summary>
    public string? BusinessType { get; private set; }

    /// <summary>
    /// Member's monthly income.
    /// </summary>
    public decimal? MonthlyIncome { get; private set; }

    /// <summary>
    /// Member's monthly expenses.
    /// </summary>
    public decimal? MonthlyExpenses { get; private set; }

    /// <summary>
    /// Existing debt obligations.
    /// </summary>
    public decimal? ExistingDebt { get; private set; }

    /// <summary>
    /// Debt-to-income ratio.
    /// </summary>
    public decimal? DebtToIncomeRatio { get; private set; }

    /// <summary>
    /// Credit score at time of application.
    /// </summary>
    public int? CreditScore { get; private set; }

    /// <summary>
    /// Risk grade assigned.
    /// </summary>
    public string? RiskGrade { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusDraft;

    /// <summary>
    /// Assigned loan officer.
    /// </summary>
    public Guid? AssignedOfficerId { get; private set; }

    /// <summary>
    /// Date assigned to officer.
    /// </summary>
    public DateTime? AssignedAt { get; private set; }

    /// <summary>
    /// User who approved/rejected.
    /// </summary>
    public Guid? DecisionByUserId { get; private set; }

    /// <summary>
    /// Date of decision.
    /// </summary>
    public DateTime? DecisionAt { get; private set; }

    /// <summary>
    /// Rejection reason (if rejected).
    /// </summary>
    public string? RejectionReason { get; private set; }

    /// <summary>
    /// Conditions for conditional approval.
    /// </summary>
    public string? ApprovalConditions { get; private set; }

    /// <summary>
    /// Expiry date for approval.
    /// </summary>
    public DateOnly? ApprovalExpiryDate { get; private set; }

    /// <summary>
    /// Reference to created loan (if disbursed).
    /// </summary>
    public Guid? LoanId { get; private set; }

    /// <summary>

    // Navigation properties
    public Member Member { get; private set; } = null!;
    public LoanProduct LoanProduct { get; private set; } = null!;
    public MemberGroup? MemberGroup { get; private set; }
    public Loan? Loan { get; private set; }

    private LoanApplication() { }

    /// <summary>
    /// Creates a new loan application.
    /// </summary>
    public static LoanApplication Create(
        string applicationNumber,
        Guid memberId,
        Guid loanProductId,
        decimal requestedAmount,
        int requestedTermMonths,
        string? purpose = null,
        Guid? memberGroupId = null,
        decimal? monthlyIncome = null,
        decimal? monthlyExpenses = null,
        decimal? existingDebt = null)
    {
        var application = new LoanApplication
        {
            ApplicationNumber = applicationNumber,
            MemberId = memberId,
            LoanProductId = loanProductId,
            RequestedAmount = requestedAmount,
            RequestedTermMonths = requestedTermMonths,
            Purpose = purpose,
            MemberGroupId = memberGroupId,
            MonthlyIncome = monthlyIncome,
            MonthlyExpenses = monthlyExpenses,
            ExistingDebt = existingDebt,
            ApplicationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = StatusDraft
        };

        // Calculate debt-to-income ratio
        if (monthlyIncome.HasValue && monthlyIncome.Value > 0)
        {
            var totalDebt = (existingDebt ?? 0) + (requestedAmount / requestedTermMonths);
            application.DebtToIncomeRatio = Math.Round((totalDebt / monthlyIncome.Value) * 100, 2);
        }

        application.QueueDomainEvent(new LoanApplicationCreated(application));
        return application;
    }

    /// <summary>
    /// Submits the application.
    /// </summary>
    public void Submit()
    {
        if (Status != StatusDraft)
            throw new InvalidOperationException("Only draft applications can be submitted.");

        Status = StatusSubmitted;
        QueueDomainEvent(new LoanApplicationSubmitted(Id));
    }

    /// <summary>
    /// Assigns to a loan officer.
    /// </summary>
    public void AssignToOfficer(Guid officerId)
    {
        AssignedOfficerId = officerId;
        AssignedAt = DateTime.UtcNow;
        Status = StatusUnderReview;

        QueueDomainEvent(new LoanApplicationAssigned(Id, officerId));
    }

    /// <summary>
    /// Sets credit assessment results.
    /// </summary>
    public void SetCreditAssessment(int creditScore, string riskGrade)
    {
        CreditScore = creditScore;
        RiskGrade = riskGrade;
        QueueDomainEvent(new LoanApplicationCreditAssessed(Id, creditScore, riskGrade));
    }

    /// <summary>
    /// Requests additional documents.
    /// </summary>
    public void RequestDocuments(string? notes = null)
    {
        Status = StatusPendingDocuments;
        if (notes is not null) Notes = notes;
        QueueDomainEvent(new LoanApplicationDocumentsRequested(Id));
    }

    /// <summary>
    /// Submits for approval.
    /// </summary>
    public void SubmitForApproval()
    {
        if (Status != StatusUnderReview && Status != StatusPendingDocuments)
            throw new InvalidOperationException("Application not ready for approval.");

        Status = StatusPendingApproval;
        QueueDomainEvent(new LoanApplicationPendingApproval(Id));
    }

    /// <summary>
    /// Approves the application.
    /// </summary>
    public void Approve(
        Guid userId,
        decimal approvedAmount,
        int approvedTermMonths,
        DateOnly? expiryDate = null)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException("Only pending applications can be approved.");

        DecisionByUserId = userId;
        DecisionAt = DateTime.UtcNow;
        ApprovedAmount = approvedAmount;
        ApprovedTermMonths = approvedTermMonths;
        ApprovalExpiryDate = expiryDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));
        Status = StatusApproved;

        QueueDomainEvent(new LoanApplicationApproved(Id, userId, approvedAmount));
    }

    /// <summary>
    /// Conditionally approves the application.
    /// </summary>
    public void ConditionallyApprove(
        Guid userId,
        decimal approvedAmount,
        int approvedTermMonths,
        string conditions,
        DateOnly? expiryDate = null)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException("Only pending applications can be approved.");

        DecisionByUserId = userId;
        DecisionAt = DateTime.UtcNow;
        ApprovedAmount = approvedAmount;
        ApprovedTermMonths = approvedTermMonths;
        ApprovalConditions = conditions;
        ApprovalExpiryDate = expiryDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));
        Status = StatusConditionallyApproved;

        QueueDomainEvent(new LoanApplicationConditionallyApproved(Id, userId, conditions));
    }

    /// <summary>
    /// Rejects the application.
    /// </summary>
    public void Reject(Guid userId, string reason)
    {
        if (Status != StatusPendingApproval && Status != StatusUnderReview)
            throw new InvalidOperationException("Application cannot be rejected at this stage.");

        DecisionByUserId = userId;
        DecisionAt = DateTime.UtcNow;
        RejectionReason = reason;
        Status = StatusRejected;

        QueueDomainEvent(new LoanApplicationRejected(Id, userId, reason));
    }

    /// <summary>
    /// Withdraws the application.
    /// </summary>
    public void Withdraw(string? reason = null)
    {
        if (Status == StatusDisbursed || Status == StatusRejected)
            throw new InvalidOperationException("Cannot withdraw completed applications.");

        Notes = reason;
        Status = StatusWithdrawn;
        QueueDomainEvent(new LoanApplicationWithdrawn(Id));
    }

    /// <summary>
    /// Links to the created loan after disbursement.
    /// </summary>
    public void LinkToLoan(Guid loanId)
    {
        LoanId = loanId;
        Status = StatusDisbursed;
        QueueDomainEvent(new LoanApplicationDisbursed(Id, loanId));
    }

    /// <summary>
    /// Marks as expired.
    /// </summary>
    public void MarkExpired()
    {
        if (Status != StatusApproved && Status != StatusConditionallyApproved)
            throw new InvalidOperationException("Only approved applications can expire.");

        Status = StatusExpired;
        QueueDomainEvent(new LoanApplicationExpired(Id));
    }

    /// <summary>
    /// Returns the application to the applicant for corrections.
    /// </summary>
    public void Return(string reason, string? notes = null)
    {
        if (Status != StatusUnderReview && Status != StatusPendingApproval && Status != StatusPendingDocuments)
            throw new InvalidOperationException("Application cannot be returned at this stage.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Return reason is required.", nameof(reason));

        Status = StatusDraft;
        if (!string.IsNullOrWhiteSpace(notes))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Returned: {reason}\n{notes}" : $"{Notes}\nReturned: {reason}\n{notes}";
        }
        else
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Returned: {reason}" : $"{Notes}\nReturned: {reason}";
        }

        QueueDomainEvent(new LoanApplicationReturned(Id, reason));
    }
}
