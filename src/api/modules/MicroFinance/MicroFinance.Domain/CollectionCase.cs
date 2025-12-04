using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for CollectionCase entity.
/// </summary>
public static class CollectionCaseConstants
{
    /// <summary>Maximum length for case number. (2^6 = 64)</summary>
    public const int CaseNumberMaxLength = 64;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for priority. (2^5 = 32)</summary>
    public const int PriorityMaxLength = 32;

    /// <summary>Maximum length for classification. (2^5 = 32)</summary>
    public const int ClassificationMaxLength = 32;

    /// <summary>Maximum length for notes. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;

    /// <summary>Maximum length for closure reason. (2^9 = 512)</summary>
    public const int ClosureReasonMaxLength = 512;
}

/// <summary>
/// Represents a collection case for managing delinquent loans.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Track delinquent loans requiring collection activities</description></item>
///   <item><description>Assign cases to collection officers for follow-up</description></item>
///   <item><description>Monitor aging and escalation of overdue accounts</description></item>
///   <item><description>Record collection outcomes and recovery efforts</description></item>
///   <item><description>Support portfolio-at-risk (PAR) management</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// A CollectionCase is automatically or manually created when a loan becomes delinquent.
/// It tracks the collection lifecycle from initial contact through resolution.
/// Cases can be escalated based on days past due and amount at risk.
/// </para>
/// </remarks>
public class CollectionCase : AuditableEntity, IAggregateRoot
{
    // Case Statuses
    /// <summary>Case opened, awaiting assignment.</summary>
    public const string StatusOpen = "OPEN";
    /// <summary>Case assigned to collector, active follow-up.</summary>
    public const string StatusAssigned = "ASSIGNED";
    /// <summary>Active collection activities in progress.</summary>
    public const string StatusInProgress = "IN_PROGRESS";
    /// <summary>Member has promised to pay.</summary>
    public const string StatusPromiseToPay = "PROMISE_TO_PAY";
    /// <summary>Case escalated to legal action.</summary>
    public const string StatusLegal = "LEGAL";
    /// <summary>Loan fully recovered, case closed.</summary>
    public const string StatusRecovered = "RECOVERED";
    /// <summary>Case closed - loan written off.</summary>
    public const string StatusWrittenOff = "WRITTEN_OFF";
    /// <summary>Case closed - settlement reached.</summary>
    public const string StatusSettled = "SETTLED";
    /// <summary>Case closed - other reason.</summary>
    public const string StatusClosed = "CLOSED";

    // Priority Levels
    /// <summary>Low priority - early stage delinquency.</summary>
    public const string PriorityLow = "LOW";
    /// <summary>Medium priority - moderate delinquency.</summary>
    public const string PriorityMedium = "MEDIUM";
    /// <summary>High priority - significant delinquency.</summary>
    public const string PriorityHigh = "HIGH";
    /// <summary>Critical priority - severe delinquency or large amount.</summary>
    public const string PriorityCritical = "CRITICAL";

    // Loan Classifications (aligned with regulatory standards)
    /// <summary>Current - no payment issues.</summary>
    public const string ClassificationCurrent = "CURRENT";
    /// <summary>Watch - 1-30 days past due.</summary>
    public const string ClassificationWatch = "WATCH";
    /// <summary>Substandard - 31-90 days past due.</summary>
    public const string ClassificationSubstandard = "SUBSTANDARD";
    /// <summary>Doubtful - 91-180 days past due.</summary>
    public const string ClassificationDoubtful = "DOUBTFUL";
    /// <summary>Loss - 180+ days past due.</summary>
    public const string ClassificationLoss = "LOSS";

    /// <summary>Gets the unique case number.</summary>
    public string CaseNumber { get; private set; } = default!;

    /// <summary>Gets the loan ID associated with this collection case.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan? Loan { get; private set; }

    /// <summary>Gets the member ID (borrower).</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member? Member { get; private set; }

    /// <summary>Gets the assigned collector's staff ID.</summary>
    public DefaultIdType? AssignedCollectorId { get; private set; }

    /// <summary>Gets the current status of the case.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the priority level.</summary>
    public string Priority { get; private set; } = default!;

    /// <summary>Gets the loan classification based on days past due.</summary>
    public string Classification { get; private set; } = default!;

    /// <summary>Gets the date the case was opened.</summary>
    public DateOnly OpenedDate { get; private set; }

    /// <summary>Gets the date the case was assigned.</summary>
    public DateOnly? AssignedDate { get; private set; }

    /// <summary>Gets the date the case was closed.</summary>
    public DateOnly? ClosedDate { get; private set; }

    /// <summary>Gets the days past due when the case was opened.</summary>
    public int DaysPastDueAtOpen { get; private set; }

    /// <summary>Gets the current days past due.</summary>
    public int CurrentDaysPastDue { get; private set; }

    /// <summary>Gets the total amount overdue (principal + interest + fees).</summary>
    public decimal AmountOverdue { get; private set; }

    /// <summary>Gets the total outstanding balance on the loan.</summary>
    public decimal TotalOutstanding { get; private set; }

    /// <summary>Gets the amount recovered during this case.</summary>
    public decimal AmountRecovered { get; private set; }

    /// <summary>Gets the last contact date with the member.</summary>
    public DateOnly? LastContactDate { get; private set; }

    /// <summary>Gets the next scheduled follow-up date.</summary>
    public DateOnly? NextFollowUpDate { get; private set; }

    /// <summary>Gets the total number of contact attempts.</summary>
    public int ContactAttempts { get; private set; }

