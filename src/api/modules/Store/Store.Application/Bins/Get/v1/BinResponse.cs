namespace FSH.Starter.WebApi.Store.Application.Bins.Get.v1;

/// <summary>
/// Response for bin operations.
/// </summary>
public sealed record BinResponse
{
    public DefaultIdType Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Code { get; init; }
    public DefaultIdType WarehouseLocationId { get; init; }
    public string? WarehouseLocationName { get; init; }
    public decimal? Capacity { get; init; }
    public decimal? UsedCapacity { get; init; }
    public string? CapacityUnit { get; init; }
    public bool IsActive { get; init; } = true;
    public string? Notes { get; init; }
    public string? Aisle { get; init; }
    public string? Section { get; init; }
    public string? Shelf { get; init; }
    public string? LocationType { get; init; }
}
