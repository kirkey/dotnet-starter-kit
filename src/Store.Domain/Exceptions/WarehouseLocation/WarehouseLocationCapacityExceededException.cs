namespace Store.Domain.Exceptions.WarehouseLocation;

public sealed class WarehouseLocationCapacityExceededException(DefaultIdType id, decimal capacity, decimal requestedUsage)
    : Exception($"Warehouse Location with ID '{id}' capacity of {capacity} would be exceeded. Requested usage: {requestedUsage}.") {}