    /// <summary>Gets the closure reason if case is closed.</summary>
    public string? ClosureReason { get; private set; }

    /// <summary>Gets the collection of actions taken on this case.</summary>
    public virtual ICollection<CollectionAction> CollectionActions { get; private set; } = new List<CollectionAction>();

    /// <summary>Gets the collection of promises to pay.</summary>
    public virtual ICollection<PromiseToPay> PromisesToPay { get; private set; } = new List<PromiseToPay>();

    private CollectionCase() { }

    private CollectionCase(
        DefaultIdType id,
        string caseNumber,
        DefaultIdType loanId,
        DefaultIdType memberId,
        int daysPastDue,
        decimal amountOverdue,
        decimal totalOutstanding)
    {
        Id = id;
        CaseNumber = caseNumber.Trim();
        LoanId = loanId;
        MemberId = memberId;
        Status = StatusOpen;
        Priority = CalculatePriority(daysPastDue, amountOverdue);
        Classification = CalculateClassification(daysPastDue);
        OpenedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        DaysPastDueAtOpen = daysPastDue;
        CurrentDaysPastDue = daysPastDue;
        AmountOverdue = amountOverdue;
        TotalOutstanding = totalOutstanding;
        AmountRecovered = 0;
        ContactAttempts = 0;

        QueueDomainEvent(new CollectionCaseCreated { CollectionCase = this });
    }

    /// <summary>Creates a new CollectionCase.</summary>
    public static CollectionCase Create(
        string caseNumber,
        DefaultIdType loanId,
        DefaultIdType memberId,
        int daysPastDue,
        decimal amountOverdue,
        decimal totalOutstanding)
    {
        return new CollectionCase(
            DefaultIdType.NewGuid(),
            caseNumber,
            loanId,
            memberId,
            daysPastDue,
            amountOverdue,
            totalOutstanding);
    }

    /// <summary>Assigns the case to a collector.</summary>
    public CollectionCase Assign(DefaultIdType collectorId, DateOnly? followUpDate = null)
    {
        if (Status == StatusClosed || Status == StatusRecovered || Status == StatusWrittenOff)
            throw new InvalidOperationException("Cannot assign a closed case.");

        AssignedCollectorId = collectorId;
        AssignedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusAssigned;
        NextFollowUpDate = followUpDate;

        QueueDomainEvent(new CollectionCaseAssigned { CaseId = Id, CollectorId = collectorId });
        return this;
    }

    /// <summary>Updates the case status.</summary>
    public CollectionCase UpdateStatus(string newStatus, string? notes = null)
    {
        Status = newStatus;
        if (!string.IsNullOrWhiteSpace(notes))
        {
            Notes = notes;
        }
        return this;
    }

    /// <summary>Records a contact attempt.</summary>
    public CollectionCase RecordContact(DateOnly contactDate, DateOnly? nextFollowUp = null)
    {
        LastContactDate = contactDate;
        NextFollowUpDate = nextFollowUp;
        ContactAttempts++;
        Status = StatusInProgress;
        return this;
    }

    /// <summary>Records a payment recovery.</summary>
    public CollectionCase RecordRecovery(decimal amount)
    {
        AmountRecovered += amount;
        AmountOverdue = Math.Max(0, AmountOverdue - amount);
        
        if (AmountOverdue <= 0)
        {
            Status = StatusRecovered;
            ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            ClosureReason = "Full recovery achieved";
            QueueDomainEvent(new CollectionCaseRecovered { CaseId = Id, AmountRecovered = AmountRecovered });
        }
        return this;
    }

    /// <summary>Escalates the case to legal action.</summary>
    public CollectionCase EscalateToLegal(string reason)
    {
        Status = StatusLegal;
        Notes = $"Escalated to legal: {reason}";
        QueueDomainEvent(new CollectionCaseEscalated { CaseId = Id, Reason = reason });
        return this;
    }

    /// <summary>Closes the case with a settlement.</summary>
    public CollectionCase Settle(decimal settlementAmount, string terms)
    {
        Status = StatusSettled;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        ClosureReason = $"Settled for {settlementAmount:C}. Terms: {terms}";
        return this;
    }

    /// <summary>Closes the case.</summary>
    public CollectionCase Close(string reason)
    {
        Status = StatusClosed;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        ClosureReason = reason;
        return this;
    }

    /// <summary>Updates arrears information.</summary>
    public CollectionCase UpdateArrears(int daysPastDue, decimal amountOverdue, decimal totalOutstanding)
    {
        CurrentDaysPastDue = daysPastDue;
        AmountOverdue = amountOverdue;
        TotalOutstanding = totalOutstanding;
        Priority = CalculatePriority(daysPastDue, amountOverdue);
        Classification = CalculateClassification(daysPastDue);
        return this;
    }

    private static string CalculatePriority(int daysPastDue, decimal amountOverdue)
    {
        if (daysPastDue > 90 || amountOverdue > 100000) return PriorityCritical;
        if (daysPastDue > 60 || amountOverdue > 50000) return PriorityHigh;
        if (daysPastDue > 30 || amountOverdue > 10000) return PriorityMedium;
        return PriorityLow;
    }

    private static string CalculateClassification(int daysPastDue)
    {
        return daysPastDue switch
        {
            > 180 => ClassificationLoss,
            > 90 => ClassificationDoubtful,
            > 30 => ClassificationSubstandard,
            > 0 => ClassificationWatch,
            _ => ClassificationCurrent
        };
    }
}
