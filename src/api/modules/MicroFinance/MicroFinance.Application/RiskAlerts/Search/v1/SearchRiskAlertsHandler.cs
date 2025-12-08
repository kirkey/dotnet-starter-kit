using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Search.v1;

public sealed class SearchRiskAlertsHandler(
    [FromKeyedServices("microfinance:riskalerts")] IReadRepository<RiskAlert> repository)
    : IRequestHandler<SearchRiskAlertsCommand, PagedList<RiskAlertSummaryResponse>>
{
    public async Task<PagedList<RiskAlertSummaryResponse>> Handle(
        SearchRiskAlertsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchRiskAlertsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<RiskAlertSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
