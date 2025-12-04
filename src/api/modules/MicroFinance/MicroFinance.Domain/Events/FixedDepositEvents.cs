using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>Domain event raised when a fixed deposit is created.</summary>
public sealed record FixedDepositCreated : DomainEvent
{
    public FixedDeposit? FixedDeposit { get; init; }
}

/// <summary>Domain event raised when interest is posted to a fixed deposit.</summary>
public sealed record FixedDepositInterestPosted : DomainEvent
{
    public DefaultIdType DepositId { get; init; }
    public decimal Amount { get; init; }
}

/// <summary>Domain event raised when a fixed deposit matures.</summary>
public sealed record FixedDepositMatured : DomainEvent
{
    public DefaultIdType DepositId { get; init; }
}

/// <summary>Domain event raised when a fixed deposit is renewed.</summary>
public sealed record FixedDepositRenewed : DomainEvent
{
    public DefaultIdType DepositId { get; init; }
}

/// <summary>Domain event raised when a fixed deposit is closed prematurely.</summary>
public sealed record FixedDepositPrematurelyClosed : DomainEvent
{
    public DefaultIdType DepositId { get; init; }
    public string? Reason { get; init; }
}
