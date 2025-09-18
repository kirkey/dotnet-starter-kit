namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

public sealed class GroceryItemsByCategoryIdSpec : Specification<GroceryItem>
{
    public GroceryItemsByCategoryIdSpec(DefaultIdType categoryId)
    {
        Query.Where(g => g.CategoryId == categoryId);
    }
}

