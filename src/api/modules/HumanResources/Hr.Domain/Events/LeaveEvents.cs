using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when leave type is created.
/// </summary>
public record LeaveTypeCreated : DomainEvent
{
    public LeaveType LeaveType { get; init; } = default!;
}

/// <summary>
/// Event raised when leave type is updated.
/// </summary>
public record LeaveTypeUpdated : DomainEvent
{
    public LeaveType LeaveType { get; init; } = default!;
}

/// <summary>
/// Event raised when leave request is created.
/// </summary>
public record LeaveRequestCreated : DomainEvent
{
    public LeaveRequest LeaveRequest { get; init; } = default!;
}

/// <summary>
/// Event raised when leave request is submitted.
/// </summary>
public record LeaveRequestSubmitted : DomainEvent
{
    public LeaveRequest LeaveRequest { get; init; } = default!;
}

/// <summary>
/// Event raised when leave request is approved.
/// </summary>
public record LeaveRequestApproved : DomainEvent
{
    public LeaveRequest LeaveRequest { get; init; } = default!;
}

/// <summary>
/// Event raised when leave request is rejected.
/// </summary>
public record LeaveRequestRejected : DomainEvent
{
    public LeaveRequest LeaveRequest { get; init; } = default!;
}

/// <summary>
/// Event raised when leave request is cancelled.
/// </summary>
public record LeaveRequestCancelled : DomainEvent
{
    public LeaveRequest LeaveRequest { get; init; } = default!;
}

