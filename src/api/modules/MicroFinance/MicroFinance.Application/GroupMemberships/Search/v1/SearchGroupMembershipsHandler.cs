using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Search.v1;

public sealed class SearchGroupMembershipsHandler(
    [FromKeyedServices("microfinance:groupmemberships")] IReadRepository<GroupMembership> repository)
    : IRequestHandler<SearchGroupMembershipsCommand, PagedList<GroupMembershipResponse>>
{
    public async Task<PagedList<GroupMembershipResponse>> Handle(SearchGroupMembershipsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchGroupMembershipsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<GroupMembershipResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
