using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Search.v1;

public class SearchCollateralValuationsSpecs : EntitiesByPaginationFilterSpec<CollateralValuation, CollateralValuationSummaryResponse>
{
    public SearchCollateralValuationsSpecs(SearchCollateralValuationsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.ValuationDate, !command.HasOrderBy())
            .Where(x => x.CollateralId == command.CollateralId!.Value, command.CollateralId.HasValue)
            .Where(x => x.ValuationReference == command.ValuationReference, !string.IsNullOrWhiteSpace(command.ValuationReference))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.ValuationMethod == command.ValuationMethod, !string.IsNullOrWhiteSpace(command.ValuationMethod))
            .Where(x => x.AppraiserName == command.AppraiserName, !string.IsNullOrWhiteSpace(command.AppraiserName))
            .Where(x => x.ValuationDate >= command.ValuationDateFrom!.Value, command.ValuationDateFrom.HasValue)
            .Where(x => x.ValuationDate <= command.ValuationDateTo!.Value, command.ValuationDateTo.HasValue)
            .Where(x => x.MarketValue >= command.MinMarketValue!.Value, command.MinMarketValue.HasValue)
            .Where(x => x.MarketValue <= command.MaxMarketValue!.Value, command.MaxMarketValue.HasValue);
}
