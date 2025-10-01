namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1;

public sealed record GetGroceryItemQuery(DefaultIdType Id) : IRequest<GroceryItemResponse>;
