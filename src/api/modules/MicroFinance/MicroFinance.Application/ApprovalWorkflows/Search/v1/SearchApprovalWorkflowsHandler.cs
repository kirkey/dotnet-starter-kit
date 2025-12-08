using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Search.v1;

public sealed class SearchApprovalWorkflowsHandler(
    [FromKeyedServices("microfinance:approvalworkflows")] IReadRepository<ApprovalWorkflow> repository)
    : IRequestHandler<SearchApprovalWorkflowsCommand, PagedList<ApprovalWorkflowSummaryResponse>>
{
    public async Task<PagedList<ApprovalWorkflowSummaryResponse>> Handle(
        SearchApprovalWorkflowsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchApprovalWorkflowsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ApprovalWorkflowSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
