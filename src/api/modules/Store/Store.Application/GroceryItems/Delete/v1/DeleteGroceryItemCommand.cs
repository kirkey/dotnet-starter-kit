namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Delete.v1;

public sealed record DeleteGroceryItemCommand(
    DefaultIdType Id) : IRequest;
