using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a formal fee waiver request with approval workflow.
/// Used when management forgives all or part of a fee charge.
/// </summary>
public class FeeWaiver : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Max Lengths
    public const int ReferenceMaxLength = 64;
    public const int WaiverReasonMaxLength = 512;
    public const int WaiverTypeMaxLength = 32;
    public const int ApprovedByMaxLength = 128;
    public const int StatusMaxLength = 32;
    public const int RejectionReasonMaxLength = 512;
    public const int NotesMaxLength = 2048;

    // Waiver Types
    public const string TypeFull = "Full";
    public const string TypePartial = "Partial";

    // Statuses
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusRejected = "Rejected";
    public const string StatusCancelled = "Cancelled";

    /// <summary>FK to FeeCharge being waived.</summary>
    public DefaultIdType FeeChargeId { get; private set; }

    /// <summary>Unique waiver reference number.</summary>
    public string Reference { get; private set; } = default!;

    /// <summary>Full or Partial waiver.</summary>
    public string WaiverType { get; private set; } = default!;

    /// <summary>Date waiver was requested.</summary>
    public DateOnly RequestDate { get; private set; }

    /// <summary>Original fee amount.</summary>
    public decimal OriginalAmount { get; private set; }

    /// <summary>Amount being waived.</summary>
    public decimal WaivedAmount { get; private set; }

    /// <summary>Remaining amount after waiver (for partial).</summary>
    public decimal RemainingAmount => OriginalAmount - WaivedAmount;

    /// <summary>Reason for waiver request.</summary>
    public string WaiverReason { get; private set; } = default!;

    /// <summary>Current status.</summary>
    public string Status { get; private set; } = StatusPending;

    /// <summary>User ID who approved/rejected.</summary>
    public DefaultIdType? ApprovedByUserId { get; private set; }

    /// <summary>Name of approver.</summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>Date of approval/rejection.</summary>
    public DateOnly? ApprovalDate { get; private set; }

    /// <summary>Rejection reason if rejected.</summary>
    public string? RejectionReason { get; private set; }

    private FeeWaiver() { }

    private FeeWaiver(
        DefaultIdType id,
        DefaultIdType feeChargeId,
        string reference,
        string waiverType,
        DateOnly requestDate,
        decimal originalAmount,
        decimal waivedAmount,
        string waiverReason,
        string? notes)
    {
        Id = id;
        FeeChargeId = feeChargeId;
        Reference = reference.Trim();
        WaiverType = waiverType.Trim();
        RequestDate = requestDate;
        OriginalAmount = originalAmount;
        WaivedAmount = waivedAmount;
        WaiverReason = waiverReason.Trim();
        Status = StatusPending;
        Notes = notes?.Trim();

        QueueDomainEvent(new FeeWaiverCreated(this));
    }

    public static FeeWaiver Create(
        DefaultIdType feeChargeId,
        string reference,
        decimal originalAmount,
        decimal waivedAmount,
        string waiverReason,
        DateOnly? requestDate = null,
        string? notes = null)
    {
        var waiverType = waivedAmount >= originalAmount ? TypeFull : TypePartial;

        return new FeeWaiver(
            DefaultIdType.NewGuid(),
            feeChargeId,
            reference,
            waiverType,
            requestDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            originalAmount,
            waivedAmount,
            waiverReason,
            notes);
    }

    public FeeWaiver Update(
        decimal? waivedAmount = null,
        string? waiverReason = null,
        string? notes = null)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot update waiver in {Status} status.");

        if (waivedAmount.HasValue && waivedAmount.Value > 0)
        {
            WaivedAmount = waivedAmount.Value;
            WaiverType = WaivedAmount >= OriginalAmount ? TypeFull : TypePartial;
        }

        if (!string.IsNullOrWhiteSpace(waiverReason))
        {
            WaiverReason = waiverReason.Trim();
        }

        Notes = notes?.Trim() ?? Notes;

        QueueDomainEvent(new FeeWaiverUpdated(this));
        return this;
    }

    public FeeWaiver Approve(DefaultIdType userId, string approverName)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot approve waiver in {Status} status.");

        ApprovedByUserId = userId;
        ApprovedBy = approverName.Trim();
        ApprovalDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusApproved;

        QueueDomainEvent(new FeeWaiverApproved(Id, userId, WaivedAmount));
        return this;
    }

    public FeeWaiver Reject(DefaultIdType userId, string approverName, string reason)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot reject waiver in {Status} status.");

        ApprovedByUserId = userId;
        ApprovedBy = approverName.Trim();
        ApprovalDate = DateOnly.FromDateTime(DateTime.UtcNow);
        RejectionReason = reason.Trim();
        Status = StatusRejected;

        QueueDomainEvent(new FeeWaiverRejected(Id, userId, reason));
        return this;
    }

    public FeeWaiver Cancel()
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot cancel waiver in {Status} status.");

        Status = StatusCancelled;

        QueueDomainEvent(new FeeWaiverCancelled(Id));
        return this;
    }
}
