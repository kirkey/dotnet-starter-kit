namespace Accounting.Domain.Events.ChartOfAccount;

public record ChartOfAccountActivated(DefaultIdType Id, string AccountCode, string Name) : DomainEvent;
public record ChartOfAccountDeactivated(DefaultIdType Id, string AccountCode, string Name) : DomainEvent;
