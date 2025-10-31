namespace Accounting.Domain.Events.Customer;

public record CustomerCreated(DefaultIdType Id, string CustomerNumber, string CustomerName, string CustomerType, decimal CreditLimit, string? Description, string? Notes) : DomainEvent;

public record CustomerUpdated(DefaultIdType Id, string CustomerNumber, string CustomerName, string? Description, string? Notes) : DomainEvent;

public record CustomerDeleted(DefaultIdType Id) : DomainEvent;

public record CustomerActivated(DefaultIdType Id, string CustomerNumber, string CustomerName) : DomainEvent;

public record CustomerDeactivated(DefaultIdType Id, string CustomerNumber, string CustomerName) : DomainEvent;

public record CustomerCreditLimitChanged(DefaultIdType Id, string CustomerNumber, string CustomerName, decimal OldLimit, decimal NewLimit, string AuthorizedBy) : DomainEvent;

public record CustomerPlacedOnCreditHold(DefaultIdType Id, string CustomerNumber, string CustomerName, string Reason) : DomainEvent;

public record CustomerRemovedFromCreditHold(DefaultIdType Id, string CustomerNumber, string CustomerName) : DomainEvent;

public record CustomerBalanceUpdated(DefaultIdType Id, string CustomerNumber, decimal NewBalance) : DomainEvent;

