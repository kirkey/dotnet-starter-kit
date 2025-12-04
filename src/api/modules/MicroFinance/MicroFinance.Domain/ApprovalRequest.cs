using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for ApprovalRequest entity.
/// </summary>
public static class ApprovalRequestConstants
{
    /// <summary>Maximum length for request number. (2^6 = 64)</summary>
    public const int RequestNumberMaxLength = 64;

    /// <summary>Maximum length for entity type. (2^6 = 64)</summary>
    public const int EntityTypeMaxLength = 64;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for comments. (2^11 = 2048)</summary>
    public const int CommentsMaxLength = 2048;

    /// <summary>Maximum length for rejection reason. (2^10 = 1024)</summary>
    public const int RejectionReasonMaxLength = 1024;
}

/// <summary>
/// Represents a pending approval request.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Track pending approvals for loans, write-offs, etc.</description></item>
///   <item><description>Record approver decisions with comments</description></item>
///   <item><description>Monitor approval SLAs and escalate overdue requests</description></item>
///   <item><description>Maintain audit trail of all approval decisions</description></item>
///   <item><description>Support multi-level approval chains</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Approval requests are created when an entity requires authorization.
/// Each request tracks the approval chain, decisions made at each level,
/// and provides a complete audit trail for compliance purposes.
/// </para>
/// </remarks>
public class ApprovalRequest : AuditableEntity, IAggregateRoot
{
    // Statuses
    /// <summary>Request is pending initial review.</summary>
    public const string StatusPending = "PENDING";
    /// <summary>Request is in approval process.</summary>
    public const string StatusInProgress = "IN_PROGRESS";
    /// <summary>Request is approved.</summary>
    public const string StatusApproved = "APPROVED";
    /// <summary>Request is rejected.</summary>
    public const string StatusRejected = "REJECTED";
    /// <summary>Request is cancelled.</summary>
    public const string StatusCancelled = "CANCELLED";
    /// <summary>Request is returned for modification.</summary>
    public const string StatusReturned = "RETURNED";
    /// <summary>Request has expired.</summary>
    public const string StatusExpired = "EXPIRED";

    /// <summary>Gets the unique request number.</summary>
    public string RequestNumber { get; private set; } = default!;

    /// <summary>Gets the workflow ID.</summary>
    public DefaultIdType WorkflowId { get; private set; }

    /// <summary>Gets the workflow navigation property.</summary>
    public virtual ApprovalWorkflow? Workflow { get; private set; }

    /// <summary>Gets the type of entity being approved.</summary>
    public string EntityType { get; private set; } = default!;

    /// <summary>Gets the ID of the entity being approved.</summary>
    public DefaultIdType EntityId { get; private set; }

    /// <summary>Gets the amount associated with the request.</summary>
    public decimal? Amount { get; private set; }

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the current approval level.</summary>
    public int CurrentLevel { get; private set; }

    /// <summary>Gets the total number of levels required.</summary>
    public int TotalLevels { get; private set; }

    /// <summary>Gets the date the request was submitted.</summary>
    public DateTime SubmittedAt { get; private set; }

    /// <summary>Gets the user ID who submitted the request.</summary>
    public DefaultIdType SubmittedById { get; private set; }

    /// <summary>Gets the date the request was completed.</summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>Gets the branch ID where the request originated.</summary>
    public DefaultIdType? BranchId { get; private set; }

    /// <summary>Gets the SLA due date for current level.</summary>
    public DateTime? SlaDueAt { get; private set; }

    /// <summary>Gets comments from the submitter.</summary>
    public string? Comments { get; private set; }

    /// <summary>Gets the rejection reason if rejected.</summary>
    public string? RejectionReason { get; private set; }

    /// <summary>Gets the collection of approval decisions.</summary>
    public virtual ICollection<ApprovalDecision> Decisions { get; private set; } = new List<ApprovalDecision>();

    private ApprovalRequest() { }

    private ApprovalRequest(
        DefaultIdType id,
        string requestNumber,
        DefaultIdType workflowId,
        string entityType,
        DefaultIdType entityId,
        int totalLevels,
        DefaultIdType submittedById,
        decimal? amount = null)
    {
        Id = id;
        RequestNumber = requestNumber.Trim();
        WorkflowId = workflowId;
        EntityType = entityType;
        EntityId = entityId;
        Amount = amount;
        TotalLevels = totalLevels;
        CurrentLevel = 1;
        Status = StatusPending;
        SubmittedAt = DateTime.UtcNow;
        SubmittedById = submittedById;

        QueueDomainEvent(new ApprovalRequestCreated { ApprovalRequest = this });
    }

    /// <summary>Creates a new ApprovalRequest.</summary>
    public static ApprovalRequest Create(
        string requestNumber,
        DefaultIdType workflowId,
        string entityType,
        DefaultIdType entityId,
        int totalLevels,
        DefaultIdType submittedById,
        decimal? amount = null)
    {
        return new ApprovalRequest(
            DefaultIdType.NewGuid(),
            requestNumber,
            workflowId,
            entityType,
            entityId,
            totalLevels,
            submittedById,
            amount);
    }

    /// <summary>Sets the SLA due date.</summary>
    public ApprovalRequest WithSla(int hours)
    {
        SlaDueAt = DateTime.UtcNow.AddHours(hours);
        return this;
    }

