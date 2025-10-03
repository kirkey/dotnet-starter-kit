namespace FSH.Starter.WebApi.Store.Application.Bins.Delete.v1;

public sealed record DeleteBinCommand(DefaultIdType Id) : IRequest<DefaultIdType>;
