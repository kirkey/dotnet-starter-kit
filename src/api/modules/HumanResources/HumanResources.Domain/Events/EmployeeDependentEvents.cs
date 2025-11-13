using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when an employee dependent is created.
/// </summary>
public record EmployeeDependentCreated : DomainEvent
{
    public EmployeeDependent Dependent { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee dependent is updated.
/// </summary>
public record EmployeeDependentUpdated : DomainEvent
{
    public EmployeeDependent Dependent { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee dependent is deactivated.
/// </summary>
public record EmployeeDependentDeactivated : DomainEvent
{
    public DefaultIdType DependentId { get; init; }
}

/// <summary>
/// Event raised when an employee dependent is activated.
/// </summary>
public record EmployeeDependentActivated : DomainEvent
{
    public DefaultIdType DependentId { get; init; }
}

