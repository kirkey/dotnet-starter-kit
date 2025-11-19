using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when an employee designation assignment is created.
/// </summary>
public record DesignationAssignmentCreated : DomainEvent
{
    public DesignationAssignment Assignment { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee designation assignment is updated.
/// </summary>
public record DesignationAssignmentUpdated : DomainEvent
{
    public DesignationAssignment Assignment { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee designation assignment ends.
/// </summary>
public record DesignationAssignmentEnded : DomainEvent
{
    public DefaultIdType AssignmentId { get; init; }
}

/// <summary>
/// Event raised when an employee designation assignment is deactivated.
/// </summary>
public record DesignationAssignmentDeactivated : DomainEvent
{
    public DefaultIdType AssignmentId { get; init; }
}

