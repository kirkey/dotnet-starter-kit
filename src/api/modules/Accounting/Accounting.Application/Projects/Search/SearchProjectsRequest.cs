using Accounting.Application.Projects.Responses;

using PaginationFilter = FSH.Framework.Core.Paging.PaginationFilter;

namespace Accounting.Application.Projects.Search;

public class SearchProjectsRequest : PaginationFilter, IRequest<PagedList<ProjectResponse>>
{
    public string? Name { get; set; }
    public string? ClientName { get; set; }
    public string? Department { get; set; }
}
