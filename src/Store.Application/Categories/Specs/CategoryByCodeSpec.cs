namespace FSH.Starter.WebApi.Store.Application.Categories.Specs;

public sealed class CategoryByCodeSpec : Specification<Category>, ISingleResultSpecification<Category>
{
    public CategoryByCodeSpec(string code, DefaultIdType? excludeId = null)
    {
        var normalized = code.Trim();
        Query
            .Where(c => c.Code == normalized)
            .Where(c => c.Id != excludeId, excludeId.HasValue);
    }
}
