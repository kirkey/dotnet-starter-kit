using Store.Domain.Exceptions.Category;

namespace FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

public sealed class UpdateCategoryHandler(
    ILogger<UpdateCategoryHandler> logger,
    [FromKeyedServices("store:categories")] IRepository<Category> repository)
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var category = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = category ?? throw new CategoryNotFoundException(request.Id);

        var updated = category.Update(
            request.Name,
            request.Description,
            request.Code,
            request.ParentCategoryId,
            request.IsActive,
            request.SortOrder,
            request.ImageUrl);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("category with id : {CategoryId} updated.", category.Id);
        return new UpdateCategoryResponse(category.Id);
    }
}

