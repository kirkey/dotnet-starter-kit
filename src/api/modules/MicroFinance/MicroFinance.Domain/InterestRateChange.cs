using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a change to a loan's interest rate after disbursement.
/// Tracks rate modifications with approval workflow and effective dates.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Apply promotional rate reductions for loyal customers</description></item>
///   <item><description>Increase rates as penalty for chronic late payment</description></item>
///   <item><description>Adjust rates for regulatory or market changes</description></item>
///   <item><description>Reduce rates as part of loan restructuring</description></item>
///   <item><description>Maintain audit trail of all rate modifications</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Interest rate changes must be carefully controlled and documented:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Regulatory</strong>: Must comply with usury laws and rate caps</description></item>
///   <item><description><strong>Contractual</strong>: Changes may require borrower consent</description></item>
///   <item><description><strong>Recalculation</strong>: Triggers repayment schedule regeneration</description></item>
///   <item><description><strong>Approval</strong>: Typically requires management approval</description></item>
/// </list>
/// <para><strong>Change Types:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Promotion</strong>: Rate reduction as reward/incentive</description></item>
///   <item><description><strong>Penalty</strong>: Rate increase for default/late payment</description></item>
///   <item><description><strong>MarketAdjustment</strong>: Response to base rate changes</description></item>
///   <item><description><strong>Restructure</strong>: Rate change as part of loan restructuring</description></item>
///   <item><description><strong>RegulatoryChange</strong>: Compliance with new regulations</description></item>
///   <item><description><strong>Goodwill</strong>: Exception approved by management</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Loan"/> - Loan being modified</description></item>
///   <item><description><see cref="LoanSchedule"/> - Regenerated after rate change</description></item>
///   <item><description><see cref="ApprovalRequest"/> - Rate change approval workflow</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Requesting a promotional rate reduction</strong></para>
/// <code>
/// POST /api/microfinance/interest-rate-changes
/// {
///   "loanId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "changeType": "Promotion",
///   "previousRate": 24.0,
///   "newRate": 20.0,
///   "effectiveDate": "2024-02-01",
///   "changeReason": "Loyalty discount for perfect 2-year payment history"
/// }
/// </code>
/// </example>
public class InterestRateChange : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Max Lengths
    public const int ReferenceMaxLength = 64;
    public const int ChangeReasonMaxLength = 512;
    public const int ChangeTypeMaxLength = 32;
    public const int ApprovedByMaxLength = 128;
    public const int StatusMaxLength = 32;
    public const int RejectionReasonMaxLength = 512;
    public const int NotesMaxLength = 2048;

    // Change Types
    public const string TypePromotion = "Promotion";
    public const string TypePenalty = "Penalty";
    public const string TypeMarketAdjustment = "MarketAdjustment";
    public const string TypeRestructure = "Restructure";
    public const string TypeRegulatoryChange = "RegulatoryChange";
    public const string TypeGoodwill = "Goodwill";

    // Statuses
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusRejected = "Rejected";
    public const string StatusApplied = "Applied";
    public const string StatusCancelled = "Cancelled";

    /// <summary>FK to Loan being modified.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Unique change reference number.</summary>
    public string Reference { get; private set; } = default!;

    /// <summary>Type of rate change.</summary>
    public string ChangeType { get; private set; } = default!;

    /// <summary>Date change was requested.</summary>
    public DateOnly RequestDate { get; private set; }

    /// <summary>Date change takes effect.</summary>
    public DateOnly EffectiveDate { get; private set; }

    /// <summary>Previous interest rate (annual %).</summary>
    public decimal PreviousRate { get; private set; }

    /// <summary>New interest rate (annual %).</summary>
    public decimal NewRate { get; private set; }

    /// <summary>Rate change amount (positive = increase, negative = decrease).</summary>
    public decimal RateChange => NewRate - PreviousRate;

    /// <summary>Reason for the rate change.</summary>
    public string ChangeReason { get; private set; } = default!;

    /// <summary>Current status.</summary>
    public string Status { get; private set; } = StatusPending;

    /// <summary>User ID who approved/rejected.</summary>
    public DefaultIdType? ApprovedByUserId { get; private set; }

    /// <summary>Name of approver.</summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>Date of approval/rejection.</summary>
    public DateOnly? ApprovalDate { get; private set; }

    /// <summary>Date change was applied to loan.</summary>
    public DateOnly? AppliedDate { get; private set; }

    /// <summary>Rejection reason if rejected.</summary>
    public string? RejectionReason { get; private set; }

    private InterestRateChange() { }

    private InterestRateChange(
        DefaultIdType id,
        DefaultIdType loanId,
        string reference,
        string changeType,
        DateOnly requestDate,
        DateOnly effectiveDate,
        decimal previousRate,
        decimal newRate,
        string changeReason,
        string? notes)
    {
        Id = id;
        LoanId = loanId;
        Reference = reference.Trim();
        ChangeType = changeType.Trim();
        RequestDate = requestDate;
        EffectiveDate = effectiveDate;
        PreviousRate = previousRate;
        NewRate = newRate;
        ChangeReason = changeReason.Trim();
        Status = StatusPending;
        Notes = notes?.Trim();

        QueueDomainEvent(new InterestRateChangeCreated(this));
    }

    public static InterestRateChange Create(
        DefaultIdType loanId,
        string reference,
        string changeType,
        DateOnly effectiveDate,
        decimal previousRate,
        decimal newRate,
        string changeReason,
        DateOnly? requestDate = null,
        string? notes = null)
    {
        return new InterestRateChange(
            DefaultIdType.NewGuid(),
            loanId,
            reference,
            changeType,
            requestDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            effectiveDate,
            previousRate,
            newRate,
            changeReason,
            notes);
    }

    public InterestRateChange Update(
        string? changeType = null,
        DateOnly? effectiveDate = null,
        decimal? newRate = null,
        string? changeReason = null,
        string? notes = null)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot update rate change in {Status} status.");

        if (!string.IsNullOrWhiteSpace(changeType))
        {
            ChangeType = changeType.Trim();
        }

        if (effectiveDate.HasValue)
        {
            EffectiveDate = effectiveDate.Value;
        }

        if (newRate.HasValue && newRate.Value >= 0)
        {
            NewRate = newRate.Value;
        }

        if (!string.IsNullOrWhiteSpace(changeReason))
        {
            ChangeReason = changeReason.Trim();
        }

        Notes = notes?.Trim() ?? Notes;

        QueueDomainEvent(new InterestRateChangeUpdated(this));
        return this;
    }

    public InterestRateChange Approve(DefaultIdType userId, string approverName)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot approve rate change in {Status} status.");

        ApprovedByUserId = userId;
        ApprovedBy = approverName.Trim();
        ApprovalDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusApproved;

        QueueDomainEvent(new InterestRateChangeApproved(Id, userId, NewRate));
        return this;
    }

    public InterestRateChange Reject(DefaultIdType userId, string approverName, string reason)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot reject rate change in {Status} status.");

        ApprovedByUserId = userId;
        ApprovedBy = approverName.Trim();
        ApprovalDate = DateOnly.FromDateTime(DateTime.UtcNow);
        RejectionReason = reason.Trim();
        Status = StatusRejected;

        QueueDomainEvent(new InterestRateChangeRejected(Id, userId, reason));
        return this;
    }

    public InterestRateChange Apply()
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException($"Cannot apply rate change in {Status} status.");

        AppliedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusApplied;

        QueueDomainEvent(new InterestRateChangeApplied(Id, LoanId, NewRate));
        return this;
    }

    public InterestRateChange Cancel()
    {
        if (Status == StatusApplied)
            throw new InvalidOperationException("Cannot cancel an applied rate change.");

        Status = StatusCancelled;

        QueueDomainEvent(new InterestRateChangeCancelled(Id));
        return this;
    }
}
