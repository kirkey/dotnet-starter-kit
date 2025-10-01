namespace FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

public class GetCategoryRequest(DefaultIdType id) : IRequest<CategoryResponse>
{
    public DefaultIdType Id { get; set; } = id;
}

