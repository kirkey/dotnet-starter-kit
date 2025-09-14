namespace Accounting.Domain.Events.Payee;

public record PayeeCreated(
    DefaultIdType Id, string PayeeCode, string Name, string Address,
    string ExpenseAccountCode, string ExpenseAccountName, string Tin, string Description, string Notes) : DomainEvent;

public record PayeeUpdated(DefaultIdType Id, Accounting.Domain.Payee Payee) : DomainEvent;

public record PayeeDeleted(DefaultIdType Id) : DomainEvent;

