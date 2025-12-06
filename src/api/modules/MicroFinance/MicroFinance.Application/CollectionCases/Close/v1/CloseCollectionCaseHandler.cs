using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Close.v1;

/// <summary>
/// Handler for closing a collection case.
/// </summary>
public sealed class CloseCollectionCaseHandler(
    ILogger<CloseCollectionCaseHandler> logger,
    [FromKeyedServices("microfinance:collectioncases")] IRepository<CollectionCase> repository)
    : IRequestHandler<CloseCollectionCaseCommand, CloseCollectionCaseResponse>
{
    public async Task<CloseCollectionCaseResponse> Handle(CloseCollectionCaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collectionCase = await repository.FirstOrDefaultAsync(
            new CollectionCaseByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (collectionCase is null)
            throw new NotFoundException($"Collection case with ID {request.Id} not found.");

        collectionCase.Close(request.Notes ?? "Case closed");

        await repository.UpdateAsync(collectionCase, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Collection case closed: {CaseId}", collectionCase.Id);

        return new CloseCollectionCaseResponse(
            collectionCase.Id,
            collectionCase.Status);
    }
}
