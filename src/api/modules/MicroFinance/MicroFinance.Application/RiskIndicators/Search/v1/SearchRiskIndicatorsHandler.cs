using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Search.v1;

public sealed class SearchRiskIndicatorsHandler(
    [FromKeyedServices("microfinance:riskindicators")] IReadRepository<RiskIndicator> repository)
    : IRequestHandler<SearchRiskIndicatorsCommand, PagedList<RiskIndicatorSummaryResponse>>
{
    public async Task<PagedList<RiskIndicatorSummaryResponse>> Handle(
        SearchRiskIndicatorsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchRiskIndicatorsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<RiskIndicatorSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
