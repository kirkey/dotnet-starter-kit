using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Payee;

public record PayeeUpdated(Accounting.Domain.Payee Payee) : DomainEvent
{
    public Accounting.Domain.Payee Payee { get; } = Payee;
}
