namespace FSH.Starter.WebApi.Store.Application.Items.Delete.v1;

public sealed record DeleteItemCommand(DefaultIdType Id) : IRequest<DefaultIdType>;
