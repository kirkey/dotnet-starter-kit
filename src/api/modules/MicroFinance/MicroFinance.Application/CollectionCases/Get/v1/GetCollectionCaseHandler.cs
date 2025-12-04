using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;

public sealed class GetCollectionCaseHandler(
    [FromKeyedServices("microfinance:collectioncases")] IReadRepository<CollectionCase> repository)
    : IRequestHandler<GetCollectionCaseRequest, CollectionCaseResponse>
{
    public async Task<CollectionCaseResponse> Handle(GetCollectionCaseRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collectionCase = await repository.FirstOrDefaultAsync(
            new CollectionCaseByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (collectionCase is null)
            throw new NotFoundException($"Collection case with ID {request.Id} not found.");

        return new CollectionCaseResponse(
            collectionCase.Id,
            collectionCase.CaseNumber,
            collectionCase.LoanId,
            collectionCase.MemberId,
            collectionCase.AssignedCollectorId,
            collectionCase.Status,
            collectionCase.Priority,
            collectionCase.Classification,
            collectionCase.OpenedDate,
            collectionCase.AssignedDate,
            collectionCase.ClosedDate,
            collectionCase.DaysPastDueAtOpen,
            collectionCase.CurrentDaysPastDue,
            collectionCase.AmountOverdue,
            collectionCase.TotalOutstanding,
            collectionCase.AmountRecovered,
            collectionCase.LastContactDate,
            collectionCase.NextFollowUpDate,
            collectionCase.ContactAttempts,
            collectionCase.ClosureReason,
            collectionCase.Notes);
    }
}
