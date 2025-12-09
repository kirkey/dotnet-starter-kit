using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

// ============================================
// Fee Definition Events
// ============================================

/// <summary>Domain event raised when a fee definition is created.</summary>
public sealed record FeeDefinitionCreated : DomainEvent
{
    public FeeDefinition? FeeDefinition { get; init; }
}

/// <summary>Domain event raised when a fee definition is updated.</summary>
public sealed record FeeDefinitionUpdated : DomainEvent
{
    public FeeDefinition? FeeDefinition { get; init; }
}

// ============================================
// Fee Charge Events
// ============================================

/// <summary>Domain event raised when a fee is charged.</summary>
public sealed record FeeChargeCreated : DomainEvent
{
    public FeeCharge? FeeCharge { get; init; }
}

/// <summary>Domain event raised when a fee charge is paid.</summary>
public sealed record FeeChargePaid : DomainEvent
{
    public DefaultIdType FeeChargeId { get; init; }
}

/// <summary>Domain event raised when a fee charge is waived.</summary>
public sealed record FeeChargeWaived : DomainEvent
{
    public DefaultIdType FeeChargeId { get; init; }
}

/// <summary>Domain event raised when a fee charge is reversed.</summary>
public sealed record FeeChargeReversed : DomainEvent
{
    public DefaultIdType FeeChargeId { get; init; }
}

// ============================================
// Fee Payment Events
// ============================================

/// <summary>Domain event raised when a fee payment is created.</summary>
public sealed record FeePaymentCreated(FeePayment FeePayment) : DomainEvent;

/// <summary>Domain event raised when a fee payment is updated.</summary>
public sealed record FeePaymentUpdated(FeePayment FeePayment) : DomainEvent;

/// <summary>Domain event raised when a fee payment is reversed.</summary>
public sealed record FeePaymentReversed(Guid FeePaymentId, string Reason) : DomainEvent;

// ============================================
// Fee Waiver Events
// ============================================

/// <summary>Domain event raised when a fee waiver is created.</summary>
public sealed record FeeWaiverCreated(FeeWaiver FeeWaiver) : DomainEvent;

/// <summary>Domain event raised when a fee waiver is updated.</summary>
public sealed record FeeWaiverUpdated(FeeWaiver FeeWaiver) : DomainEvent;

/// <summary>Domain event raised when a fee waiver is approved.</summary>
public sealed record FeeWaiverApproved(Guid FeeWaiverId, Guid ApprovedByUserId, decimal WaivedAmount) : DomainEvent;

/// <summary>Domain event raised when a fee waiver is rejected.</summary>
public sealed record FeeWaiverRejected(Guid FeeWaiverId, Guid RejectedByUserId, string Reason) : DomainEvent;

/// <summary>Domain event raised when a fee waiver is cancelled.</summary>
public sealed record FeeWaiverCancelled(Guid FeeWaiverId) : DomainEvent;
