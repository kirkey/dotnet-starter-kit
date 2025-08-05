using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.ChartOfAccount;
public record ChartOfAccountCreated(
    DefaultIdType Id, string AccountId, string AccountName, string AccountType,
    string UsoaCategory, string? Description, string? Notes) : DomainEvent;
