using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Search.v1;

public class SearchCollectionStrategiesSpecs : EntitiesByPaginationFilterSpec<CollectionStrategy, CollectionStrategySummaryResponse>
{
    public SearchCollectionStrategiesSpecs(SearchCollectionStrategiesCommand command)
        : base(command) =>
        Query
            .OrderBy(x => x.Priority, !command.HasOrderBy())
            .ThenBy(x => x.TriggerDaysPastDue)
            .Where(x => x.Code == command.Code, !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.LoanProductId == command.LoanProductId!.Value, command.LoanProductId.HasValue)
            .Where(x => x.ActionType == command.ActionType, !string.IsNullOrWhiteSpace(command.ActionType))
            .Where(x => x.TriggerDaysPastDue >= command.MinDaysPastDue!.Value, command.MinDaysPastDue.HasValue)
            .Where(x => x.TriggerDaysPastDue <= command.MaxDaysPastDue!.Value, command.MaxDaysPastDue.HasValue)
            .Where(x => x.IsActive == command.IsActive!.Value, command.IsActive.HasValue)
            .Where(x => x.RequiresApproval == command.RequiresApproval!.Value, command.RequiresApproval.HasValue);
}
