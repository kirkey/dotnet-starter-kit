using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;


/// <summary>
/// Event raised when a deduction is created.
/// </summary>
public record DeductionCreated : DomainEvent
{
    public Deduction Deduction { get; init; } = default!;
}

/// <summary>
/// Event raised when a deduction is updated.
/// </summary>
public record DeductionUpdated : DomainEvent
{
    public Deduction Deduction { get; init; } = default!;
}

/// <summary>
/// Event raised when a deduction is deleted.
/// </summary>
public record DeductionDeleted : DomainEvent
{
    public DefaultIdType DeductionId { get; init; }
    public string DeductionName { get; init; } = default!;
}

/// <summary>
/// Event raised when a deduction is activated.
/// </summary>
public record DeductionActivated : DomainEvent
{
    public Deduction Deduction { get; init; } = default!;
}

/// <summary>
/// Event raised when a deduction is deactivated.
/// </summary>
public record DeductionDeactivated : DomainEvent
{
    public Deduction Deduction { get; init; } = default!;
}
