using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Advanced Loan Management entities.
/// </summary>

// LoanRestructure Events
public sealed record LoanRestructureCreated(LoanRestructure Restructure) : DomainEvent;
public sealed record LoanRestructureSubmitted(Guid RestructureId) : DomainEvent;
public sealed record LoanRestructureApproved(Guid RestructureId, Guid ApprovedByUserId, DateOnly EffectiveDate) : DomainEvent;
public sealed record LoanRestructureRejected(Guid RestructureId, Guid RejectedByUserId, string Reason) : DomainEvent;
public sealed record LoanRestructureActivated(Guid RestructureId) : DomainEvent;
public sealed record LoanRestructureCompleted(Guid RestructureId) : DomainEvent;

// LoanWriteOff Events
public sealed record LoanWriteOffCreated(LoanWriteOff WriteOff) : DomainEvent;
public sealed record LoanWriteOffSubmitted(Guid WriteOffId) : DomainEvent;
public sealed record LoanWriteOffApproved(Guid WriteOffId, Guid ApprovedByUserId, decimal TotalWriteOff) : DomainEvent;
public sealed record LoanWriteOffRejected(Guid WriteOffId, Guid RejectedByUserId, string Reason) : DomainEvent;
public sealed record LoanWriteOffProcessed(Guid WriteOffId, decimal Amount) : DomainEvent;
public sealed record LoanWriteOffRecovery(Guid WriteOffId, decimal RecoveredAmount, decimal TotalRecovered) : DomainEvent;

// LoanDisbursementTranche Events
public sealed record LoanDisbursementTrancheCreated(LoanDisbursementTranche Tranche) : DomainEvent;
public sealed record LoanDisbursementTrancheMilestoneVerified(Guid TrancheId) : DomainEvent;
public sealed record LoanDisbursementTrancheSubmitted(Guid TrancheId) : DomainEvent;
public sealed record LoanDisbursementTrancheApproved(Guid TrancheId, Guid ApprovedByUserId) : DomainEvent;
public sealed record LoanDisbursementTrancheDisbursed(Guid TrancheId, decimal Amount, DateOnly DisbursedDate) : DomainEvent;
public sealed record LoanDisbursementTrancheOnHold(Guid TrancheId, string Reason) : DomainEvent;
public sealed record LoanDisbursementTrancheCancelled(Guid TrancheId, string Reason) : DomainEvent;

// LoanApplication Events
public sealed record LoanApplicationCreated(LoanApplication Application) : DomainEvent;
public sealed record LoanApplicationSubmitted(Guid ApplicationId) : DomainEvent;
public sealed record LoanApplicationAssigned(Guid ApplicationId, Guid OfficerId) : DomainEvent;
public sealed record LoanApplicationCreditAssessed(Guid ApplicationId, int CreditScore, string RiskGrade) : DomainEvent;
public sealed record LoanApplicationDocumentsRequested(Guid ApplicationId) : DomainEvent;
public sealed record LoanApplicationPendingApproval(Guid ApplicationId) : DomainEvent;
public sealed record LoanApplicationApproved(Guid ApplicationId, Guid ApprovedByUserId, decimal ApprovedAmount) : DomainEvent;
public sealed record LoanApplicationConditionallyApproved(Guid ApplicationId, Guid ApprovedByUserId, string Conditions) : DomainEvent;
public sealed record LoanApplicationRejected(Guid ApplicationId, Guid RejectedByUserId, string Reason) : DomainEvent;
public sealed record LoanApplicationWithdrawn(Guid ApplicationId) : DomainEvent;
public sealed record LoanApplicationDisbursed(Guid ApplicationId, Guid LoanId) : DomainEvent;
public sealed record LoanApplicationExpired(Guid ApplicationId) : DomainEvent;
public sealed record LoanApplicationReturned(Guid ApplicationId, string Reason) : DomainEvent;
