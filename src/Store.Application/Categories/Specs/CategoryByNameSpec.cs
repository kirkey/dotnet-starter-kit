namespace FSH.Starter.WebApi.Store.Application.Categories.Specs;

public sealed class CategoryByNameSpec : Specification<Category>, ISingleResultSpecification<Category>
{
    public CategoryByNameSpec(string name, DefaultIdType? excludeId = null)
    {
        var normalized = name.Trim();
        Query
            .Where(c => c.Name == normalized)
            .Where(c => c.Id != excludeId, excludeId.HasValue);
    }
}

