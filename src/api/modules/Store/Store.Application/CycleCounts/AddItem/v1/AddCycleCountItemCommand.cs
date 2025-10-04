namespace FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.v1;

public sealed record AddCycleCountItemCommand(
    DefaultIdType CycleCountId,
    DefaultIdType ItemId,
    int SystemQuantity,
    int? CountedQuantity = null) : IRequest<AddCycleCountItemResponse>;

