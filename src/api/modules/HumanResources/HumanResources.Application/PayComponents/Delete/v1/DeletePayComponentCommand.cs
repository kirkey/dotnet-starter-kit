namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Delete.v1;

public sealed record DeletePayComponentCommand(DefaultIdType Id) : IRequest<DeletePayComponentResponse>;

