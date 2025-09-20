using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

public sealed class CreateCategoryHandler(
    ILogger<CreateCategoryHandler> logger,
    [FromKeyedServices("store:categories")] IRepository<Category> repository,
    IStorageService storageService)
    : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    /// <summary>
    /// Creates a new category. If the client uploaded an image, saves it to storage and sets ImageUrl to the returned public URI.
    /// </summary>
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        string? imageUrl = request.ImageUrl;
        if (request.Image is not null && !string.IsNullOrWhiteSpace(request.Image.Data))
        {
            var uri = await storageService.UploadAsync<Category>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            if (uri is null)
            {
                throw new InvalidOperationException("Image upload failed: storage provider returned no URI.");
            }

            // Persist the full absolute URI returned by the storage provider so clients can load images directly.
            imageUrl = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }

        var category = Category.Create(
            request.Name!,
            request.Description,
            request.Code!,
            request.ParentCategoryId,
            request.IsActive,
            request.SortOrder,
            imageUrl);

        await repository.AddAsync(category, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Category created {CategoryId}. ImageUrl: {ImageUrl}", category.Id, imageUrl);
        return new CreateCategoryResponse(category.Id);
    }
}
