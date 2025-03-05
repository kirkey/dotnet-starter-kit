using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record AccountUpdated(Account Account) : DomainEvent
{
    public Account Account { get; } = Account;
}
