namespace Store.Domain.Exceptions.WarehouseLocation;

/// <summary>
/// Exception thrown when attempting to delete a warehouse location that still contains inventory or used capacity.
/// </summary>
/// <param name="locationId">The ID of the warehouse location</param>
/// <param name="usedCapacity">Used capacity at the location</param>
public sealed class WarehouseLocationDeletionNotAllowedException(DefaultIdType locationId, decimal usedCapacity)
    : CustomException($"Cannot delete warehouse location '{locationId}' because it has used capacity of {usedCapacity}. Remove all inventory before deleting.") { }

