using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Search.v1;

public sealed class SearchMarketingCampaignsHandler(
    [FromKeyedServices("microfinance:marketingcampaigns")] IReadRepository<MarketingCampaign> repository)
    : IRequestHandler<SearchMarketingCampaignsCommand, PagedList<MarketingCampaignSummaryResponse>>
{
    public async Task<PagedList<MarketingCampaignSummaryResponse>> Handle(
        SearchMarketingCampaignsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchMarketingCampaignsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<MarketingCampaignSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
