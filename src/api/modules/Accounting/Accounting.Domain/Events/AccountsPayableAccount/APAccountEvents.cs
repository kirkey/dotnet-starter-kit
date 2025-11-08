namespace Accounting.Domain.Events.AccountsPayableAccount;

public record ApAccountCreated(DefaultIdType Id, string AccountNumber, string AccountName, string? Description, string? Notes) : DomainEvent;

public record ApAccountBalanceUpdated(DefaultIdType Id, string AccountNumber, decimal CurrentBalance) : DomainEvent;

public record ApAccountPaymentRecorded(DefaultIdType Id, string AccountNumber, decimal Amount, decimal YearToDatePayments) : DomainEvent;

public record ApAccountDiscountLost(DefaultIdType Id, string AccountNumber, decimal DiscountAmount, decimal YearToDateDiscountsLost) : DomainEvent;

public record ApAccountReconciled(DefaultIdType Id, string AccountNumber, decimal CurrentBalance, decimal SubsidiaryBalance, decimal Variance, bool IsReconciled) : DomainEvent;

public record ApAccountMetricsUpdated(DefaultIdType Id, string AccountNumber, int VendorCount, decimal DaysPayableOutstanding) : DomainEvent;

public record ApAccountDeleted(DefaultIdType Id) : DomainEvent;

