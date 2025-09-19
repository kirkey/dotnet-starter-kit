using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Search;

public sealed class SearchProjectsSpec : EntitiesByPaginationFilterSpec<Project, ProjectResponse>
{
    public SearchProjectsSpec(SearchProjectsRequest request) : base(request)
    {
        Query
            .OrderBy(p => p.Name!, !request.HasOrderBy())
            .Where(p => p.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(p => p.ClientName!.Contains(request.ClientName!), !string.IsNullOrEmpty(request.ClientName))
            .Where(p => p.Department!.Contains(request.Department!), !string.IsNullOrEmpty(request.Department));
    }
}


