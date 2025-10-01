using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Categories.Specs;

public class GetCategorySpecs : Specification<Category, CategoryResponse>
{
    public GetCategorySpecs(DefaultIdType id)
    {
        Query
            .Where(c => c.Id == id);
    }
}

