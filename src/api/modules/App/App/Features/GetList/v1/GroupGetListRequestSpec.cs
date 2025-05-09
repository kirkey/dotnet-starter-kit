using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Features.Dtos;

namespace FSH.Starter.WebApi.App.Features.GetList.v1;

public sealed class GroupGetListRequestSpec : EntitiesByPaginationFilterSpec<Group, GroupDto>
{
    public GroupGetListRequestSpec(GroupGetListRequest request)
        : base(request.Filter)
    {
        Query
            .AsNoTracking()
            .OrderBy(o => o.Code, !request.Filter.HasOrderBy());
    }
}
