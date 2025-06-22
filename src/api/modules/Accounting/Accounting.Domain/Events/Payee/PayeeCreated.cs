using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Payee;

public record PayeeCreated(
    DefaultIdType Id, string PayeeCode, string Name, string Address,
    string ExpenseAccountCode, string ExpenseAccountName, string Tin, string Description, string Notes) : DomainEvent;
