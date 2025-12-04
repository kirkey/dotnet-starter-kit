using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

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
