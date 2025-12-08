using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Search.v1;

public class SearchRiskIndicatorsSpecs : EntitiesByPaginationFilterSpec<RiskIndicator, RiskIndicatorSummaryResponse>
{
    public SearchRiskIndicatorsSpecs(SearchRiskIndicatorsCommand command)
        : base(command) =>
        Query
            .OrderBy(x => x.Code, !command.HasOrderBy())
            .Where(x => x.RiskCategoryId == command.RiskCategoryId!.Value, command.RiskCategoryId.HasValue)
            .Where(x => x.Code.Contains(command.Code!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.Direction == command.Direction, !string.IsNullOrWhiteSpace(command.Direction))
            .Where(x => x.Frequency == command.Frequency, !string.IsNullOrWhiteSpace(command.Frequency))
            .Where(x => x.TargetValue >= command.TargetValueMin!.Value, command.TargetValueMin.HasValue)
            .Where(x => x.TargetValue <= command.TargetValueMax!.Value, command.TargetValueMax.HasValue);
}
