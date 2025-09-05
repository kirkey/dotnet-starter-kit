namespace FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

public record Capacity(
    decimal MaxWeight = 0,
    decimal MaxVolume = 0,
    string WeightUnit = "kg",
    string VolumeUnit = "m³")
{
    public bool IsWithinCapacity(decimal weight, decimal volume) =>
        (MaxWeight == 0 || weight <= MaxWeight) &&
        (MaxVolume == 0 || volume <= MaxVolume);
}
