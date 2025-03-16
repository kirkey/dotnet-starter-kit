using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record AccountStatusChanged(DefaultIdType Id, string Status) : DomainEvent;
