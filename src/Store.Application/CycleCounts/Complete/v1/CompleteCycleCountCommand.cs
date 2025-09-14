namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Complete.v1;

public sealed record CompleteCycleCountCommand(DefaultIdType Id) : IRequest<CompleteCycleCountResponse>;

