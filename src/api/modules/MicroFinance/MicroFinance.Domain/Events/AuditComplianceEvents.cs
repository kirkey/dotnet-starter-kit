using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

// ============================================================================
// Approval Workflow Events
// ============================================================================

/// <summary>Domain event raised when an approval workflow is created.</summary>
public sealed record ApprovalWorkflowCreated : DomainEvent
{
    public ApprovalWorkflow? ApprovalWorkflow { get; init; }
}

// ============================================================================
// Approval Request Events
// ============================================================================

/// <summary>Domain event raised when an approval request is created.</summary>
public sealed record ApprovalRequestCreated : DomainEvent
{
    public ApprovalRequest? ApprovalRequest { get; init; }
}

/// <summary>Domain event raised when an approval request is approved.</summary>
public sealed record ApprovalRequestApproved : DomainEvent
{
    public DefaultIdType RequestId { get; init; }
    public DefaultIdType EntityId { get; init; }
}

/// <summary>Domain event raised when an approval request is rejected.</summary>
public sealed record ApprovalRequestRejected : DomainEvent
{
    public DefaultIdType RequestId { get; init; }
    public DefaultIdType EntityId { get; init; }
    public string? Reason { get; init; }
}

// ============================================================================
// KYC Document Events
// ============================================================================

/// <summary>Domain event raised when a KYC document is uploaded.</summary>
public sealed record KycDocumentUploaded : DomainEvent
{
    public KycDocument? KycDocument { get; init; }
}

/// <summary>Domain event raised when a KYC document is verified.</summary>
public sealed record KycDocumentVerified : DomainEvent
{
    public DefaultIdType DocumentId { get; init; }
    public DefaultIdType MemberId { get; init; }
}

/// <summary>Domain event raised when a KYC document is rejected.</summary>
public sealed record KycDocumentRejected : DomainEvent
{
    public DefaultIdType DocumentId { get; init; }
    public DefaultIdType MemberId { get; init; }
    public string? Reason { get; init; }
}

/// <summary>Domain event raised when a KYC document expires.</summary>
public sealed record KycDocumentExpired : DomainEvent
{
    public DefaultIdType DocumentId { get; init; }
    public DefaultIdType MemberId { get; init; }
}

// ============================================================================
// AML Alert Events
// ============================================================================

/// <summary>Domain event raised when an AML alert is created.</summary>
public sealed record AmlAlertCreated : DomainEvent
{
    public AmlAlert? AmlAlert { get; init; }
}

/// <summary>Domain event raised when an AML alert is escalated.</summary>
public sealed record AmlAlertEscalated : DomainEvent
{
    public DefaultIdType AlertId { get; init; }
    public string? Reason { get; init; }
}

/// <summary>Domain event raised when an AML alert is confirmed suspicious.</summary>
public sealed record AmlAlertConfirmed : DomainEvent
{
    public DefaultIdType AlertId { get; init; }
    public DefaultIdType? MemberId { get; init; }
}

/// <summary>Domain event raised when a SAR is filed.</summary>
public sealed record SarFiled : DomainEvent
{
    public DefaultIdType AlertId { get; init; }
    public string? SarReference { get; init; }
}
