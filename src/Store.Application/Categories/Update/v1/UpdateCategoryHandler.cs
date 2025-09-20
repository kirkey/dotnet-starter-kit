using Store.Domain.Exceptions.Category;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

/// <summary>
/// Handles update operations for Category entities.
/// If an image payload is provided by the client, it uploads the image using the local storage service
/// and sets ImageUrl from the uploaded file URL.
/// </summary>
public sealed class UpdateCategoryHandler(
    ILogger<UpdateCategoryHandler> logger,
    [FromKeyedServices("store:categories")] IRepository<Category> repository,
    IStorageService storageService)
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    /// <summary>
    /// Processes the update command, optionally handling image upload and setting the resulting ImageUrl.
    /// </summary>
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var category = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = category ?? throw new CategoryNotFoundException(request.Id);

        // If client uploaded an image, save it and use the uploaded filename as ImageUrl.
        string? imageUrl = request.ImageUrl;
        if (request.Image is not null)
        {
            var uri = await storageService.UploadAsync<Category>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            // Persist only the file name, as requested.
            imageUrl = Path.GetFileName(uri.LocalPath);
        }

        var updated = category.Update(
            request.Name,
            request.Description,
            request.Code,
            request.ParentCategoryId,
            request.IsActive,
            request.SortOrder,
            imageUrl);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("category with id : {CategoryId} updated.", category.Id);
        return new UpdateCategoryResponse(category.Id);
    }
}
