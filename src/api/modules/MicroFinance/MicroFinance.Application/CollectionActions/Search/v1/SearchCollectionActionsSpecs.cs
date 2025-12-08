using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Search.v1;

public class SearchCollectionActionsSpecs : EntitiesByPaginationFilterSpec<CollectionAction, CollectionActionSummaryResponse>
{
    public SearchCollectionActionsSpecs(SearchCollectionActionsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.ActionDateTime, !command.HasOrderBy())
            .Where(x => x.CollectionCaseId == command.CollectionCaseId!.Value, command.CollectionCaseId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.PerformedById == command.PerformedById!.Value, command.PerformedById.HasValue)
            .Where(x => x.ActionType == command.ActionType, !string.IsNullOrWhiteSpace(command.ActionType))
            .Where(x => x.Outcome == command.Outcome, !string.IsNullOrWhiteSpace(command.Outcome))
            .Where(x => x.ActionDateTime >= command.ActionDateFrom!.Value, command.ActionDateFrom.HasValue)
            .Where(x => x.ActionDateTime <= command.ActionDateTo!.Value, command.ActionDateTo.HasValue)
            .Where(x => x.NextFollowUpDate >= command.NextFollowUpDateFrom!.Value, command.NextFollowUpDateFrom.HasValue)
            .Where(x => x.NextFollowUpDate <= command.NextFollowUpDateTo!.Value, command.NextFollowUpDateTo.HasValue);
}
