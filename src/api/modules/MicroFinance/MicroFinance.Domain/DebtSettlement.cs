using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for DebtSettlement entity.
/// </summary>
public static class DebtSettlementConstants
{
    /// <summary>Maximum length for reference number. (2^6 = 64)</summary>
    public const int ReferenceNumberMaxLength = 64;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for settlement type. (2^5 = 32)</summary>
    public const int SettlementTypeMaxLength = 32;

    /// <summary>Maximum length for terms. (2^11 = 2048)</summary>
    public const int TermsMaxLength = 2048;

    /// <summary>Maximum length for justification. (2^11 = 2048)</summary>
    public const int JustificationMaxLength = 2048;

    /// <summary>Maximum length for notes. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;
}

/// <summary>
/// Represents a negotiated debt settlement for a delinquent loan.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Record negotiated settlements with discounts on outstanding balances</description></item>
///   <item><description>Track approval workflow for settlement offers</description></item>
///   <item><description>Monitor settlement payment compliance</description></item>
///   <item><description>Analyze settlement patterns and recovery rates</description></item>
///   <item><description>Support write-off calculations for settled amounts</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Debt settlements are used when full recovery is unlikely and a negotiated
/// resolution is more cost-effective than continued collection or legal action.
/// Settlements typically involve accepting less than the full amount owed.
/// </para>
/// </remarks>
public class DebtSettlement : AuditableEntity, IAggregateRoot
{
    // Settlement Types
    /// <summary>One-time lump sum settlement.</summary>
    public const string TypeLumpSum = "LUMP_SUM";
    /// <summary>Settlement paid in installments.</summary>
    public const string TypeInstallment = "INSTALLMENT";
    /// <summary>Principal only - interest waived.</summary>
    public const string TypePrincipalOnly = "PRINCIPAL_ONLY";
    /// <summary>Partial settlement - remaining written off.</summary>
    public const string TypePartial = "PARTIAL";

    // Statuses
    /// <summary>Settlement proposed, pending review.</summary>
    public const string StatusProposed = "PROPOSED";
    /// <summary>Settlement pending approval.</summary>
    public const string StatusPendingApproval = "PENDING_APPROVAL";
    /// <summary>Settlement approved.</summary>
    public const string StatusApproved = "APPROVED";
    /// <summary>Settlement rejected.</summary>
    public const string StatusRejected = "REJECTED";
    /// <summary>Settlement accepted by member.</summary>
    public const string StatusAccepted = "ACCEPTED";
    /// <summary>Settlement payment in progress.</summary>
    public const string StatusInProgress = "IN_PROGRESS";
    /// <summary>Settlement fully paid - completed.</summary>
    public const string StatusCompleted = "COMPLETED";
    /// <summary>Settlement payment defaulted.</summary>
    public const string StatusDefaulted = "DEFAULTED";
    /// <summary>Settlement cancelled.</summary>
    public const string StatusCancelled = "CANCELLED";

    /// <summary>Gets the unique reference number.</summary>
    public string ReferenceNumber { get; private set; } = default!;

    /// <summary>Gets the collection case ID.</summary>
    public DefaultIdType CollectionCaseId { get; private set; }

    /// <summary>Gets the collection case navigation property.</summary>
    public virtual CollectionCase? CollectionCase { get; private set; }

    /// <summary>Gets the loan ID.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan? Loan { get; private set; }

    /// <summary>Gets the member ID.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member? Member { get; private set; }

    /// <summary>Gets the type of settlement.</summary>
    public string SettlementType { get; private set; } = default!;

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the original outstanding amount.</summary>
    public decimal OriginalOutstanding { get; private set; }

    /// <summary>Gets the settlement amount agreed.</summary>
    public decimal SettlementAmount { get; private set; }

    /// <summary>Gets the discount amount (original - settlement).</summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>Gets the discount percentage.</summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>Gets the amount paid so far.</summary>
    public decimal AmountPaid { get; private set; }

    /// <summary>Gets the remaining balance to pay.</summary>
    public decimal RemainingBalance { get; private set; }

    /// <summary>Gets the number of installments (if applicable).</summary>
    public int? NumberOfInstallments { get; private set; }

    /// <summary>Gets the installment amount (if applicable).</summary>
    public decimal? InstallmentAmount { get; private set; }

    /// <summary>Gets the date settlement was proposed.</summary>
    public DateOnly ProposedDate { get; private set; }

    /// <summary>Gets the date settlement was approved.</summary>
    public DateOnly? ApprovedDate { get; private set; }

    /// <summary>Gets the settlement due date (for lump sum) or final payment date.</summary>
    public DateOnly DueDate { get; private set; }

    /// <summary>Gets the date settlement was completed.</summary>
    public DateOnly? CompletedDate { get; private set; }

    /// <summary>Gets the settlement terms and conditions.</summary>
    public string Terms { get; private set; } = default!;

    /// <summary>Gets the justification for the discount.</summary>
    public string? Justification { get; private set; }

    /// <summary>Gets the staff ID who proposed the settlement.</summary>
    public DefaultIdType ProposedById { get; private set; }

    /// <summary>Gets the staff ID who approved the settlement.</summary>
    public DefaultIdType? ApprovedById { get; private set; }

