namespace FSH.Starter.WebApi.Store.Application.Categories.Delete.v1;

public sealed record DeleteCategoryCommand(DefaultIdType Id) : IRequest;

