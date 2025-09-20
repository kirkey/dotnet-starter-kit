using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Store Categories page logic. Provides CRUD and search over Category entities using the generated API client.
/// </summary>
public partial class Categories
{
    [Inject] protected IApiClient _client { get; set; } = default!;

    protected EntityServerTableContext<CategoryResponse, DefaultIdType, CategoryViewModel> Context { get; set; } = default!;

    private EntityTable<CategoryResponse, DefaultIdType, CategoryViewModel> _table = default!;

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<CategoryResponse, DefaultIdType, CategoryViewModel>(
            entityName: "Category",
            entityNamePlural: "Categories",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<CategoryResponse>(c => c.Id, "Id", "Id"),
                new EntityField<CategoryResponse>(c => c.Name, "Name", "Name"),
                new EntityField<CategoryResponse>(c => c.Code, "Code", "Code"),
                new EntityField<CategoryResponse>(c => c.IsActive, "Active", "IsActive"),
                new EntityField<CategoryResponse>(c => c.SortOrder, "Sort", "SortOrder"),
                new EntityField<CategoryResponse>(c => c.ParentCategoryId, "Parent", "ParentCategoryId")
            ],
            enableAdvancedSearch: true,
            idFunc: c => c.Id!.Value,
            searchFunc: async filter =>
            {
                var cmd = filter.Adapt<SearchCategoriesCommand>();
                cmd.Name = _searchName;
                cmd.Code = _searchCode;
                cmd.IsActive = _searchIsActive;
                var result = await _client.SearchCategoriesEndpointAsync("1", cmd);
                return result.Adapt<PaginationResponse<CategoryResponse>>();
            },
            createFunc: async vm =>
            {
                await _client.CreateCategoryEndpointAsync("1", vm.Adapt<CreateCategoryCommand>());
            },
            updateFunc: async (id, vm) =>
            {
                await _client.UpdateCategoryEndpointAsync("1", id, vm.Adapt<UpdateCategoryCommand>());
            },
            deleteFunc: async id => await _client.DeleteCategoryEndpointAsync("1", id));
    }

    // Advanced Search State
    private string? _searchName;
    private string? _searchCode;
    private bool? _searchIsActive;

    public string? SearchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    public string? SearchCode
    {
        get => _searchCode;
        set
        {
            _searchCode = value;
            _ = _table.ReloadDataAsync();
        }
    }

    public bool? SearchIsActive
    {
        get => _searchIsActive;
        set
        {
            _searchIsActive = value;
            _ = _table.ReloadDataAsync();
        }
    }
}

/// <summary>
/// ViewModel used by the Categories page for add/edit operations; maps to UpdateCategoryCommand for update and to CreateCategoryCommand for create.
/// </summary>
public partial class CategoryViewModel : UpdateCategoryCommand;
