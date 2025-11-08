namespace Accounting.Domain.Events.InterconnectionAgreement;

public record InterconnectionAgreementCreated(DefaultIdType Id, string AgreementNumber, DefaultIdType MemberId, string GenerationType, decimal InstalledCapacityKw, DateTime EffectiveDate, string? Description, string? Notes) : DomainEvent;

public record InterconnectionAgreementUpdated(DefaultIdType Id, string AgreementNumber, string? Description, string? Notes) : DomainEvent;

public record InterconnectionAgreementActivated(DefaultIdType Id, string AgreementNumber) : DomainEvent;

public record InterconnectionAgreementSuspended(DefaultIdType Id, string AgreementNumber, string Reason) : DomainEvent;

public record InterconnectionAgreementTerminated(DefaultIdType Id, string AgreementNumber, string Reason, DateTime TerminationDate) : DomainEvent;

public record GenerationRecorded(DefaultIdType Id, string AgreementNumber, decimal GenerationKWh, decimal YearToDateGeneration, decimal LifetimeGeneration, DateTime RecordDate) : DomainEvent;

public record CreditApplied(DefaultIdType Id, string AgreementNumber, decimal CreditAmount, decimal CurrentCreditBalance) : DomainEvent;

public record CreditUsed(DefaultIdType Id, string AgreementNumber, decimal CreditAmount, decimal RemainingCreditBalance) : DomainEvent;

public record InspectionRecorded(DefaultIdType Id, string AgreementNumber, DateTime InspectionDate, DateTime? NextInspectionDate) : DomainEvent;

public record InterconnectionAgreementDeleted(DefaultIdType Id) : DomainEvent;

