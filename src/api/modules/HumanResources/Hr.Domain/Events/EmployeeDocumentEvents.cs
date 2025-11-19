using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when an employee document is created.
/// </summary>
public record EmployeeDocumentCreated : DomainEvent
{
    public EmployeeDocument Document { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee document is updated.
/// </summary>
public record EmployeeDocumentUpdated : DomainEvent
{
    public EmployeeDocument Document { get; init; } = default!;
}

/// <summary>
/// Event raised when an employee document is deactivated.
/// </summary>
public record EmployeeDocumentDeactivated : DomainEvent
{
    public DefaultIdType DocumentId { get; init; }
}

/// <summary>
/// Event raised when an employee document is activated.
/// </summary>
public record EmployeeDocumentActivated : DomainEvent
{
    public DefaultIdType DocumentId { get; init; }
}

