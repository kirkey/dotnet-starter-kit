namespace Accounting.Domain.Events.AccountsReceivableAccount;

public record ARAccountCreated(DefaultIdType Id, string AccountNumber, string AccountName, string? Description, string? Notes) : DomainEvent;

public record ARAccountBalanceUpdated(DefaultIdType Id, string AccountNumber, decimal CurrentBalance, decimal NetReceivables) : DomainEvent;

public record ARAccountAllowanceUpdated(DefaultIdType Id, string AccountNumber, decimal AllowanceAmount) : DomainEvent;

public record ARAccountWriteOffRecorded(DefaultIdType Id, string AccountNumber, decimal Amount, decimal YearToDateWriteOffs) : DomainEvent;

public record ARAccountCollectionRecorded(DefaultIdType Id, string AccountNumber, decimal Amount, decimal YearToDateCollections) : DomainEvent;

public record ARAccountReconciled(DefaultIdType Id, string AccountNumber, decimal CurrentBalance, decimal SubsidiaryBalance, decimal Variance, bool IsReconciled) : DomainEvent;

public record ARAccountMetricsUpdated(DefaultIdType Id, string AccountNumber, int CustomerCount, decimal DaysSalesOutstanding) : DomainEvent;

public record ARAccountDeleted(DefaultIdType Id) : DomainEvent;

