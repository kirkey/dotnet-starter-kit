namespace Store.Domain.Exceptions.Warehouse;

public sealed class WarehouseCapacityExceededException(DefaultIdType id, decimal capacity, decimal requestedUsage)
    : CustomException($"Warehouse with ID '{id}' capacity of {capacity} would be exceeded. Requested usage: {requestedUsage}.") {}
