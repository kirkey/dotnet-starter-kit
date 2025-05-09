using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.App.Features.Dtos;
using FSH.Starter.WebApi.App.Features.GetList.v1;
using MediatR;

namespace FSH.Starter.WebApi.App.Features.Search.v1;

public class GroupSearchCommand : PaginationFilter, IRequest<PagedList<GroupDto>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
