using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

// ============================================
// Loan Schedule Events
// ============================================

/// <summary>Domain event raised when a loan schedule entry is created.</summary>
public sealed record LoanScheduleCreated : DomainEvent
{
    public LoanSchedule? LoanSchedule { get; init; }
}

/// <summary>Domain event raised when a loan schedule is paid.</summary>
public sealed record LoanSchedulePaid : DomainEvent
{
    public DefaultIdType ScheduleId { get; init; }
}

/// <summary>Domain event raised when a loan schedule is waived.</summary>
public sealed record LoanScheduleWaived : DomainEvent
{
    public DefaultIdType ScheduleId { get; init; }
}

// ============================================
// Loan Guarantor Events
// ============================================

/// <summary>Domain event raised when a loan guarantor is created.</summary>
public sealed record LoanGuarantorCreated : DomainEvent
{
    public LoanGuarantor? LoanGuarantor { get; init; }
}

/// <summary>Domain event raised when a loan guarantor is approved.</summary>
public sealed record LoanGuarantorApproved : DomainEvent
{
    public DefaultIdType GuarantorId { get; init; }
}

/// <summary>Domain event raised when a loan guarantor is rejected.</summary>
public sealed record LoanGuarantorRejected : DomainEvent
{
    public DefaultIdType GuarantorId { get; init; }
    public string? Reason { get; init; }
}

/// <summary>Domain event raised when a loan guarantor is released.</summary>
public sealed record LoanGuarantorReleased : DomainEvent
{
    public DefaultIdType GuarantorId { get; init; }
}

// ============================================
// Loan Collateral Events
// ============================================

/// <summary>Domain event raised when loan collateral is created.</summary>
public sealed record LoanCollateralCreated : DomainEvent
{
    public LoanCollateral? LoanCollateral { get; init; }
}

/// <summary>Domain event raised when collateral valuation is updated.</summary>
public sealed record LoanCollateralValuationUpdated : DomainEvent
{
    public DefaultIdType CollateralId { get; init; }
}

/// <summary>Domain event raised when collateral is verified.</summary>
public sealed record LoanCollateralVerified : DomainEvent
{
    public DefaultIdType CollateralId { get; init; }
}

/// <summary>Domain event raised when collateral is pledged.</summary>
public sealed record LoanCollateralPledged : DomainEvent
{
    public DefaultIdType CollateralId { get; init; }
}

/// <summary>Domain event raised when collateral is released.</summary>
public sealed record LoanCollateralReleased : DomainEvent
{
    public DefaultIdType CollateralId { get; init; }
}

/// <summary>Domain event raised when collateral is seized.</summary>
public sealed record LoanCollateralSeized : DomainEvent
{
    public DefaultIdType CollateralId { get; init; }
    public string? Reason { get; init; }
}
