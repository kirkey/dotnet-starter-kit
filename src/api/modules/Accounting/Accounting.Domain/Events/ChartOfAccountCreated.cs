using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record ChartOfAccountCreated(
    DefaultIdType Id, string AccountCategory, string AccountType,
    string AccountCode, string Name, decimal Balance,
    string? Description, string? Notes) : DomainEvent;
