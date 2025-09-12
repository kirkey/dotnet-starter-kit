namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1;

public class GetGroceryItemRequest(DefaultIdType id) : IRequest<GroceryItemResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
