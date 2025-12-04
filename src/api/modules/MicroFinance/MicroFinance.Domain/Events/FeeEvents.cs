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
