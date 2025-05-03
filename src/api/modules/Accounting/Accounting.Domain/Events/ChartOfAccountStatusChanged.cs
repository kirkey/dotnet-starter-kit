using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record ChartOfAccountStatusChanged(DefaultIdType Id, string Status) : DomainEvent;
