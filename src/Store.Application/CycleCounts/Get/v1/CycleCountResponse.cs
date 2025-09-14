namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

public sealed record CycleCountResponse(
    DefaultIdType? Id,
    string CountNumber,
    DefaultIdType WarehouseId,
    DefaultIdType? WarehouseLocationId,
    DateTime ScheduledDate,
    DateTime? ActualStartDate,
    DateTime? CompletionDate,
    string Status,
    string CountType,
    string? CounterName,
    string? SupervisorName,
    int TotalItemsToCount,
    int ItemsCountedCorrect,
    int ItemsWithDiscrepancies,
    decimal AccuracyPercentage,
    IReadOnlyList<CycleCountItemResponse> Items);

