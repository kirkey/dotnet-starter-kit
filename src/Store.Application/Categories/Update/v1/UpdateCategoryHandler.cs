using Store.Domain.Exceptions.Category;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

/// <summary>
/// Handles update operations for Category entities.
/// If an image payload is provided by the client, it uploads the image using the configured storage service
/// and sets the Category.ImageUrl to the saved file's public URI.
/// </summary>
public sealed class UpdateCategoryHandler(
    ILogger<UpdateCategoryHandler> logger,
    [FromKeyedServices("store:categories")] IRepository<Category> repository,
    IStorageService storageService)
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    /// <summary>
    /// Processes the update command, optionally handling image upload and setting the resulting ImageUrl.
    /// Validates inputs and ensures that when an image payload is provided it is saved and the resulting
    /// absolute URI (as returned by the storage provider) is persisted on the Category.
    /// </summary>
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var category = await repository.GetByIdAsync(command.Id, cancellationToken).ConfigureAwait(false);
        _ = category ?? throw new CategoryNotFoundException(command.Id);

        // Determine the image URL to save on the Category.
        // Priority: uploaded image URI > explicitly provided ImageUrl > keep existing value.
        string? imageUrl = category.ImageUrl;
        if (command.Image is not null && !string.IsNullOrWhiteSpace(command.Image.Data))
        {
            var uri = await storageService.UploadAsync<Category>(command.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            if (uri is null)
            {
                // Guard: storage provider should return a URI for a successful upload
                throw new InvalidOperationException("Image upload failed: storage provider returned no URI.");
            }

            // Persist the full absolute URI returned by the storage provider so it can be used by clients.
            imageUrl = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }
        else if (!string.IsNullOrWhiteSpace(command.ImageUrl))
        {
            imageUrl = command.ImageUrl;
        }

        var updated = category.Update(
            command.Name,
            command.Description,
            command.Code,
            command.ParentCategoryId,
            command.IsActive,
            command.SortOrder,
            imageUrl);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Category with id: {CategoryId} updated. ImageUrl set: {ImageUrl}", category.Id, imageUrl);
        return new UpdateCategoryResponse(category.Id);
    }
}