    /// <summary>Sets the branch.</summary>
    public ApprovalRequest FromBranch(DefaultIdType branchId)
    {
        BranchId = branchId;
        return this;
    }

    /// <summary>Adds comments to the request.</summary>
    public ApprovalRequest WithComments(string comments)
    {
        Comments = comments?.Trim();
        return this;
    }

    /// <summary>Approves the current level.</summary>
    public ApprovalRequest ApproveLevel(DefaultIdType approverId, string? comments = null)
    {
        if (Status != StatusPending && Status != StatusInProgress)
            throw new InvalidOperationException($"Cannot approve request in {Status} status.");

        var decision = ApprovalDecision.CreateApproval(Id, CurrentLevel, approverId, comments);
        Decisions.Add(decision);

        if (CurrentLevel >= TotalLevels)
        {
            Status = StatusApproved;
            CompletedAt = DateTime.UtcNow;
            QueueDomainEvent(new ApprovalRequestApproved { RequestId = Id, EntityId = EntityId });
        }
        else
        {
            CurrentLevel++;
            Status = StatusInProgress;
            SlaDueAt = DateTime.UtcNow.AddHours(24); // Reset SLA for next level
        }

        return this;
    }

    /// <summary>Rejects the request.</summary>
    public ApprovalRequest Reject(DefaultIdType approverId, string reason)
    {
        if (Status != StatusPending && Status != StatusInProgress)
            throw new InvalidOperationException($"Cannot reject request in {Status} status.");

        var decision = ApprovalDecision.CreateRejection(Id, CurrentLevel, approverId, reason);
        Decisions.Add(decision);

        Status = StatusRejected;
        RejectionReason = reason?.Trim();
        CompletedAt = DateTime.UtcNow;
        
        QueueDomainEvent(new ApprovalRequestRejected { RequestId = Id, EntityId = EntityId, Reason = reason });
        return this;
    }

    /// <summary>Returns the request for modification.</summary>
    public ApprovalRequest Return(DefaultIdType approverId, string reason)
    {
        if (Status != StatusPending && Status != StatusInProgress)
            throw new InvalidOperationException($"Cannot return request in {Status} status.");

        var decision = ApprovalDecision.CreateReturn(Id, CurrentLevel, approverId, reason);
        Decisions.Add(decision);

        Status = StatusReturned;
        return this;
    }

    /// <summary>Resubmits after modification.</summary>
    public ApprovalRequest Resubmit()
    {
        if (Status != StatusReturned)
            throw new InvalidOperationException($"Cannot resubmit request in {Status} status.");

        Status = StatusPending;
        CurrentLevel = 1;
        SlaDueAt = DateTime.UtcNow.AddHours(24);
        return this;
    }

    /// <summary>Cancels the request.</summary>
    public ApprovalRequest Cancel(string reason)
    {
        if (Status == StatusApproved || Status == StatusRejected)
            throw new InvalidOperationException($"Cannot cancel request in {Status} status.");

        Status = StatusCancelled;
        CompletedAt = DateTime.UtcNow;
        Comments = $"Cancelled: {reason}";
        return this;
    }

    /// <summary>Marks as expired.</summary>
    public ApprovalRequest Expire()
    {
        Status = StatusExpired;
        CompletedAt = DateTime.UtcNow;
        return this;
    }
}

/// <summary>
/// Represents an individual approval decision.
/// </summary>
public class ApprovalDecision : AuditableEntity
{
    /// <summary>Gets the request ID.</summary>
    public DefaultIdType RequestId { get; private set; }

    /// <summary>Gets the request navigation property.</summary>
    public virtual ApprovalRequest? Request { get; private set; }

    /// <summary>Gets the approval level.</summary>
    public int Level { get; private set; }

    /// <summary>Gets the approver's user ID.</summary>
    public DefaultIdType ApproverId { get; private set; }

    /// <summary>Gets the decision (APPROVED, REJECTED, RETURNED).</summary>
    public string Decision { get; private set; } = default!;

    /// <summary>Gets the decision timestamp.</summary>
    public DateTime DecisionAt { get; private set; }

    /// <summary>Gets comments from the approver.</summary>
    public string? Comments { get; private set; }

    private ApprovalDecision() { }

    public static ApprovalDecision CreateApproval(DefaultIdType requestId, int level, DefaultIdType approverId, string? comments = null)
    {
        return new ApprovalDecision
        {
            Id = DefaultIdType.NewGuid(),
            RequestId = requestId,
            Level = level,
            ApproverId = approverId,
            Decision = "APPROVED",
            DecisionAt = DateTime.UtcNow,
            Comments = comments?.Trim()
        };
    }

    public static ApprovalDecision CreateRejection(DefaultIdType requestId, int level, DefaultIdType approverId, string reason)
    {
        return new ApprovalDecision
        {
            Id = DefaultIdType.NewGuid(),
            RequestId = requestId,
            Level = level,
            ApproverId = approverId,
            Decision = "REJECTED",
            DecisionAt = DateTime.UtcNow,
            Comments = reason?.Trim()
        };
    }

    public static ApprovalDecision CreateReturn(DefaultIdType requestId, int level, DefaultIdType approverId, string reason)
    {
        return new ApprovalDecision
        {
            Id = DefaultIdType.NewGuid(),
            RequestId = requestId,
            Level = level,
            ApproverId = approverId,
            Decision = "RETURNED",
            DecisionAt = DateTime.UtcNow,
            Comments = reason?.Trim()
        };
    }
}
