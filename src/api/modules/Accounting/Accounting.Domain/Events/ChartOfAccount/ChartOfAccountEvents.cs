namespace Accounting.Domain.Events.ChartOfAccount;

public record ChartOfAccountCreated(
    DefaultIdType Id, string AccountId, string AccountName, string AccountType,
    string UsoaCategory, string? Description, string? Notes) : DomainEvent;

public record ChartOfAccountUpdated(
    DefaultIdType Id, string AccountId, string AccountName, string AccountType,
    string UsoaCategory, string? Description, string? Notes) : DomainEvent;

public record ChartOfAccountDeleted(DefaultIdType Id) : DomainEvent;

public record ChartOfAccountBalanceChanged(DefaultIdType Id, decimal NewBalance, decimal Amount, string Type) : DomainEvent;

public record ChartOfAccountBalanceUpdated(
    DefaultIdType Id, string AccountId, decimal Balance) : DomainEvent;

public record ChartOfAccountStatusChanged(DefaultIdType Id, string AccountId, bool IsActive) : DomainEvent;
