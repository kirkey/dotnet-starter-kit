using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Features.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.App.Features.GetList.v1;

public sealed class GroupGetListHandler(
    [FromKeyedServices("app:group")] IReadRepository<Group> repository)
    : IRequestHandler<GroupGetListRequest, PagedList<GroupDto>>
{
    public async Task<PagedList<GroupDto>> Handle(GroupGetListRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // var spec = new EntitiesByPaginationFilterSpec<Group, GroupDto>(request.Filter);

        var spec = new GroupGetListRequestSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<GroupDto>(items, request.Filter.PageNumber, request.Filter.PageSize, totalCount);
    }
}
