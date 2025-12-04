using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Search.v1;

/// <summary>
/// Handler for searching member groups with filters and pagination.
/// </summary>
public sealed class SearchMemberGroupsHandler(
    ILogger<SearchMemberGroupsHandler> logger,
    [FromKeyedServices("microfinance:membergroups")] IReadRepository<MemberGroup> repository)
    : IRequestHandler<SearchMemberGroupsCommand, PagedList<MemberGroupResponse>>
{
    public async Task<PagedList<MemberGroupResponse>> Handle(SearchMemberGroupsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchMemberGroupsSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Searched member groups with {Count} results", items.Count);

        return new PagedList<MemberGroupResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
