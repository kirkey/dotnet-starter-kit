using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Search.v1;

public sealed class SearchApprovalRequestsHandler(
    [FromKeyedServices("microfinance:approvalrequests")] IReadRepository<ApprovalRequest> repository)
    : IRequestHandler<SearchApprovalRequestsCommand, PagedList<ApprovalRequestSummaryResponse>>
{
    public async Task<PagedList<ApprovalRequestSummaryResponse>> Handle(
        SearchApprovalRequestsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchApprovalRequestsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ApprovalRequestSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
