using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Get.v1;

/// <summary>
/// Handler for getting a collection action by ID.
/// </summary>
public sealed class GetCollectionActionHandler(
    [FromKeyedServices("microfinance:collectionactions")] IReadRepository<CollectionAction> repository,
    ILogger<GetCollectionActionHandler> logger)
    : IRequestHandler<GetCollectionActionRequest, CollectionActionResponse>
{
    public async Task<CollectionActionResponse> Handle(GetCollectionActionRequest request, CancellationToken cancellationToken)
    {
        var spec = new CollectionActionByIdSpec(request.Id);
        var collectionAction = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (collectionAction is null)
        {
            throw new NotFoundException($"Collection action with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved collection action {CollectionActionId}", request.Id);

        return collectionAction;
    }
}
