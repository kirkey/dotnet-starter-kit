using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Search.v1;

public sealed class SearchInterestRateChangesHandler(
    [FromKeyedServices("microfinance:interestratechanges")] IReadRepository<InterestRateChange> repository)
    : IRequestHandler<SearchInterestRateChangesCommand, PagedList<InterestRateChangeSummaryResponse>>
{
    public async Task<PagedList<InterestRateChangeSummaryResponse>> Handle(
        SearchInterestRateChangesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInterestRateChangesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InterestRateChangeSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
