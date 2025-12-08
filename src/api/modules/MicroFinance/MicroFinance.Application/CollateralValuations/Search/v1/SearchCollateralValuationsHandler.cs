using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Search.v1;

public sealed class SearchCollateralValuationsHandler(
    [FromKeyedServices("microfinance:collateralvaluations")] IReadRepository<CollateralValuation> repository)
    : IRequestHandler<SearchCollateralValuationsCommand, PagedList<CollateralValuationSummaryResponse>>
{
    public async Task<PagedList<CollateralValuationSummaryResponse>> Handle(
        SearchCollateralValuationsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCollateralValuationsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CollateralValuationSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
