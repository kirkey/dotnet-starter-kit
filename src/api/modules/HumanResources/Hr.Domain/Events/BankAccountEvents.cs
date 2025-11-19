using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when a bank account is created.
/// </summary>
public record BankAccountCreated : DomainEvent
{
    public BankAccount BankAccount { get; init; } = default!;
}

/// <summary>
/// Event raised when a bank account is updated.
/// </summary>
public record BankAccountUpdated : DomainEvent
{
    public BankAccount BankAccount { get; init; } = default!;
}

/// <summary>
/// Event raised when a bank account is deactivated.
/// </summary>
public record BankAccountDeactivated : DomainEvent
{
    public DefaultIdType BankAccountId { get; init; }
}

/// <summary>
/// Event raised when a bank account is activated.
/// </summary>
public record BankAccountActivated : DomainEvent
{
    public DefaultIdType BankAccountId { get; init; }
}

