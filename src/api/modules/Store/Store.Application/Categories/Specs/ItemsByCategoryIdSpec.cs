namespace FSH.Starter.WebApi.Store.Application.Categories.Specs;

/// <summary>
/// Specification to find items by category ID.
/// </summary>
public sealed class ItemsByCategoryIdSpec : Specification<Item>
{
    public ItemsByCategoryIdSpec(DefaultIdType categoryId)
    {
        Query.Where(item => item.CategoryId == categoryId);
    }
}
