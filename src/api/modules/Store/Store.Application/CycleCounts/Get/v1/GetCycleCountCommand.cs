namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

/// <summary>
/// Command to get a cycle count by ID.
/// </summary>
public sealed record GetCycleCountCommand(DefaultIdType Id) : IRequest<CycleCountResponse>;
