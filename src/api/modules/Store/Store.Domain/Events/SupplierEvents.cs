using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Raised when a new Supplier aggregate is created.
/// </summary>
public record SupplierCreated : DomainEvent
{
    /// <summary>
    /// The supplier that was created.
    /// </summary>
    public Supplier Supplier { get; init; } = null!;
}

/// <summary>
/// Raised when an existing Supplier aggregate is updated.
/// </summary>
public record SupplierUpdated : DomainEvent
{
    /// <summary>
    /// The supplier that was updated.
    /// </summary>
    public Supplier Supplier { get; init; } = null!;
}

/// <summary>
/// Raised when a Supplier is activated.
/// </summary>
public record SupplierActivated : DomainEvent
{
    /// <summary>
    /// The supplier that was activated.
    /// </summary>
    public Supplier Supplier { get; init; } = null!;
}

/// <summary>
/// Raised when a Supplier is deactivated.
/// </summary>
public record SupplierDeactivated : DomainEvent
{
    /// <summary>
    /// The supplier that was deactivated.
    /// </summary>
    public Supplier Supplier { get; init; } = null!;
}

/// <summary>
/// Event raised when a supplier's rating is updated.
/// </summary>
public record SupplierRatingUpdated : DomainEvent
{
    public Supplier Supplier { get; init; } = null!;
    public decimal OldRating { get; init; }
    public decimal NewRating { get; init; }
}

/// <summary>
/// Event raised when a supplier's credit limit is changed.
/// </summary>
public record SupplierCreditLimitChanged : DomainEvent
{
    public Supplier Supplier { get; init; } = null!;
    public decimal? OldCreditLimit { get; init; }
    public decimal? NewCreditLimit { get; init; }
}

