using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordRecovery.v1;

public sealed class RecordCollectionCaseRecoveryHandler(
    ILogger<RecordCollectionCaseRecoveryHandler> logger,
    [FromKeyedServices("microfinance:collectioncases")] IRepository<CollectionCase> repository)
    : IRequestHandler<RecordCollectionCaseRecoveryCommand, RecordCollectionCaseRecoveryResponse>
{
    public async Task<RecordCollectionCaseRecoveryResponse> Handle(RecordCollectionCaseRecoveryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collectionCase = await repository.FirstOrDefaultAsync(
            new CollectionCaseByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (collectionCase is null)
            throw new NotFoundException($"Collection case with ID {request.Id} not found.");

        collectionCase.RecordRecovery(request.Amount);

        await repository.UpdateAsync(collectionCase, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Recovery recorded for case {CaseId}: {Amount}, Status: {Status}",
            collectionCase.Id, request.Amount, collectionCase.Status);

        return new RecordCollectionCaseRecoveryResponse(
            collectionCase.Id,
            collectionCase.AmountRecovered,
            collectionCase.AmountOverdue,
            collectionCase.Status);
    }
}
