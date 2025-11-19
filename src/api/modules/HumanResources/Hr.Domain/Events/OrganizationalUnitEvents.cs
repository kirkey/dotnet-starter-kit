using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when a new organizational unit is created.
/// </summary>
public record OrganizationalUnitCreated : DomainEvent
{
    public OrganizationalUnit OrganizationalUnit { get; init; } = default!;
}

/// <summary>
/// Event raised when an organizational unit is updated.
/// </summary>
public record OrganizationalUnitUpdated : DomainEvent
{
    public OrganizationalUnit OrganizationalUnit { get; init; } = default!;
}

/// <summary>
/// Event raised when an organizational unit is moved to a new parent.
/// </summary>
public record OrganizationalUnitMoved : DomainEvent
{
    public DefaultIdType OrganizationalUnitId { get; init; }
    public DefaultIdType? NewParentId { get; init; }
    public int NewLevel { get; init; }
}

/// <summary>
/// Event raised when an organizational unit is activated.
/// </summary>
public record OrganizationalUnitActivated : DomainEvent
{
    public DefaultIdType OrganizationalUnitId { get; init; }
}

/// <summary>
/// Event raised when an organizational unit is deactivated.
/// </summary>
public record OrganizationalUnitDeactivated : DomainEvent
{
    public DefaultIdType OrganizationalUnitId { get; init; }
}

