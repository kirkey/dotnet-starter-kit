namespace Accounting.Domain.Events.AccountsPayableAccount;

public record APAccountCreated(DefaultIdType Id, string AccountNumber, string AccountName, string? Description, string? Notes) : DomainEvent;

public record APAccountBalanceUpdated(DefaultIdType Id, string AccountNumber, decimal CurrentBalance) : DomainEvent;

public record APAccountPaymentRecorded(DefaultIdType Id, string AccountNumber, decimal Amount, decimal YearToDatePayments) : DomainEvent;

public record APAccountDiscountLost(DefaultIdType Id, string AccountNumber, decimal DiscountAmount, decimal YearToDateDiscountsLost) : DomainEvent;

public record APAccountReconciled(DefaultIdType Id, string AccountNumber, decimal CurrentBalance, decimal SubsidiaryBalance, decimal Variance, bool IsReconciled) : DomainEvent;

public record APAccountMetricsUpdated(DefaultIdType Id, string AccountNumber, int VendorCount, decimal DaysPayableOutstanding) : DomainEvent;

public record APAccountDeleted(DefaultIdType Id) : DomainEvent;

