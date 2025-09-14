using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.PowerPurchaseAgreement;

public record PowerPurchaseAgreementCreated(DefaultIdType Id, string AgreementNumber, DateTime StartDate, DateTime EndDate, decimal Amount, string? Description) : DomainEvent;

public record PowerPurchaseAgreementUpdated(DefaultIdType Id, string AgreementNumber, DateTime StartDate, DateTime EndDate, decimal Amount, string? Description) : DomainEvent;

public record PowerPurchaseAgreementDeleted(DefaultIdType Id) : DomainEvent;

