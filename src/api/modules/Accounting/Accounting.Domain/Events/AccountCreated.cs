using Accounting.Domain.Enums;
using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events;
public record AccountCreated(DefaultIdType Id, Category Category, string Code, string Name, decimal Balance, string? Description, string? Notes) : DomainEvent;
