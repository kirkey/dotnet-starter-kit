using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Search.v1;

public class SearchApprovalWorkflowsSpecs : EntitiesByPaginationFilterSpec<ApprovalWorkflow, ApprovalWorkflowSummaryResponse>
{
    public SearchApprovalWorkflowsSpecs(SearchApprovalWorkflowsCommand command)
        : base(command) =>
        Query
            .OrderBy(x => x.Priority, !command.HasOrderBy())
            .Where(x => x.Code.Contains(command.Code!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.EntityType == command.EntityType, !string.IsNullOrWhiteSpace(command.EntityType))
            .Where(x => x.MinAmount >= command.MinAmount!.Value, command.MinAmount.HasValue)
            .Where(x => x.MaxAmount <= command.MaxAmount!.Value, command.MaxAmount.HasValue)
            .Where(x => x.BranchId == command.BranchId, command.BranchId.HasValue)
            .Where(x => x.IsActive == command.IsActive!.Value, command.IsActive.HasValue)
            .Where(x => x.IsSequential == command.IsSequential!.Value, command.IsSequential.HasValue);
}
