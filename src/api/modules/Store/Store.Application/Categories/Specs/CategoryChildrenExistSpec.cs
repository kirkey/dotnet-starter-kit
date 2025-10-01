namespace FSH.Starter.WebApi.Store.Application.Categories.Specs;

public sealed class CategoryChildrenExistSpec : Specification<Category>
{
    public CategoryChildrenExistSpec(DefaultIdType parentId)
    {
        Query.Where(c => c.ParentCategoryId == parentId);
    }
}

