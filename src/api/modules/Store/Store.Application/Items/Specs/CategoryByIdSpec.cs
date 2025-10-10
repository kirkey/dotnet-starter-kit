namespace FSH.Starter.WebApi.Store.Application.Items.Specs;

/// <summary>
/// Specification for finding a Category by its ID.
/// </summary>
public sealed class CategoryByIdSpec : Specification<Category>
{
    public CategoryByIdSpec(DefaultIdType categoryId)
    {
        Query.Where(c => c.Id == categoryId);
    }
}
