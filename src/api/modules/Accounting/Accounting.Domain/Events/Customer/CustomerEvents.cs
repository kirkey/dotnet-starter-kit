using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Customer;

public record CustomerCreated(DefaultIdType Id, string CustomerCode, string Name, string? Email, string? Terms, decimal CreditLimit, string? Description, string? Notes) : DomainEvent;

public record CustomerUpdated(Accounting.Domain.Customer Customer) : DomainEvent;

public record CustomerDeleted(DefaultIdType Id) : DomainEvent;

public record CustomerBalanceChanged(DefaultIdType Id, decimal NewBalance, decimal ChangeAmount, string TransactionType, string? Reference) : DomainEvent;

public record CustomerCreditLimitChanged(DefaultIdType Id, decimal OldLimit, decimal NewLimit) : DomainEvent;

public record CustomerActivated(DefaultIdType Id, string CustomerCode, string Name) : DomainEvent;

public record CustomerDeactivated(DefaultIdType Id, string CustomerCode, string Name) : DomainEvent;
