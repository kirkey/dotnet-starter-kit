using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Create.v1;

/// <summary>
/// Handler for creating a new collection action.
/// </summary>
public sealed class CreateCollectionActionHandler(
    [FromKeyedServices("microfinance:collectionactions")] IRepository<CollectionAction> repository,
    ILogger<CreateCollectionActionHandler> logger)
    : IRequestHandler<CreateCollectionActionCommand, CreateCollectionActionResponse>
{
    public async Task<CreateCollectionActionResponse> Handle(CreateCollectionActionCommand request, CancellationToken cancellationToken)
    {
        var collectionAction = CollectionAction.Create(
            request.CollectionCaseId,
            request.LoanId,
            request.ActionType,
            request.PerformedById,
            request.Outcome,
            request.Description);

        await repository.AddAsync(collectionAction, cancellationToken);

        logger.LogInformation("Collection action {ActionType} created for case {CaseId} - Outcome: {Outcome}",
            request.ActionType, request.CollectionCaseId, request.Outcome);

        return new CreateCollectionActionResponse(
            collectionAction.Id,
            collectionAction.ActionType,
            collectionAction.Outcome,
            collectionAction.ActionDateTime);
    }
}
