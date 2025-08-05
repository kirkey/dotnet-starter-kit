using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.FuelConsumption;

public record FuelConsumptionRecorded(DefaultIdType ConsumptionId, DefaultIdType PowerPlantId, string FuelType, decimal Quantity, decimal TotalCost, DateTime ConsumptionDate, string? Description, string? Notes) : DomainEvent;

public record FuelConsumptionUpdated(Accounting.Domain.FuelConsumption FuelConsumption) : DomainEvent;
