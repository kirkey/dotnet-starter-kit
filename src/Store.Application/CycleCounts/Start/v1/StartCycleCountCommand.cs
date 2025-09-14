namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Start.v1;

public sealed record StartCycleCountCommand(DefaultIdType Id) : IRequest<StartCycleCountResponse>;

