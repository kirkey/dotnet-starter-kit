namespace Store.Domain.Exceptions.Warehouse;

/// <summary>
/// Exception thrown when an operation would cause a warehouse's capacity to be exceeded.
/// </summary>
/// <param name="id">The ID of the warehouse.</param>
/// <param name="capacity">The total capacity of the warehouse.</param>
/// <param name="requestedUsage">The requested usage that would exceed capacity.</param>
public sealed class WarehouseCapacityExceededException(DefaultIdType id, decimal capacity, decimal requestedUsage)
    : CustomException($"Warehouse with ID '{id}' capacity of {capacity} would be exceeded. Requested usage: {requestedUsage}.") {}
