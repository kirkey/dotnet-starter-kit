using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Categories.Create.v1;

public sealed class CreateCategoryHandler(
    ILogger<CreateCategoryHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Category> repository)
    : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = Category.Create(request.Name, request.Code, request.Description, request.IsActive);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("category created {CategoryId}", entity.Id);
        return new CreateCategoryResponse(entity.Id);
    }
}
