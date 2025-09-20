using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Store Categories page logic. Provides CRUD and search over Category entities using the generated API client.
/// </summary>
public partial class Categories
{
    [Inject] protected IApiClient ApiClient { get; set; } = default!;

    private EntityServerTableContext<CategoryResponse, DefaultIdType, CategoryViewModel> Context { get; set; } = default!;

    private EntityTable<CategoryResponse, DefaultIdType, CategoryViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<CategoryResponse, DefaultIdType, CategoryViewModel>(
            entityName: "Category",
            entityNamePlural: "Categories",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<CategoryResponse>(response => response.ParentCategoryId, "Parent", "ParentCategoryId"),
                new EntityField<CategoryResponse>(response => response.Code, "Code", "Code"),
                new EntityField<CategoryResponse>(response => response.Name, "Name", "Name"),
                new EntityField<CategoryResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<CategoryResponse>(response => response.SortOrder, "Sort", "SortOrder"),
                new EntityField<CategoryResponse>(response => response.Description, "Description", "Description"),
            ],
            enableAdvancedSearch: false,
            idFunc: response => response.Id!.Value,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchCategoriesCommand>();

                var apiResult = await ApiClient.SearchCategoriesEndpointAsync("1", command).ConfigureAwait(false);

                return new PaginationResponse<CategoryResponse>
                {
                    Items = apiResult.Items?.ToList() ?? [],
                    TotalCount = apiResult.TotalCount,
                    CurrentPage = apiResult.PageNumber,
                    PageSize = apiResult.PageSize
                };
            },
            createFunc: async viewModel =>
            {
                await ApiClient.CreateCategoryEndpointAsync("1", viewModel.Adapt<CreateCategoryCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await ApiClient.UpdateCategoryEndpointAsync("1", id, viewModel.Adapt<UpdateCategoryCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await ApiClient.DeleteCategoryEndpointAsync("1", id).ConfigureAwait(false));
}

/// <summary>
/// ViewModel used by the Categories page for add/edit operations; maps to UpdateCategoryCommand for update and to CreateCategoryCommand for create.
/// </summary>
public partial class CategoryViewModel : UpdateCategoryCommand;

