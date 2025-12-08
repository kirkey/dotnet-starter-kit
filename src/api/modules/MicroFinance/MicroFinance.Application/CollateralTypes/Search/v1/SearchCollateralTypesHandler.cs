using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Search.v1;

public sealed class SearchCollateralTypesHandler(
    [FromKeyedServices("microfinance:collateraltypes")] IReadRepository<CollateralType> repository)
    : IRequestHandler<SearchCollateralTypesCommand, PagedList<CollateralTypeSummaryResponse>>
{
    public async Task<PagedList<CollateralTypeSummaryResponse>> Handle(
        SearchCollateralTypesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCollateralTypesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CollateralTypeSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
