namespace Accounting.Domain.Events.AccountsReceivableAccount;

public record ArAccountCreated(DefaultIdType Id, string AccountNumber, string AccountName, string? Description, string? Notes) : DomainEvent;

public record ArAccountBalanceUpdated(DefaultIdType Id, string AccountNumber, decimal CurrentBalance, decimal NetReceivables) : DomainEvent;

public record ArAccountAllowanceUpdated(DefaultIdType Id, string AccountNumber, decimal AllowanceAmount) : DomainEvent;

public record ArAccountWriteOffRecorded(DefaultIdType Id, string AccountNumber, decimal Amount, decimal YearToDateWriteOffs) : DomainEvent;

public record ArAccountCollectionRecorded(DefaultIdType Id, string AccountNumber, decimal Amount, decimal YearToDateCollections) : DomainEvent;

public record ArAccountReconciled(DefaultIdType Id, string AccountNumber, decimal CurrentBalance, decimal SubsidiaryBalance, decimal Variance, bool IsReconciled) : DomainEvent;

public record ArAccountMetricsUpdated(DefaultIdType Id, string AccountNumber, int CustomerCount, decimal DaysSalesOutstanding) : DomainEvent;

public record ArAccountDeleted(DefaultIdType Id) : DomainEvent;

