using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record AccountCreated(
    DefaultIdType Id, string AccountCategory, string Type,
    string Code, string Name, decimal Balance,
    string? Description, string? Notes) : DomainEvent;
