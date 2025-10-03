using Store.Domain.Entities;

namespace Store.Domain.Events;

// ============================================================================
// StockLevel Events
// ============================================================================

/// <summary>
/// Event raised when a new stock level record is created.
/// </summary>
public record StockLevelCreated : DomainEvent
{
    public StockLevel StockLevel { get; init; } = default!;
}

/// <summary>
/// Event raised when stock level is updated.
/// </summary>
public record StockLevelUpdated : DomainEvent
{
    public StockLevel StockLevel { get; init; } = default!;
    public int QuantityChange { get; init; }
    public string ChangeType { get; init; } = default!;
}

/// <summary>
/// Event raised when quantity is reserved.
/// </summary>
public record StockLevelReserved : DomainEvent
{
    public StockLevel StockLevel { get; init; } = default!;
    public int ReservedQuantity { get; init; }
}

/// <summary>
/// Event raised when quantity is allocated to a pick list.
/// </summary>
public record StockLevelAllocated : DomainEvent
{
    public StockLevel StockLevel { get; init; } = default!;
    public int AllocatedQuantity { get; init; }
}

/// <summary>
/// Event raised when a physical count is recorded.
/// </summary>
public record StockLevelCounted : DomainEvent
{
    public StockLevel StockLevel { get; init; } = default!;
    public int CountedQuantity { get; init; }
    public int Variance { get; init; }
}

// ============================================================================
// Bin Events
// ============================================================================

/// <summary>
/// Event raised when a new bin is created.
/// </summary>
public record BinCreated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
}

/// <summary>
/// Event raised when a bin is updated.
/// </summary>
public record BinUpdated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
}

/// <summary>
/// Event raised when a bin is activated.
/// </summary>
public record BinActivated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
}

/// <summary>
/// Event raised when a bin is deactivated.
/// </summary>
public record BinDeactivated : DomainEvent
{
    public Bin Bin { get; init; } = default!;
}

// ============================================================================
// LotNumber Events
// ============================================================================

/// <summary>
/// Event raised when a new lot number is created.
/// </summary>
public record LotNumberCreated : DomainEvent
{
    public LotNumber LotNumber { get; init; } = default!;
}

/// <summary>
/// Event raised when a lot number is updated.
/// </summary>
public record LotNumberUpdated : DomainEvent
{
    public LotNumber LotNumber { get; init; } = default!;
}

/// <summary>
/// Event raised when a lot number expires.
/// </summary>
public record LotNumberExpired : DomainEvent
{
    public LotNumber LotNumber { get; init; } = default!;
}