    private DebtSettlement() { }

    private DebtSettlement(
        DefaultIdType id,
        string referenceNumber,
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        DefaultIdType memberId,
        string settlementType,
        decimal originalOutstanding,
        decimal settlementAmount,
        DateOnly dueDate,
        string terms,
        DefaultIdType proposedById)
    {
        Id = id;
        ReferenceNumber = referenceNumber.Trim();
        CollectionCaseId = collectionCaseId;
        LoanId = loanId;
        MemberId = memberId;
        SettlementType = settlementType;
        OriginalOutstanding = originalOutstanding;
        SettlementAmount = settlementAmount;
        DiscountAmount = originalOutstanding - settlementAmount;
        DiscountPercentage = originalOutstanding > 0 
            ? Math.Round((DiscountAmount / originalOutstanding) * 100, 2) 
            : 0;
        AmountPaid = 0;
        RemainingBalance = settlementAmount;
        DueDate = dueDate;
        Terms = terms.Trim();
        ProposedById = proposedById;
        ProposedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusProposed;

        QueueDomainEvent(new DebtSettlementProposed { DebtSettlement = this });
    }

    /// <summary>Creates a new lump sum settlement.</summary>
    public static DebtSettlement CreateLumpSum(
        string referenceNumber,
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        DefaultIdType memberId,
        decimal originalOutstanding,
        decimal settlementAmount,
        DateOnly dueDate,
        string terms,
        DefaultIdType proposedById)
    {
        return new DebtSettlement(
            DefaultIdType.NewGuid(),
            referenceNumber,
            collectionCaseId,
            loanId,
            memberId,
            TypeLumpSum,
            originalOutstanding,
            settlementAmount,
            dueDate,
            terms,
            proposedById);
    }

    /// <summary>Creates a new installment settlement.</summary>
    public static DebtSettlement CreateInstallment(
        string referenceNumber,
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        DefaultIdType memberId,
        decimal originalOutstanding,
        decimal settlementAmount,
        int numberOfInstallments,
        DateOnly finalDueDate,
        string terms,
        DefaultIdType proposedById)
    {
        var settlement = new DebtSettlement(
            DefaultIdType.NewGuid(),
            referenceNumber,
            collectionCaseId,
            loanId,
            memberId,
            TypeInstallment,
            originalOutstanding,
            settlementAmount,
            finalDueDate,
            terms,
            proposedById);

        settlement.NumberOfInstallments = numberOfInstallments;
        settlement.InstallmentAmount = Math.Round(settlementAmount / numberOfInstallments, 2);
        
        return settlement;
    }

    /// <summary>Submits settlement for approval.</summary>
    public DebtSettlement SubmitForApproval(string justification)
    {
        if (Status != StatusProposed)
            throw new InvalidOperationException($"Cannot submit for approval when status is {Status}.");

        Justification = justification?.Trim();
        Status = StatusPendingApproval;
        return this;
    }

    /// <summary>Approves the settlement.</summary>
    public DebtSettlement Approve(DefaultIdType approvedById)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException($"Cannot approve when status is {Status}.");

        ApprovedById = approvedById;
        ApprovedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusApproved;
        
        QueueDomainEvent(new DebtSettlementApproved { SettlementId = Id, ApprovedById = approvedById });
        return this;
    }

    /// <summary>Rejects the settlement.</summary>
    public DebtSettlement Reject(string reason)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException($"Cannot reject when status is {Status}.");

        Status = StatusRejected;
        Notes = $"Rejected: {reason}";
        return this;
    }

    /// <summary>Records member acceptance of settlement.</summary>
    public DebtSettlement RecordAcceptance()
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException($"Cannot accept when status is {Status}.");

        Status = StatusAccepted;
        return this;
    }

    /// <summary>Records a payment towards the settlement.</summary>
    public DebtSettlement RecordPayment(decimal amount)
    {
        if (Status != StatusAccepted && Status != StatusInProgress)
            throw new InvalidOperationException($"Cannot record payment when status is {Status}.");

        AmountPaid += amount;
        RemainingBalance = Math.Max(0, SettlementAmount - AmountPaid);
        Status = StatusInProgress;

        if (RemainingBalance <= 0)
        {
            Status = StatusCompleted;
            CompletedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            QueueDomainEvent(new DebtSettlementCompleted { SettlementId = Id, TotalPaid = AmountPaid });
        }
        
        return this;
    }

    /// <summary>Marks settlement as defaulted.</summary>
    public DebtSettlement MarkAsDefaulted(string reason)
    {
        if (Status != StatusAccepted && Status != StatusInProgress)
            throw new InvalidOperationException($"Cannot mark as defaulted when status is {Status}.");

        Status = StatusDefaulted;
        Notes = $"Defaulted: {reason}";
        return this;
    }

    /// <summary>Cancels the settlement.</summary>
    public DebtSettlement Cancel(string reason)
    {
        if (Status == StatusCompleted)
            throw new InvalidOperationException("Cannot cancel a completed settlement.");

        Status = StatusCancelled;
        Notes = $"Cancelled: {reason}";
        return this;
    }
}
