namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

public sealed record GetCycleCountRequest(DefaultIdType Id) : IRequest<CycleCountResponse>;
