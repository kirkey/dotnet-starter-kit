using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

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
