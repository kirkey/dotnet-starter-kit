namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Search.v1;

/// <summary>
/// Response DTO for lot number search results.
/// </summary>
/// <remarks>
/// Contains lot number information for search and listing operations.
/// Used in search results, dropdowns, and reference lookups.
/// 
/// Default values:
/// - LotNumber: required unique identifier
/// - ItemId: required item reference
/// - ExpirationDate: optional expiration date
/// - QuantityOnHand: current available quantity
/// - Status: "Active" (Active, Expired, Quarantine, Consumed)
/// </remarks>
public sealed record LotNumberResponse
{
    /// <summary>
    /// Unique lot number identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Lot number value.
    /// Example: "LOT-2025-001", "EXP-030125".
    /// </summary>
    public string LotNumber { get; init; } = default!;

    /// <summary>
    /// Lot number value (alias for compatibility).
    /// </summary>
    public string LotNumberValue => LotNumber;

    /// <summary>
    /// Associated item identifier.
    /// </summary>
    public DefaultIdType ItemId { get; init; }

    /// <summary>
    /// Item name for display purposes.
    /// </summary>
    public string ItemName { get; init; } = default!;

    /// <summary>
    /// Item SKU for reference.
    /// </summary>
    public string ItemSku { get; init; } = default!;

    /// <summary>
    /// Lot expiration date (for perishable items).
    /// </summary>
    public DateOnly? ExpirationDate { get; init; }

    /// <summary>
    /// Current quantity on hand for this lot.
    /// </summary>
    public decimal QuantityOnHand { get; init; }

    /// <summary>
    /// Lot status: Active, Expired, Quarantine, Consumed.
    /// </summary>
    public string Status { get; init; } = "Active";

    /// <summary>
    /// Supplier information.
    /// </summary>
    public string? SupplierName { get; init; }

    /// <summary>
    /// Manufacturing date.
    /// </summary>
    public DateOnly? ManufacturedDate { get; init; }

    /// <summary>
    /// Date lot was received.
    /// </summary>
    public DateTime ReceivedDate { get; init; }
}
