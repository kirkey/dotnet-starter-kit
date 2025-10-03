namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

/// <summary>
/// Response for cycle count operations.
/// </summary>
public sealed record CycleCountResponse
{
    public DefaultIdType Id { get; init; }
    public string CountNumber { get; init; } = default!;
    public DefaultIdType WarehouseId { get; init; }
    public string? WarehouseName { get; init; }
    public DefaultIdType? WarehouseLocationId { get; init; }
    public string? WarehouseLocationName { get; init; }
    public DateTime CountDate { get; init; }
    public string Status { get; init; } = "Created";
    public string CountType { get; init; } = default!;
    public string? CountedBy { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? CompletedDate { get; init; }
    public int TotalItems { get; init; }
    public int CountedItems { get; init; }
    public int VarianceItems { get; init; }
    public string? Notes { get; init; }
}
