namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Store Categories page logic. Provides CRUD and search over Category entities using the generated API client.
/// </summary>
public partial class Categories
{
    [Inject] protected IClient ApiClient { get; set; } = default!;
    // Toast and Navigation are already available in this partial via other declarations; avoid duplicate members.

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
            idFunc: response => response.Id,
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

    // Image helpers used by the edit modal

    private string? GetImagePreviewUrl(CategoryViewModel vm)
    {
        // 1) Newly uploaded (but not yet saved) preview
        if (!string.IsNullOrWhiteSpace(vm.Image?.Data))
        {
            return vm.Image!.Data;
        }

        // 2) Existing server file name -> construct an absolute URL to API static files
        if (!string.IsNullOrWhiteSpace(vm.ImageUrl))
        {
            // Server stores images under files/images; ImageUrl is just the filename
            var baseUri = Navigation.BaseUri.TrimEnd('/');
            return $"{baseUri}/files/images/{vm.ImageUrl}";
        }

        return null;
    }

    private async Task OnImageSelected(InputFileChangeEventArgs e, CategoryViewModel vm)
    {
        var file = e.File;
        if (file is null)
        {
            return;
        }

        var extension = Path.GetExtension(file.Name);
        if (string.IsNullOrWhiteSpace(extension) || !AppConstants.SupportedImageFormats.Contains(extension.ToLowerInvariant()))
        {
            Toast.Add("Image format not supported.", Severity.Error);
            return;
        }

        try
        {
            // Create a sanitized file name (we only send name + base64; server generates final file name)
            var fileName = $"category-{DefaultIdType.NewGuid():N}";
            fileName = fileName[..Math.Min(fileName.Length, 90)];

            // Optionally resize to standard format/size, like profile image flow
            var imageFile = await file.RequestImageFileAsync(AppConstants.StandardImageFormat, AppConstants.MaxImageWidth, AppConstants.MaxImageHeight);
            byte[] buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream(AppConstants.MaxAllowedSize).ReadExactlyAsync(buffer);
            var base64String = $"data:{AppConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}";

            vm.Image = new FileUploadCommand
            {
                Name = fileName,
                Extension = extension,
                Data = base64String
            };

            // Clear server image filename to indicate replacement
            vm.ImageUrl = null;
        }
        catch (Exception ex)
        {
            Toast.Add($"Failed to process image: {ex.Message}", Severity.Error);
        }
    }

    private static void RemoveImage(CategoryViewModel vm)
    {
        // Remove pending upload and mark existing server file to be cleared
        vm.Image = null;
        vm.ImageUrl = null;
    }
}

/// <summary>
/// ViewModel used by the Categories page for add/edit operations; maps to UpdateCategoryCommand for update and to CreateCategoryCommand for create.
/// Includes optional Image payload for upload and ImageUrl filename storage.
/// </summary>
public class CategoryViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }
    public DefaultIdType? ParentCategoryId { get; set; }
    public bool? IsActive { get; set; }
    public int? SortOrder { get; set; }
    public string? ImageUrl { get; set; }
    public FileUploadCommand? Image { get; set; }
}
