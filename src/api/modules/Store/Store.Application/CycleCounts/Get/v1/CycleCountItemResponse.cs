namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

public sealed record CycleCountItemResponse(
    DefaultIdType? Id,
    DefaultIdType ItemId,
    int SystemQuantity,
    int? CountedQuantity,
    int? VarianceQuantity,
    DateTime? CountDate,
    string? CountedBy,
    bool RequiresRecount);

