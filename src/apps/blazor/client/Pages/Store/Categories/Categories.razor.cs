namespace FSH.Starter.Blazor.Client.Pages.Store.Categories;

/// <summary>
/// Store Categories page logic. Provides CRUD and search over Category entities using the generated API client.
/// </summary>
public partial class Categories
{
    
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = null!;
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<CategoryResponse, DefaultIdType, CategoryViewModel> Context { get; set; } = null!;
    private EntityTable<CategoryResponse, DefaultIdType, CategoryViewModel> _table = null!;

    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<CategoryResponse, DefaultIdType, CategoryViewModel>(
            entityName: "Category",
            entityNamePlural: "Categories",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<CategoryResponse>(response => response.ImageUrl, "Image", "ImageUrl", Template: TemplateImage),
                new EntityField<CategoryResponse>(response => response.ParentCategoryId, "Parent", "ParentCategoryId"),
                new EntityField<CategoryResponse>(response => response.Code, "Code", "Code"),
                new EntityField<CategoryResponse>(response => response.Name, "Name", "Name"),
                new EntityField<CategoryResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<CategoryResponse>(response => response.SortOrder, "Sort", "SortOrder"),
                new EntityField<CategoryResponse>(response => response.Description, "Description", "Description"),
            ],
            enableAdvancedSearch: false,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchCategoriesCommand>();
                var result = await Client.SearchCategoriesEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CategoryResponse>>();
            },
            createFunc: async viewModel =>
            {
                viewModel.Image = new FileUploadCommand
                {
                    Name = viewModel.Image?.Name,
                    Extension = viewModel.Image?.Extension,
                    Data = viewModel.Image?.Data,
                    Size = viewModel.Image?.Size,
                };
                await Client.CreateCategoryEndpointAsync("1", viewModel.Adapt<CreateCategoryCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                viewModel.Image = new FileUploadCommand
                {
                    Name = viewModel.Image?.Name,
                    Extension = viewModel.Image?.Extension,
                    Data = viewModel.Image?.Data,
                    Size = viewModel.Image?.Size,
                };
                await Client.UpdateCategoryEndpointAsync("1", id, viewModel.Adapt<UpdateCategoryCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteCategoryEndpointAsync("1", id).ConfigureAwait(false));
        
        await Task.CompletedTask;
    }

    /// <summary>
    /// Show categories help dialog.
    /// </summary>
    private async Task ShowCategoriesHelp()
    {
        await DialogService.ShowAsync<CategoriesHelpDialog>("Categories Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel used by the Categories page for add/edit operations.
/// Inherits from UpdateCategoryCommand to ensure proper mapping with the API.
/// Includes optional Image payload for upload and ImageUrl filename storage.
/// </summary>
public partial class CategoryViewModel : UpdateCategoryCommand;
