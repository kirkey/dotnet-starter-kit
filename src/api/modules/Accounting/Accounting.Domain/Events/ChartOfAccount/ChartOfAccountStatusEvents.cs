using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.ChartOfAccount;

public record ChartOfAccountActivated(DefaultIdType Id, string AccountCode, string Name) : DomainEvent;
public record ChartOfAccountDeactivated(DefaultIdType Id, string AccountCode, string Name) : DomainEvent;
