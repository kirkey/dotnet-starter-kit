using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

// ============================================
// Fixed Deposit Events
// ============================================

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

// ============================================
// Share Product Events
// ============================================

/// <summary>Domain event raised when a share product is created.</summary>
public sealed record ShareProductCreated : DomainEvent
{
    public ShareProduct? ShareProduct { get; init; }
}

/// <summary>Domain event raised when a share product is updated.</summary>
public sealed record ShareProductUpdated : DomainEvent
{
    public ShareProduct? ShareProduct { get; init; }
}

/// <summary>Domain event raised when share product price is updated.</summary>
public sealed record ShareProductPriceUpdated : DomainEvent
{
    public DefaultIdType ProductId { get; init; }
    public decimal NewPrice { get; init; }
}

// ============================================
// Share Account Events
// ============================================

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
