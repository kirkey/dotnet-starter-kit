namespace Accounting.Domain.Events.GeneralLedger;

public record GeneralLedgerEntryCreated(
    DefaultIdType Id, DefaultIdType EntryId, DefaultIdType AccountId,
    decimal Debit, decimal Credit, string UsoaClass, DateTime TransactionDate) : DomainEvent;

public record GeneralLedgerEntryUpdated(
    DefaultIdType Id, DefaultIdType EntryId, DefaultIdType AccountId,
    decimal Debit, decimal Credit, string UsoaClass) : DomainEvent;

public record GeneralLedgerEntryDeleted(DefaultIdType EntryId) : DomainEvent;
