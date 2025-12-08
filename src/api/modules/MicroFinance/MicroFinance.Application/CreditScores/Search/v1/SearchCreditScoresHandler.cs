using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Search.v1;

public sealed class SearchCreditScoresHandler(
    [FromKeyedServices("microfinance:creditscores")] IReadRepository<CreditScore> repository)
    : IRequestHandler<SearchCreditScoresCommand, PagedList<CreditScoreSummaryResponse>>
{
    public async Task<PagedList<CreditScoreSummaryResponse>> Handle(
        SearchCreditScoresCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCreditScoresSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CreditScoreSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
