using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Categories.Search.v1;

public class SearchCategoriesCommand : PaginationFilter, IRequest<PagedList<CategoryResponse>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public bool? IsActive { get; set; }
}

