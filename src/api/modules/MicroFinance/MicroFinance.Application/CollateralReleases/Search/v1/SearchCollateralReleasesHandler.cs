using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Search.v1;

public sealed class SearchCollateralReleasesHandler(
    [FromKeyedServices("microfinance:collateralreleases")] IReadRepository<CollateralRelease> repository)
    : IRequestHandler<SearchCollateralReleasesCommand, PagedList<CollateralReleaseSummaryResponse>>
{
    public async Task<PagedList<CollateralReleaseSummaryResponse>> Handle(
        SearchCollateralReleasesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCollateralReleasesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CollateralReleaseSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
