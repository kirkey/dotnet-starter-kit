using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.ChartOfAccount;
public record ChartOfAccountUpdated(
    DefaultIdType Id, string AccountId, string AccountName, string AccountType,
    string UsoaCategory, string? Description, string? Notes) : DomainEvent;
