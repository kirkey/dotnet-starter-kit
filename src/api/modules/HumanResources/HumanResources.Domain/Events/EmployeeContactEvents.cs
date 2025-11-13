using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when an employee contact is created.
/// </summary>
public record EmployeeContactCreated : DomainEvent
{
    public EmployeeContact Contact { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee contact is updated.
/// </summary>
public record EmployeeContactUpdated : DomainEvent
{
    public EmployeeContact Contact { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee contact is deactivated.
/// </summary>
public record EmployeeContactDeactivated : DomainEvent
{
    public DefaultIdType ContactId { get; init; }
}

/// <summary>
/// Event raised when an employee contact is activated.
/// </summary>
public record EmployeeContactActivated : DomainEvent
{
    public DefaultIdType ContactId { get; init; }
}

