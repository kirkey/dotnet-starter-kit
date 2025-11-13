using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when an employee designation assignment is created.
/// </summary>
public record EmployeeDesignationAssignmentCreated : DomainEvent
{
    public EmployeeDesignationAssignment Assignment { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee designation assignment is updated.
/// </summary>
public record EmployeeDesignationAssignmentUpdated : DomainEvent
{
    public EmployeeDesignationAssignment Assignment { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee designation assignment ends.
/// </summary>
public record EmployeeDesignationAssignmentEnded : DomainEvent
{
    public DefaultIdType AssignmentId { get; init; }
}

/// <summary>
/// Event raised when an employee designation assignment is deactivated.
/// </summary>
public record EmployeeDesignationAssignmentDeactivated : DomainEvent
{
    public DefaultIdType AssignmentId { get; init; }
}

