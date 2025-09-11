using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Categories.Update.v1;

public sealed class UpdateCategoryHandler(
    ILogger<UpdateCategoryHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Category> repository)
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new CategoryNotFoundException(request.Id);
        entity.Update(request.Name, request.Code, request.Description, request.IsActive);
        await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("category updated {CategoryId}", entity.Id);
        return new UpdateCategoryResponse(entity.Id);
    }
}
