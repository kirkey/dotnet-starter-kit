using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Collateral Enhancement entities.
/// </summary>
/// 
// CollateralType Events
public sealed record CollateralTypeCreated(CollateralType CollateralType) : DomainEvent;
public sealed record CollateralTypeUpdated(CollateralType CollateralType) : DomainEvent;
public sealed record CollateralTypeActivated(Guid CollateralTypeId, string Code) : DomainEvent;
public sealed record CollateralTypeDeactivated(Guid CollateralTypeId, string Code) : DomainEvent;

// CollateralValuation Events
public sealed record CollateralValuationCreated(CollateralValuation Valuation) : DomainEvent;
public sealed record CollateralValuationUpdated(CollateralValuation Valuation) : DomainEvent;
public sealed record CollateralValuationSubmitted(Guid ValuationId, Guid CollateralId) : DomainEvent;
public sealed record CollateralValuationApproved(Guid ValuationId, Guid CollateralId, decimal MarketValue) : DomainEvent;
public sealed record CollateralValuationRejected(Guid ValuationId, Guid CollateralId, string Reason) : DomainEvent;
public sealed record CollateralValuationExpired(Guid ValuationId, Guid CollateralId) : DomainEvent;

// CollateralInsurance Events
public sealed record CollateralInsuranceCreated(CollateralInsurance Insurance) : DomainEvent;
public sealed record CollateralInsuranceUpdated(CollateralInsurance Insurance) : DomainEvent;
public sealed record CollateralInsurancePremiumPaid(Guid InsuranceId, Guid CollateralId, decimal Amount) : DomainEvent;
public sealed record CollateralInsuranceRenewed(Guid InsuranceId, Guid CollateralId, DateOnly NewExpiryDate) : DomainEvent;
public sealed record CollateralInsuranceRenewalDue(Guid InsuranceId, Guid CollateralId, DateOnly ExpiryDate) : DomainEvent;
public sealed record CollateralInsuranceExpired(Guid InsuranceId, Guid CollateralId) : DomainEvent;
public sealed record CollateralInsuranceClaimFiled(Guid InsuranceId, Guid CollateralId) : DomainEvent;

// CollateralRelease Events
public sealed record CollateralReleaseRequested(CollateralRelease Release) : DomainEvent;
public sealed record CollateralReleaseUpdated(CollateralRelease Release) : DomainEvent;
public sealed record CollateralReleaseApproved(Guid ReleaseId, Guid CollateralId, Guid LoanId) : DomainEvent;
public sealed record CollateralReleaseRejected(Guid ReleaseId, Guid CollateralId, string Reason) : DomainEvent;
public sealed record CollateralReleased(Guid ReleaseId, Guid CollateralId, Guid LoanId) : DomainEvent;
