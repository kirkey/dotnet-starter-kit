namespace Store.Domain.Exceptions.Shipment;

public sealed class ShipmentNotFoundException(DefaultIdType id)
    : NotFoundException($"Shipment with ID '{id}' was not found.") {}

