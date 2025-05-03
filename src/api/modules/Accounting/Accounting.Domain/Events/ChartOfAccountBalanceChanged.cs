using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record ChartOfAccountBalanceChanged(DefaultIdType Id, decimal NewBalance, decimal Amount, string Type) : DomainEvent;
