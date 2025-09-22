namespace Store.Domain.Events;

/// <summary>
/// Raised when a new Supplier aggregate is created.
/// </summary>
public record SupplierCreated : DomainEvent
{
    /// <summary>
    /// The supplier that was created.
    /// </summary>
    public Supplier Supplier { get; init; } = default!;
}

/// <summary>
/// Raised when an existing Supplier aggregate is updated.
/// </summary>
public record SupplierUpdated : DomainEvent
{
    /// <summary>
    /// The supplier that was updated.
    /// </summary>
    public Supplier Supplier { get; init; } = default!;
}

/// <summary>
/// Raised when a Supplier is activated.
/// </summary>
public record SupplierActivated : DomainEvent
{
    /// <summary>
    /// The supplier that was activated.
    /// </summary>
    public Supplier Supplier { get; init; } = default!;
}

/// <summary>
/// Raised when a Supplier is deactivated.
/// </summary>
public record SupplierDeactivated : DomainEvent
{
    /// <summary>
    /// The supplier that was deactivated.
    /// </summary>
    public Supplier Supplier { get; init; } = default!;
}
