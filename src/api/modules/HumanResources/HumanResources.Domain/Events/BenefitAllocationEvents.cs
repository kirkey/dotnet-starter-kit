namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Event raised when a benefit allocation is created.
/// </summary>
public record BenefitAllocationCreated : DomainEvent
{
    public BenefitAllocation Allocation { get; init; } = default!;
}

/// <summary>
/// Event raised when a benefit allocation is approved.
/// </summary>
public record BenefitAllocationApproved : DomainEvent
{
    public BenefitAllocation Allocation { get; init; } = default!;
}

/// <summary>
/// Event raised when a benefit allocation is rejected.
/// </summary>
public record BenefitAllocationRejected : DomainEvent
{
    public BenefitAllocation Allocation { get; init; } = default!;
}

/// <summary>
/// Event raised when a benefit allocation is paid.
/// </summary>
public record BenefitAllocationPaid : DomainEvent
{
    public BenefitAllocation Allocation { get; init; } = default!;
}

/// <summary>
/// Event raised when a benefit allocation is cancelled.
/// </summary>
public record BenefitAllocationCancelled : DomainEvent
{
    public BenefitAllocation Allocation { get; init; } = default!;
}

