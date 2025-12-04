using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>Domain event raised when a share account is created.</summary>
public sealed record ShareAccountCreated : DomainEvent
{
    public ShareAccount? ShareAccount { get; init; }
}

/// <summary>Domain event raised when shares are purchased.</summary>
public sealed record SharesPurchased : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
    public int Shares { get; init; }
    public decimal Amount { get; init; }
}

/// <summary>Domain event raised when shares are redeemed.</summary>
public sealed record SharesRedeemed : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
    public int Shares { get; init; }
    public decimal Amount { get; init; }
}

/// <summary>Domain event raised when dividend is posted to shares.</summary>
public sealed record ShareDividendPosted : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
    public decimal Amount { get; init; }
}

/// <summary>Domain event raised when a share account is closed.</summary>
public sealed record ShareAccountClosed : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
}

/// <summary>Domain event raised when a share transaction is created.</summary>
public sealed record ShareTransactionCreated : DomainEvent
{
    public ShareTransaction? ShareTransaction { get; init; }
}
