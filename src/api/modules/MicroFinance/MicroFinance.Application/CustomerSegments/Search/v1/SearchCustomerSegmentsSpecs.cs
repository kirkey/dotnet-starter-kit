using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Search.v1;

public class SearchCustomerSegmentsSpecs : EntitiesByPaginationFilterSpec<CustomerSegment, CustomerSegmentSummaryResponse>
{
    public SearchCustomerSegmentsSpecs(SearchCustomerSegmentsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.Priority, !command.HasOrderBy())
            .Where(x => x.Code.Contains(command.Code!), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.Name.Contains(command.Name!), !string.IsNullOrWhiteSpace(command.Name))
            .Where(x => x.SegmentType == command.SegmentType, !string.IsNullOrWhiteSpace(command.SegmentType))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.RiskLevel == command.RiskLevel, !string.IsNullOrWhiteSpace(command.RiskLevel))
            .Where(x => x.MinIncomeLevel >= command.MinIncomeLevel!.Value, command.MinIncomeLevel.HasValue)
            .Where(x => x.MaxIncomeLevel <= command.MaxIncomeLevel!.Value, command.MaxIncomeLevel.HasValue)
            .Where(x => x.Priority >= command.MinPriority!.Value, command.MinPriority.HasValue)
            .Where(x => x.Priority <= command.MaxPriority!.Value, command.MaxPriority.HasValue);
}
