using Store.Domain.Exceptions.Category;

namespace FSH.Starter.WebApi.Store.Application.Categories.Delete.v1;

public sealed class DeleteCategoryHandler(
    ILogger<DeleteCategoryHandler> logger,
    [FromKeyedServices("store:categories")] IRepository<Category> repository)
    : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var category = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = category ?? throw new CategoryNotFoundException(request.Id);
        await repository.DeleteAsync(category, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("category with id : {CategoryId} successfully deleted", category.Id);
    }
}

