namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;

/// <summary>
/// Command to update an existing cycle count.
/// Only allows updates to cycle counts that are in 'Scheduled' status.
/// </summary>
public sealed record UpdateCycleCountCommand(
    DefaultIdType Id,
    DefaultIdType WarehouseId,
    DefaultIdType? WarehouseLocationId,
    DateTime ScheduledDate,
    string CountType,
    string? Description = null,
    string? CounterName = null,
    string? SupervisorName = null,
    string? Notes = null) : IRequest<UpdateCycleCountResponse>;

