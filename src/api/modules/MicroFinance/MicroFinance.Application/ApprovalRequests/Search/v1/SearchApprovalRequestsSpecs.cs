using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Search.v1;

public class SearchApprovalRequestsSpecs : EntitiesByPaginationFilterSpec<ApprovalRequest, ApprovalRequestSummaryResponse>
{
    public SearchApprovalRequestsSpecs(SearchApprovalRequestsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.SubmittedAt, !command.HasOrderBy())
            .Where(x => x.RequestNumber.Contains(command.RequestNumber!), !string.IsNullOrWhiteSpace(command.RequestNumber))
            .Where(x => x.WorkflowId == command.WorkflowId!.Value, command.WorkflowId.HasValue)
            .Where(x => x.EntityType == command.EntityType, !string.IsNullOrWhiteSpace(command.EntityType))
            .Where(x => x.EntityId == command.EntityId!.Value, command.EntityId.HasValue)
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.CurrentLevel == command.CurrentLevel!.Value, command.CurrentLevel.HasValue)
            .Where(x => x.SubmittedById == command.SubmittedById!.Value, command.SubmittedById.HasValue)
            .Where(x => x.SubmittedAt >= command.SubmittedAtFrom!.Value, command.SubmittedAtFrom.HasValue)
            .Where(x => x.SubmittedAt <= command.SubmittedAtTo!.Value, command.SubmittedAtTo.HasValue)
            .Where(x => x.Amount >= command.MinAmount!.Value, command.MinAmount.HasValue)
            .Where(x => x.Amount <= command.MaxAmount!.Value, command.MaxAmount.HasValue);
}
