using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for PromiseToPay entity.
/// </summary>
public static class PromiseToPayConstants
{
    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes. (2^11 = 2048)</summary>
    public const int NotesMaxLength = 2048;

    /// <summary>Maximum length for payment method. (2^5 = 32)</summary>
    public const int PaymentMethodMaxLength = 32;

    /// <summary>Maximum length for breach reason. (2^9 = 512)</summary>
    public const int BreachReasonMaxLength = 512;
}

/// <summary>
/// Represents a promise to pay made by a delinquent borrower.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Record borrower commitments to repay on specific dates</description></item>
///   <item><description>Track promise fulfillment rates for collection effectiveness</description></item>
///   <item><description>Schedule follow-up actions based on promise dates</description></item>
///   <item><description>Analyze patterns in broken promises for risk assessment</description></item>
///   <item><description>Support escalation decisions when promises are not kept</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Promises to pay are formal commitments made by borrowers during collection activities.
/// Tracking promise fulfillment helps measure collection effectiveness and identify
/// borrowers who repeatedly break commitments (indicating higher risk).
/// </para>
/// </remarks>
public class PromiseToPay : AuditableEntity, IAggregateRoot
{
    // Promise Statuses
    /// <summary>Promise is pending, not yet due.</summary>
    public const string StatusPending = "PENDING";
    /// <summary>Promise was fulfilled - payment received.</summary>
    public const string StatusKept = "KEPT";
    /// <summary>Promise was partially fulfilled.</summary>
    public const string StatusPartial = "PARTIAL";
    /// <summary>Promise was broken - no payment received.</summary>
    public const string StatusBroken = "BROKEN";
    /// <summary>Promise was rescheduled.</summary>
    public const string StatusRescheduled = "RESCHEDULED";
    /// <summary>Promise was cancelled.</summary>
    public const string StatusCancelled = "CANCELLED";

    /// <summary>Gets the collection case ID.</summary>
    public DefaultIdType CollectionCaseId { get; private set; }

    /// <summary>Gets the collection case navigation property.</summary>
    public virtual CollectionCase? CollectionCase { get; private set; }

    /// <summary>Gets the loan ID.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan? Loan { get; private set; }

    /// <summary>Gets the member ID who made the promise.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member? Member { get; private set; }

    /// <summary>Gets the collection action ID that generated this promise.</summary>
    public DefaultIdType? CollectionActionId { get; private set; }

    /// <summary>Gets the date the promise was made.</summary>
    public DateOnly PromiseDate { get; private set; }

    /// <summary>Gets the promised payment date.</summary>
    public DateOnly PromisedPaymentDate { get; private set; }

    /// <summary>Gets the promised amount.</summary>
    public decimal PromisedAmount { get; private set; }

    /// <summary>Gets the actual amount paid.</summary>
    public decimal ActualAmountPaid { get; private set; }

    /// <summary>Gets the date payment was received.</summary>
    public DateOnly? ActualPaymentDate { get; private set; }

    /// <summary>Gets the current status of the promise.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the expected payment method.</summary>
    public string? PaymentMethod { get; private set; }

    /// <summary>Gets the reason for breach if promise was broken.</summary>
    public string? BreachReason { get; private set; }

    /// <summary>Gets the number of times this promise was rescheduled.</summary>
    public int RescheduleCount { get; private set; }

    /// <summary>Gets the staff ID who recorded the promise.</summary>
    public DefaultIdType RecordedById { get; private set; }

    private PromiseToPay() { }

    private PromiseToPay(
        DefaultIdType id,
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        DefaultIdType memberId,
        DateOnly promisedPaymentDate,
        decimal promisedAmount,
        DefaultIdType recordedById,
        DefaultIdType? collectionActionId = null)
    {
        Id = id;
        CollectionCaseId = collectionCaseId;
        LoanId = loanId;
        MemberId = memberId;
        CollectionActionId = collectionActionId;
        PromiseDate = DateOnly.FromDateTime(DateTime.UtcNow);
        PromisedPaymentDate = promisedPaymentDate;
        PromisedAmount = promisedAmount;
        ActualAmountPaid = 0;
        Status = StatusPending;
        RescheduleCount = 0;
        RecordedById = recordedById;

        QueueDomainEvent(new PromiseToPayCreated { PromiseToPay = this });
    }

    /// <summary>Creates a new PromiseToPay.</summary>
    public static PromiseToPay Create(
        DefaultIdType collectionCaseId,
        DefaultIdType loanId,
        DefaultIdType memberId,
        DateOnly promisedPaymentDate,
        decimal promisedAmount,
        DefaultIdType recordedById,
        DefaultIdType? collectionActionId = null)
    {
        return new PromiseToPay(
            DefaultIdType.NewGuid(),
            collectionCaseId,
            loanId,
            memberId,
            promisedPaymentDate,
            promisedAmount,
            recordedById,
            collectionActionId);
    }

    /// <summary>Records full payment and marks promise as kept.</summary>
    public PromiseToPay RecordPayment(decimal amount, DateOnly paymentDate)
    {
        ActualAmountPaid += amount;
        ActualPaymentDate = paymentDate;

        if (ActualAmountPaid >= PromisedAmount)
        {
            Status = StatusKept;
            QueueDomainEvent(new PromiseToPayKept { PromiseId = Id, AmountPaid = ActualAmountPaid });
        }
        else
        {
            Status = StatusPartial;
        }
        return this;
    }

    /// <summary>Marks the promise as broken.</summary>
    public PromiseToPay MarkAsBroken(string reason)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot mark promise as broken when status is {Status}.");

        Status = StatusBroken;
        BreachReason = reason?.Trim();
        QueueDomainEvent(new PromiseToPayBroken { PromiseId = Id, Reason = reason });
        return this;
    }

    /// <summary>Reschedules the promise to a new date.</summary>
    public PromiseToPay Reschedule(DateOnly newDate, string? reason = null)
    {
        if (Status != StatusPending && Status != StatusBroken)
            throw new InvalidOperationException($"Cannot reschedule promise when status is {Status}.");

        PromisedPaymentDate = newDate;
        Status = StatusRescheduled;
        RescheduleCount++;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = $"Rescheduled ({RescheduleCount}): {reason}";
        }
        return this;
    }

    /// <summary>Cancels the promise.</summary>
    public PromiseToPay Cancel(string reason)
    {
        Status = StatusCancelled;
        Notes = $"Cancelled: {reason}";
        return this;
    }

    /// <summary>Sets the expected payment method.</summary>
    public PromiseToPay WithPaymentMethod(string method)
    {
        PaymentMethod = method?.Trim();
        return this;
    }

    /// <summary>Adds notes to the promise.</summary>
    public PromiseToPay WithNotes(string notes)
    {
        Notes = notes?.Trim();
        return this;
    }
}
