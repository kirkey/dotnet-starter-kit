

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1;

public class GetGroceryItemSpecs : Specification<GroceryItem, GroceryItemResponse>
{
    public GetGroceryItemSpecs(DefaultIdType id)
    {
        Query
            .Where(g => g.Id == id);
    }
}
