using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Insurance entities.
/// </summary>

// InsuranceProduct Events
public sealed record InsuranceProductCreated(InsuranceProduct Product) : DomainEvent;
public sealed record InsuranceProductUpdated(InsuranceProduct Product) : DomainEvent;
public sealed record InsuranceProductActivated(Guid ProductId) : DomainEvent;
public sealed record InsuranceProductDeactivated(Guid ProductId) : DomainEvent;
public sealed record InsuranceProductDiscontinued(Guid ProductId) : DomainEvent;

// InsurancePolicy Events
public sealed record InsurancePolicyCreated(InsurancePolicy Policy) : DomainEvent;
public sealed record InsurancePremiumPaid(Guid PolicyId, decimal Amount, decimal TotalPaid) : DomainEvent;
public sealed record InsuranceBeneficiaryUpdated(Guid PolicyId, string BeneficiaryName) : DomainEvent;
public sealed record InsurancePolicyLapsed(Guid PolicyId) : DomainEvent;
public sealed record InsurancePolicyReinstated(Guid PolicyId) : DomainEvent;
public sealed record InsurancePolicyCancelled(Guid PolicyId, string Reason) : DomainEvent;
public sealed record InsurancePolicyExpired(Guid PolicyId) : DomainEvent;
public sealed record InsurancePolicyClaimed(Guid PolicyId) : DomainEvent;
public sealed record InsurancePolicyMatured(Guid PolicyId) : DomainEvent;

// InsuranceClaim Events
public sealed record InsuranceClaimCreated(InsuranceClaim Claim) : DomainEvent;
public sealed record InsuranceClaimFiled(Guid ClaimId) : DomainEvent;
public sealed record InsuranceClaimUnderReview(Guid ClaimId, Guid ReviewerUserId) : DomainEvent;
public sealed record InsuranceClaimDocumentsRequested(Guid ClaimId) : DomainEvent;
public sealed record InsuranceClaimApproved(Guid ClaimId, Guid ApproverUserId, decimal ApprovedAmount) : DomainEvent;
public sealed record InsuranceClaimRejected(Guid ClaimId, Guid ApproverUserId, string Reason) : DomainEvent;
public sealed record InsuranceClaimPaid(Guid ClaimId, decimal Amount, string PaymentReference) : DomainEvent;
public sealed record InsuranceClaimClosed(Guid ClaimId) : DomainEvent;
