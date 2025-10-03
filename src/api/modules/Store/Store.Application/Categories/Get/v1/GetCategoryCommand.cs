namespace FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

public class GetCategoryCommand(DefaultIdType id) : IRequest<CategoryResponse>
{
    public DefaultIdType Id { get; set; } = id;
}

