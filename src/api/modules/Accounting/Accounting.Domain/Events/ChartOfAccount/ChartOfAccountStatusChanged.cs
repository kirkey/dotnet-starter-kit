using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.ChartOfAccount;
public record ChartOfAccountStatusChanged(DefaultIdType Id, string AccountId, bool IsActive) : DomainEvent;
