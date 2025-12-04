using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Create.v1;

public sealed class CreateCollectionCaseHandler(
    ILogger<CreateCollectionCaseHandler> logger,
    [FromKeyedServices("microfinance:collectioncases")] IRepository<CollectionCase> repository)
    : IRequestHandler<CreateCollectionCaseCommand, CreateCollectionCaseResponse>
{
    public async Task<CreateCollectionCaseResponse> Handle(CreateCollectionCaseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collectionCase = CollectionCase.Create(
            caseNumber: request.CaseNumber,
            loanId: request.LoanId,
            memberId: request.MemberId,
            daysPastDue: request.DaysPastDue,
            amountOverdue: request.AmountOverdue,
            totalOutstanding: request.TotalOutstanding);

        await repository.AddAsync(collectionCase, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Collection case created: {CaseNumber}, Priority: {Priority}",
            collectionCase.CaseNumber, collectionCase.Priority);

        return new CreateCollectionCaseResponse(
            collectionCase.Id,
            collectionCase.CaseNumber,
            collectionCase.Status,
            collectionCase.Priority,
            collectionCase.Classification);
    }
}