/// <summary>
/// Event raised when a lot number is quarantined.
/// </summary>
public record LotNumberQuarantined : DomainEvent
{
    public LotNumber LotNumber { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

/// <summary>
/// Event raised when a lot number is recalled.
/// </summary>
public record LotNumberRecalled : DomainEvent
{
    public LotNumber LotNumber { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

// ============================================================================
// SerialNumber Events
// ============================================================================

/// <summary>
/// Event raised when a new serial number is created.
/// </summary>
public record SerialNumberCreated : DomainEvent
{
    public SerialNumber SerialNumber { get; init; } = default!;
}

/// <summary>
/// Event raised when a serial number is updated.
/// </summary>
public record SerialNumberUpdated : DomainEvent
{
    public SerialNumber SerialNumber { get; init; } = default!;
}

/// <summary>
/// Event raised when a serial number is allocated.
/// </summary>
public record SerialNumberAllocated : DomainEvent
{
    public SerialNumber SerialNumber { get; init; } = default!;
}

/// <summary>
/// Event raised when a serial number is shipped.
/// </summary>
public record SerialNumberShipped : DomainEvent
{
    public SerialNumber SerialNumber { get; init; } = default!;
}

/// <summary>
/// Event raised when a serial number is sold.
/// </summary>
public record SerialNumberSold : DomainEvent
{
    public SerialNumber SerialNumber { get; init; } = default!;
}

/// <summary>
/// Event raised when a serial number is marked defective.
/// </summary>
public record SerialNumberDefective : DomainEvent
{
    public SerialNumber SerialNumber { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

// ============================================================================
// PickList Events
// ============================================================================

/// <summary>
/// Event raised when a pick list is created.
/// </summary>
public record PickListCreated : DomainEvent
{
    public PickList PickList { get; init; } = default!;
}

/// <summary>
/// Event raised when a pick list is updated.
/// </summary>
public record PickListUpdated : DomainEvent
{
    public PickList PickList { get; init; } = default!;
}

/// <summary>
/// Event raised when an item is added to a pick list.
/// </summary>
public record PickListItemAdded : DomainEvent
{
    public PickList PickList { get; init; } = default!;
    public PickListItem Item { get; init; } = default!;
}

/// <summary>
/// Event raised when a pick list is assigned to a picker.
/// </summary>
public record PickListAssigned : DomainEvent
{
    public PickList PickList { get; init; } = default!;
    public string AssignedTo { get; init; } = default!;
}

/// <summary>
/// Event raised when picking starts.
/// </summary>
public record PickListStarted : DomainEvent
{
    public PickList PickList { get; init; } = default!;
}

/// <summary>
/// Event raised when picking is completed.
/// </summary>
public record PickListCompleted : DomainEvent
{
    public PickList PickList { get; init; } = default!;
}

/// <summary>
/// Event raised when a pick list is cancelled.
/// </summary>
public record PickListCancelled : DomainEvent
{
    public PickList PickList { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

// ============================================================================
// PutAwayTask Events
// ============================================================================

/// <summary>
/// Event raised when a put-away task is created.
/// </summary>
public record PutAwayTaskCreated : DomainEvent
{
    public PutAwayTask PutAwayTask { get; init; } = default!;
}

/// <summary>
/// Event raised when a put-away task is updated.
/// </summary>
public record PutAwayTaskUpdated : DomainEvent
{
    public PutAwayTask PutAwayTask { get; init; } = default!;
}

/// <summary>
/// Event raised when an item is added to a put-away task.
/// </summary>
public record PutAwayTaskItemAdded : DomainEvent
{
    public PutAwayTask PutAwayTask { get; init; } = default!;
    public PutAwayTaskItem Item { get; init; } = default!;
}

/// <summary>
/// Event raised when a put-away task is assigned to a worker.
/// </summary>
public record PutAwayTaskAssigned : DomainEvent
{
    public PutAwayTask PutAwayTask { get; init; } = default!;
    public string AssignedTo { get; init; } = default!;
}

/// <summary>
/// Event raised when put-away starts.
/// </summary>
public record PutAwayTaskStarted : DomainEvent
{
    public PutAwayTask PutAwayTask { get; init; } = default!;
}

/// <summary>
/// Event raised when put-away is completed.
/// </summary>
public record PutAwayTaskCompleted : DomainEvent
{
    public PutAwayTask PutAwayTask { get; init; } = default!;
}

// ============================================================================
// ItemSupplier Events
// ============================================================================

/// <summary>
/// Event raised when an item-supplier relationship is created.
/// </summary>
public record ItemSupplierCreated : DomainEvent
{
    public ItemSupplier ItemSupplier { get; init; } = default!;
}

/// <summary>
/// Event raised when an item-supplier relationship is updated.
/// </summary>
public record ItemSupplierUpdated : DomainEvent
{
    public ItemSupplier ItemSupplier { get; init; } = default!;
}

/// <summary>
/// Event raised when an item-supplier is activated.
/// </summary>
public record ItemSupplierActivated : DomainEvent
{
    public ItemSupplier ItemSupplier { get; init; } = default!;
}

/// <summary>
/// Event raised when an item-supplier is deactivated.
/// </summary>
public record ItemSupplierDeactivated : DomainEvent
{
    public ItemSupplier ItemSupplier { get; init; } = default!;
}

// ============================================================================
// InventoryReservation Events
// ============================================================================

/// <summary>
/// Event raised when inventory is reserved.
/// </summary>
public record InventoryReservationCreated : DomainEvent
{
    public InventoryReservation InventoryReservation { get; init; } = default!;
}

/// <summary>
/// Event raised when a reservation is allocated to a pick list.
/// </summary>
public record InventoryReservationAllocated : DomainEvent
{
    public InventoryReservation InventoryReservation { get; init; } = default!;
}

/// <summary>
/// Event raised when a reservation is released.
/// </summary>
public record InventoryReservationReleased : DomainEvent
{
    public InventoryReservation InventoryReservation { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

/// <summary>
/// Event raised when a reservation is cancelled.
/// </summary>
public record InventoryReservationCancelled : DomainEvent
{
    public InventoryReservation InventoryReservation { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

/// <summary>
/// Event raised when a reservation expires automatically.
/// </summary>
public record InventoryReservationExpired : DomainEvent
{
    public InventoryReservation InventoryReservation { get; init; } = default!;
}
