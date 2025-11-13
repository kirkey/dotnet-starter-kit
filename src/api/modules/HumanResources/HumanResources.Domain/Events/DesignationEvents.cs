using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when a designation is created.
/// </summary>
public record DesignationCreated : DomainEvent
{
    public Designation Designation { get; init; } = default!;
}

/// <summary>
/// Event raised when a designation is updated.
/// </summary>
public record DesignationUpdated : DomainEvent
{
    public Designation Designation { get; init; } = default!;
}

/// <summary>
/// Event raised when a designation is activated.
/// </summary>
public record DesignationActivated : DomainEvent
{
    public DefaultIdType DesignationId { get; init; }
}

/// <summary>
/// Event raised when a designation is deactivated.
/// </summary>
public record DesignationDeactivated : DomainEvent
{
    public DefaultIdType DesignationId { get; init; }
}

