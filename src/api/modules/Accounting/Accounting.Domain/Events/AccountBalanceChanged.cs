using Accounting.Domain.Enums;
using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record AccountBalanceChanged(DefaultIdType Id, decimal NewBalance, decimal Amount, TransactionType Type) : DomainEvent;
