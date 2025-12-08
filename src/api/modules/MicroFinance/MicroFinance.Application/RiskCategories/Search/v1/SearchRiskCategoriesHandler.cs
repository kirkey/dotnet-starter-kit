using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Search.v1;

public sealed class SearchRiskCategoriesHandler(
    [FromKeyedServices("microfinance:riskcategories")] IReadRepository<RiskCategory> repository)
    : IRequestHandler<SearchRiskCategoriesCommand, PagedList<RiskCategorySummaryResponse>>
{
    public async Task<PagedList<RiskCategorySummaryResponse>> Handle(
        SearchRiskCategoriesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchRiskCategoriesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<RiskCategorySummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
