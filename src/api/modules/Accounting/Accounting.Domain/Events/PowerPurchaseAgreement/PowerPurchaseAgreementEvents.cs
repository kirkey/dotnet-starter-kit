namespace Accounting.Domain.Events.PowerPurchaseAgreement;

public record PowerPurchaseAgreementCreated(DefaultIdType Id, string ContractNumber, string CounterpartyName, string ContractType, DateTime StartDate, DateTime EndDate, decimal EnergyPricePerKWh, string? Description) : DomainEvent;

public record PowerPurchaseAgreementUpdated(DefaultIdType Id, string ContractNumber, string CounterpartyName, string ContractType, decimal EnergyPricePerKWh, string? Description) : DomainEvent;

public record PowerPurchaseAgreementDeleted(DefaultIdType Id) : DomainEvent;

public record PowerPurchaseAgreementActivated(DefaultIdType Id, string ContractNumber, string CounterpartyName, DateTime ActivationDate) : DomainEvent;

public record PowerPurchaseAgreementSuspended(DefaultIdType Id, string ContractNumber, string CounterpartyName, string Reason) : DomainEvent;

public record PowerPurchaseAgreementReactivated(DefaultIdType Id, string ContractNumber, string CounterpartyName) : DomainEvent;

public record PowerPurchaseAgreementTerminated(DefaultIdType Id, string ContractNumber, string CounterpartyName, DateTime TerminationDate, string Reason) : DomainEvent;

public record PowerPurchaseAgreementSettled(DefaultIdType Id, string ContractNumber, string CounterpartyName, decimal EnergyKWh, decimal SettlementAmount, DateTime SettlementDate) : DomainEvent;

public record PowerPurchaseAgreementPriceEscalated(DefaultIdType Id, string ContractNumber, decimal OldPrice, decimal NewPrice, decimal EscalationRate) : DomainEvent;

public record PowerPurchaseAgreementExpired(DefaultIdType Id, string ContractNumber, string CounterpartyName, DateTime EndDate) : DomainEvent;

