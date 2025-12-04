using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

// ============================================================================
// Collection Case Events
// ============================================================================

/// <summary>Domain event raised when a collection case is created.</summary>
public sealed record CollectionCaseCreated : DomainEvent
{
    public CollectionCase? CollectionCase { get; init; }
}

/// <summary>Domain event raised when a collection case is assigned to a collector.</summary>
public sealed record CollectionCaseAssigned : DomainEvent
{
    public DefaultIdType CaseId { get; init; }
    public DefaultIdType CollectorId { get; init; }
}

/// <summary>Domain event raised when a collection case is recovered.</summary>
public sealed record CollectionCaseRecovered : DomainEvent
{
    public DefaultIdType CaseId { get; init; }
    public decimal AmountRecovered { get; init; }
}

/// <summary>Domain event raised when a collection case is escalated.</summary>
public sealed record CollectionCaseEscalated : DomainEvent
{
    public DefaultIdType CaseId { get; init; }
    public string? Reason { get; init; }
}

// ============================================================================
// Collection Action Events
// ============================================================================

/// <summary>Domain event raised when a collection action is recorded.</summary>
public sealed record CollectionActionRecorded : DomainEvent
{
    public CollectionAction? CollectionAction { get; init; }
}

// ============================================================================
// Promise to Pay Events
// ============================================================================

/// <summary>Domain event raised when a promise to pay is created.</summary>
public sealed record PromiseToPayCreated : DomainEvent
{
    public PromiseToPay? PromiseToPay { get; init; }
}

/// <summary>Domain event raised when a promise to pay is kept.</summary>
public sealed record PromiseToPayKept : DomainEvent
{
    public DefaultIdType PromiseId { get; init; }
    public decimal AmountPaid { get; init; }
}

/// <summary>Domain event raised when a promise to pay is broken.</summary>
public sealed record PromiseToPayBroken : DomainEvent
{
    public DefaultIdType PromiseId { get; init; }
    public string? Reason { get; init; }
}

// ============================================================================
// Legal Action Events
// ============================================================================

/// <summary>Domain event raised when a legal action is initiated.</summary>
public sealed record LegalActionInitiated : DomainEvent
{
    public LegalAction? LegalAction { get; init; }
}

/// <summary>Domain event raised when a legal action judgment is won.</summary>
public sealed record LegalActionJudgmentWon : DomainEvent
{
    public DefaultIdType LegalActionId { get; init; }
    public decimal JudgmentAmount { get; init; }
}

// ============================================================================
// Debt Settlement Events
// ============================================================================

/// <summary>Domain event raised when a debt settlement is proposed.</summary>
public sealed record DebtSettlementProposed : DomainEvent
{
    public DebtSettlement? DebtSettlement { get; init; }
}

/// <summary>Domain event raised when a debt settlement is approved.</summary>
public sealed record DebtSettlementApproved : DomainEvent
{
    public DefaultIdType SettlementId { get; init; }
    public DefaultIdType ApprovedById { get; init; }
}

/// <summary>Domain event raised when a debt settlement is completed.</summary>
public sealed record DebtSettlementCompleted : DomainEvent
{
    public DefaultIdType SettlementId { get; init; }
    public decimal TotalPaid { get; init; }
}

// ============================================================================
// Collection Strategy Events
// ============================================================================

/// <summary>Domain event raised when a collection strategy is created.</summary>
public sealed record CollectionStrategyCreated : DomainEvent
{
    public CollectionStrategy? CollectionStrategy { get; init; }
}
