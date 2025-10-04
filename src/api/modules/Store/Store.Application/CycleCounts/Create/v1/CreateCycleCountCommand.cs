namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Create.v1;

public sealed record CreateCycleCountCommand(
    [property: DefaultValue("Cycle Count")] string? Name,
    [property: DefaultValue(null)] string? Description,
    string CountNumber,
    DefaultIdType WarehouseId,
    DefaultIdType? WarehouseLocationId,
    DateTime ScheduledDate,
    string CountType,
    string? CounterName = null,
    string? SupervisorName = null,
    string? Notes = null) : IRequest<CreateCycleCountResponse>;

