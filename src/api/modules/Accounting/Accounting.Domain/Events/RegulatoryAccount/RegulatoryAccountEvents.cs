namespace Accounting.Domain.Events.RegulatoryAccount;

public record RegulatoryAccountCreated(DefaultIdType Id, string AccountCode, string AccountName, string? Description, string? Notes) : DomainEvent;

public record RegulatoryAccountUpdated(DefaultIdType Id) : DomainEvent;

public record RegulatoryAccountDeleted(DefaultIdType Id) : DomainEvent;
