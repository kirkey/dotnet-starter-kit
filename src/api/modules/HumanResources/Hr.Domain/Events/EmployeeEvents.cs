using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when employee is created.
/// </summary>
public record EmployeeCreated : DomainEvent
{
    public Employee Employee { get; init; } = default!;
}

/// <summary>
/// Event raised when employee information is updated.
/// </summary>
public record EmployeeUpdated : DomainEvent
{
    public Employee Employee { get; init; } = default!;
}

/// <summary>
/// Event raised when employee is hired.
/// </summary>
public record EmployeeHired : DomainEvent
{
    public DefaultIdType EmployeeId { get; init; }
    public DateTime HireDate { get; init; }
}

/// <summary>
/// Event raised when employee is terminated.
/// </summary>
public record EmployeeTerminated : DomainEvent
{
    public DefaultIdType EmployeeId { get; init; }
    public DateTime TerminationDate { get; init; }
    public string? Reason { get; init; }
}

/// <summary>
/// Event raised when employee contact information is updated.
/// </summary>
public record EmployeeContactInfoUpdated : DomainEvent
{
    public Employee Employee { get; init; } = default!;
}

/// <summary>
/// Event raised when employee is transferred to another organizational unit.
/// </summary>
public record EmployeeTransferred : DomainEvent
{
    public DefaultIdType EmployeeId { get; init; }
    public DefaultIdType FromUnitId { get; init; }
    public DefaultIdType ToUnitId { get; init; }
}

/// <summary>
/// Event raised when employee is marked as on leave.
/// </summary>
public record EmployeeOnLeave : DomainEvent
{
    public DefaultIdType EmployeeId { get; init; }
}

/// <summary>
/// Event raised when employee returns from leave.
/// </summary>
public record EmployeeReturnedFromLeave : DomainEvent
{
    public DefaultIdType EmployeeId { get; init; }
}

/// <summary>
/// Event raised when employee status changes.
/// </summary>
public record EmployeeStatusChanged : DomainEvent
{
    public Employee Employee { get; init; } = default!;
    public string OldStatus { get; init; } = default!;
    public string NewStatus { get; init; } = default!;
}

/// <summary>
/// Event raised when employee is deactivated.
/// </summary>
public record EmployeeDeactivated : DomainEvent
{
    public DefaultIdType EmployeeId { get; init; }
}

/// <summary>
/// Event raised when employee is activated.
/// </summary>
public record EmployeeActivated : DomainEvent
{
    public DefaultIdType EmployeeId { get; init; }
}

