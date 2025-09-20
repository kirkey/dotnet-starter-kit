namespace FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

using Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using System.IO;

public sealed class CreateCategoryHandler(
    ILogger<CreateCategoryHandler> logger,
    [FromKeyedServices("store:categories")] IRepository<Category> repository,
    IStorageService storageService)
    : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    /// <summary>
    /// Creates a new category. If the client uploaded an image, saves it to local storage and sets ImageUrl from the saved filename.
    /// </summary>
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        string? imageUrl = request.ImageUrl;
        if (request.Image is not null)
        {
            var uri = await storageService.UploadAsync<Category>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            imageUrl = Path.GetFileName(uri.LocalPath);
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
        logger.LogInformation("category created {CategoryId}", category.Id);
        return new CreateCategoryResponse(category.Id);
    }
}
