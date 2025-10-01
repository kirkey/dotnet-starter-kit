namespace Store.Domain.Events;

public record ShipmentCreated : DomainEvent { public Shipment Shipment { get; init; } = default!; }
public record ShipmentItemAdded : DomainEvent { public Shipment Shipment { get; init; } = default!; public ShipmentItem Item { get; init; } = default!; }
public record ShipmentShipped : DomainEvent { public Shipment Shipment { get; init; } = default!; }

