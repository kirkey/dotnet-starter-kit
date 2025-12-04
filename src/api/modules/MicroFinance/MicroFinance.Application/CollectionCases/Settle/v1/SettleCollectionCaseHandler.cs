using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Settle.v1;

public sealed class SettleCollectionCaseHandler(
    ILogger<SettleCollectionCaseHandler> logger,
    [FromKeyedServices("microfinance:collectioncases")] IRepository<CollectionCase> repository)
    : IRequestHandler<SettleCollectionCaseCommand, SettleCollectionCaseResponse>
{
    public async Task<SettleCollectionCaseResponse> Handle(SettleCollectionCaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collectionCase = await repository.FirstOrDefaultAsync(
            new CollectionCaseByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (collectionCase is null)
            throw new NotFoundException($"Collection case with ID {request.Id} not found.");

        collectionCase.Settle(request.SettlementAmount, request.Terms);

        await repository.UpdateAsync(collectionCase, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Collection case settled: {CaseId}, Amount: {Amount}",
            collectionCase.Id, request.SettlementAmount);

        return new SettleCollectionCaseResponse(
            collectionCase.Id,
            collectionCase.Status,
            collectionCase.ClosedDate!.Value,
            collectionCase.ClosureReason!);
    }
}
