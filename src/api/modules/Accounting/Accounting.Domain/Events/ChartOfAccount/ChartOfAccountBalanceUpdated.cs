using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.ChartOfAccount;
public record ChartOfAccountBalanceUpdated(
    DefaultIdType Id, string AccountId, decimal Balance) : DomainEvent;
