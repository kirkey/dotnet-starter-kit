using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Search.v1;

public class SearchFeeWaiversSpecs : EntitiesByPaginationFilterSpec<FeeWaiver, FeeWaiverSummaryResponse>
{
    public SearchFeeWaiversSpecs(SearchFeeWaiversCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.RequestDate, !command.HasOrderBy())
            .ThenByDescending(x => x.CreatedOn)
            .Where(x => x.FeeChargeId == command.FeeChargeId!.Value, command.FeeChargeId.HasValue)
            .Where(x => x.Reference.Contains(command.Reference!), !string.IsNullOrWhiteSpace(command.Reference))
            .Where(x => x.WaiverType == command.WaiverType, !string.IsNullOrWhiteSpace(command.WaiverType))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.RequestDate >= command.RequestDateFrom!.Value, command.RequestDateFrom.HasValue)
            .Where(x => x.RequestDate <= command.RequestDateTo!.Value, command.RequestDateTo.HasValue);
}
