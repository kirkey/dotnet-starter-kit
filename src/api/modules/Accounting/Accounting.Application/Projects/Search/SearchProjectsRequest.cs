using Accounting.Application.Projects.Dtos;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.Projects.Search;

public class SearchProjectsRequest : PaginationFilter, IRequest<PagedList<ProjectDto>>
{
    public string? Name { get; set; }
    public string? ClientName { get; set; }
    public string? Department { get; set; }
}


