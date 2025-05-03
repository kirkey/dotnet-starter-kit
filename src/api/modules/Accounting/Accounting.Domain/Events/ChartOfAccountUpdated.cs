using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record ChartOfAccountUpdated(ChartOfAccount ChartOfAccount) : DomainEvent
{
    public ChartOfAccount ChartOfAccount { get; } = ChartOfAccount;
}
