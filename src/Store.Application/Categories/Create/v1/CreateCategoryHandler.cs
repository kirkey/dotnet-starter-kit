namespace FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

public sealed class CreateCategoryHandler(
    ILogger<CreateCategoryHandler> logger,
    [FromKeyedServices("store:categories")] IRepository<Category> repository)
    : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var category = Category.Create(
            request.Name!,
            request.Description,
            request.Code!,
            request.ParentCategoryId,
            request.IsActive,
            request.SortOrder,
            request.ImageUrl);

        await repository.AddAsync(category, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("category created {CategoryId}", category.Id);
        return new CreateCategoryResponse(category.Id);
    }
}

