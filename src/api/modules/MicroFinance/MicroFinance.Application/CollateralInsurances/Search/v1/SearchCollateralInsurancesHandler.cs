using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Search.v1;

public sealed class SearchCollateralInsurancesHandler(
    [FromKeyedServices("microfinance:collateralinsurances")] IReadRepository<CollateralInsurance> repository)
    : IRequestHandler<SearchCollateralInsurancesCommand, PagedList<CollateralInsuranceSummaryResponse>>
{
    public async Task<PagedList<CollateralInsuranceSummaryResponse>> Handle(
        SearchCollateralInsurancesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCollateralInsurancesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CollateralInsuranceSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
