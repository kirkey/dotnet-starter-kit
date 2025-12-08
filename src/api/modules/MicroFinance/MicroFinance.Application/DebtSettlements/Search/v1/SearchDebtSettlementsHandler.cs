using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Search.v1;

public sealed class SearchDebtSettlementsHandler(
    [FromKeyedServices("microfinance:debtsettlements")] IReadRepository<DebtSettlement> repository)
    : IRequestHandler<SearchDebtSettlementsCommand, PagedList<DebtSettlementSummaryResponse>>
{
    public async Task<PagedList<DebtSettlementSummaryResponse>> Handle(
        SearchDebtSettlementsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDebtSettlementsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<DebtSettlementSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
