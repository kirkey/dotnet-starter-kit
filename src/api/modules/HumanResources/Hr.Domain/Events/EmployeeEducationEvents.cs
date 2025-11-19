using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when an employee education record is created.
/// </summary>
public record EmployeeEducationCreated : DomainEvent
{
    public EmployeeEducation Education { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee education record is updated.
/// </summary>
public record EmployeeEducationUpdated : DomainEvent
{
    public EmployeeEducation Education { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee education record is deactivated.
/// </summary>
public record EmployeeEducationDeactivated : DomainEvent
{
    public DefaultIdType EducationId { get; init; }
}

/// <summary>
/// Event raised when an employee education record is activated.
/// </summary>
public record EmployeeEducationActivated : DomainEvent
{
    public DefaultIdType EducationId { get; init; }
}

