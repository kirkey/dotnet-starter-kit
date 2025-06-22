using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.ChartOfAccount;
public record ChartOfAccountUpdated(Domain.ChartOfAccount ChartOfAccount) : DomainEvent
{
    public Domain.ChartOfAccount ChartOfAccount { get; } = ChartOfAccount;
}
