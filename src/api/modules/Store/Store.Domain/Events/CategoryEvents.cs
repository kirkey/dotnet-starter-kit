using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new category is created in the system.
/// </summary>
public record CategoryCreated : DomainEvent
{
    public Category Category { get; init; } = null!;
}

/// <summary>
/// Event raised when a category is updated.
/// </summary>
public record CategoryUpdated : DomainEvent
{
    public Category Category { get; init; } = null!;
}

/// <summary>
/// Event raised when a category is activated.
/// </summary>
public record CategoryActivated : DomainEvent
{
    public Category Category { get; init; } = null!;
}

/// <summary>
/// Event raised when a category is deactivated.
/// </summary>
public record CategoryDeactivated : DomainEvent
{
    public Category Category { get; init; } = null!;
}

/// <summary>
/// Event raised when a category's parent hierarchy is changed.
/// </summary>
public record CategoryHierarchyChanged : DomainEvent
{
    public Category Category { get; init; } = null!;
    public DefaultIdType? OldParentCategoryId { get; init; }
    public DefaultIdType? NewParentCategoryId { get; init; }
}

