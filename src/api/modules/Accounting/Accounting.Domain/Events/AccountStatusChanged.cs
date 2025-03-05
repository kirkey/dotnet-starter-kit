using Accounting.Domain.Enums;
using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record AccountStatusChanged(DefaultIdType Id, AccountStatus Status) : DomainEvent;
