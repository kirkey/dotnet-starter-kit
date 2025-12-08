using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>Domain event raised when a new savings product is created.</summary>
public sealed record SavingsProductCreated : DomainEvent
{
    public SavingsProduct? SavingsProduct { get; init; }
}

/// <summary>Domain event raised when a savings product is updated.</summary>
public sealed record SavingsProductUpdated : DomainEvent
{
    public SavingsProduct? SavingsProduct { get; init; }
}

/// <summary>Domain event raised when a new savings account is created.</summary>
public sealed record SavingsAccountCreated : DomainEvent
{
    public SavingsAccount? SavingsAccount { get; init; }
}

/// <summary>Domain event raised when money is deposited to a savings account.</summary>
public sealed record SavingsDeposited : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
    public decimal Amount { get; init; }
}

/// <summary>Domain event raised when money is withdrawn from a savings account.</summary>
public sealed record SavingsWithdrawn : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
    public decimal Amount { get; init; }
}

/// <summary>Domain event raised when interest is posted to a savings account.</summary>
public sealed record SavingsInterestPosted : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
    public decimal Amount { get; init; }
}

/// <summary>Domain event raised when a savings account is closed.</summary>
public sealed record SavingsAccountClosed : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
}

/// <summary>Domain event raised when a savings transaction is created.</summary>
public sealed record SavingsTransactionCreated : DomainEvent
{
    public SavingsTransaction? SavingsTransaction { get; init; }
}

/// <summary>Domain event raised when a savings account is activated.</summary>
public sealed record SavingsAccountActivated : DomainEvent
{
    public DefaultIdType AccountId { get; init; }
}

