using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Features.GetList.v1;

namespace FSH.Starter.WebApi.App.Features.Search.v1;
public sealed class GroupSearchSpec : EntitiesByPaginationFilterSpec<Group, GroupDto>
{
    public GroupSearchSpec(GroupSearchCommand command)
        : base(command)
    {
        Query
            .OrderBy(c => c.Code, !command.HasOrderBy())
            .Where(a => a.Code.Contains(command.Keyword!)
                || a.Name.Contains(command.Keyword!)
                || a.Description!.Contains(command.Keyword!)
                || a.Notes!.Contains(command.Keyword!),
                !string.IsNullOrEmpty(command.Keyword));
    }
}
