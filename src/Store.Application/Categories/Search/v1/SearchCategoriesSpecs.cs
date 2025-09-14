using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Categories.Search.v1;

public class SearchCategoriesSpecs : EntitiesByPaginationFilterSpec<Category, CategoryResponse>
{
    public SearchCategoriesSpecs(SearchCategoriesCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(c => c.Name!.Contains(command.Name!), !string.IsNullOrEmpty(command.Name))
            .Where(c => c.Code == command.Code, !string.IsNullOrEmpty(command.Code))
            .Where(c => c.IsActive == command.IsActive, command.IsActive.HasValue);
}
