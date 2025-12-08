using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Search.v1;

public class SearchMarketingCampaignsSpecs : EntitiesByPaginationFilterSpec<MarketingCampaign, MarketingCampaignSummaryResponse>
{
    public SearchMarketingCampaignsSpecs(SearchMarketingCampaignsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.StartDate, !command.HasOrderBy())
            .Where(x => x.Code.Contains(command.Code!), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.CampaignType == command.CampaignType, !string.IsNullOrWhiteSpace(command.CampaignType))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.StartDate >= command.StartDateFrom!.Value, command.StartDateFrom.HasValue)
            .Where(x => x.StartDate <= command.StartDateTo!.Value, command.StartDateTo.HasValue)
            .Where(x => x.EndDate >= command.EndDateFrom!.Value, command.EndDateFrom.HasValue)
            .Where(x => x.EndDate <= command.EndDateTo!.Value, command.EndDateTo.HasValue)
            .Where(x => x.Channels.Contains(command.Channels!), !string.IsNullOrWhiteSpace(command.Channels))
            .Where(x => x.Budget >= command.MinBudget!.Value, command.MinBudget.HasValue)
            .Where(x => x.Budget <= command.MaxBudget!.Value, command.MaxBudget.HasValue)
            .Where(x => x.CreatedById == command.CreatedById!.Value, command.CreatedById.HasValue)
            .Where(x => x.ApprovedById == command.ApprovedById!.Value, command.ApprovedById.HasValue);
}
