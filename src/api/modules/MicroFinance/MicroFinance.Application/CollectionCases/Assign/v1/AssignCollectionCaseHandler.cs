using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Assign.v1;

public sealed class AssignCollectionCaseHandler(
    ILogger<AssignCollectionCaseHandler> logger,
    [FromKeyedServices("microfinance:collectioncases")] IRepository<CollectionCase> repository)
    : IRequestHandler<AssignCollectionCaseCommand, AssignCollectionCaseResponse>
{
    public async Task<AssignCollectionCaseResponse> Handle(AssignCollectionCaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collectionCase = await repository.FirstOrDefaultAsync(
            new CollectionCaseByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (collectionCase is null)
            throw new NotFoundException($"Collection case with ID {request.Id} not found.");

        collectionCase.Assign(request.CollectorId, request.FollowUpDate);

        await repository.UpdateAsync(collectionCase, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Collection case assigned: {CaseId} to collector {CollectorId}",
            collectionCase.Id, request.CollectorId);

        return new AssignCollectionCaseResponse(
            collectionCase.Id,
            request.CollectorId,
            collectionCase.Status,
            collectionCase.NextFollowUpDate);
    }
}
