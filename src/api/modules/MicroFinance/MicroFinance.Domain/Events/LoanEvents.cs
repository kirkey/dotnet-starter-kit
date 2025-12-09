using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>Domain event raised when a new loan product is created.</summary>
public sealed record LoanProductCreated : DomainEvent
{
    public LoanProduct? LoanProduct { get; init; }
}

/// <summary>Domain event raised when a loan product is updated.</summary>
public sealed record LoanProductUpdated : DomainEvent
{
    public LoanProduct? LoanProduct { get; init; }
}

/// <summary>Domain event raised when a new loan is created.</summary>
public sealed record LoanCreated : DomainEvent
{
    public Loan? Loan { get; init; }
}

/// <summary>Domain event raised when a loan is approved.</summary>
public sealed record LoanApproved : DomainEvent
{
    public Loan? Loan { get; init; }
}

/// <summary>Domain event raised when a loan is rejected.</summary>
public sealed record LoanRejected : DomainEvent
{
    public DefaultIdType LoanId { get; init; }
    public string? Reason { get; init; }
}

/// <summary>Domain event raised when a loan is disbursed.</summary>
public sealed record LoanDisbursed : DomainEvent
{
    public Loan? Loan { get; init; }
}

/// <summary>Domain event raised when a loan is fully paid off.</summary>
public sealed record LoanPaidOff : DomainEvent
{
    public DefaultIdType LoanId { get; init; }
}

/// <summary>Domain event raised when a loan is defaulted.</summary>
public sealed record LoanDefaulted : DomainEvent
{
    public DefaultIdType LoanId { get; init; }
    public string? Reason { get; init; }
}

/// <summary>Domain event raised when a loan repayment is created.</summary>
public sealed record LoanRepaymentCreated : DomainEvent
{
    public LoanRepayment? LoanRepayment { get; init; }
}

/// <summary>Domain event raised when a loan repayment is reversed.</summary>
public sealed record LoanRepaymentReversed : DomainEvent
{
    public DefaultIdType LoanRepaymentId { get; init; }
    public string? Reason { get; init; }
}

// ============================================
// Interest Rate Change Events
// ============================================

/// <summary>Domain event raised when an interest rate change is created.</summary>
public sealed record InterestRateChangeCreated(InterestRateChange RateChange) : DomainEvent;

/// <summary>Domain event raised when an interest rate change is updated.</summary>
public sealed record InterestRateChangeUpdated(InterestRateChange RateChange) : DomainEvent;

/// <summary>Domain event raised when an interest rate change is approved.</summary>
public sealed record InterestRateChangeApproved(Guid RateChangeId, Guid ApprovedByUserId, decimal NewRate) : DomainEvent;

/// <summary>Domain event raised when an interest rate change is rejected.</summary>
public sealed record InterestRateChangeRejected(Guid RateChangeId, Guid RejectedByUserId, string Reason) : DomainEvent;

/// <summary>Domain event raised when an interest rate change is applied to a loan.</summary>
public sealed record InterestRateChangeApplied(Guid RateChangeId, Guid LoanId, decimal NewRate) : DomainEvent;

/// <summary>Domain event raised when an interest rate change is cancelled.</summary>
public sealed record InterestRateChangeCancelled(Guid RateChangeId) : DomainEvent;

