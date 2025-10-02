using Accounting.Application.Projects.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Search.v1;
public class SearchProjectsSpecs : EntitiesByPaginationFilterSpec<Project, ProjectResponse>
{
    public SearchProjectsSpecs(SearchProjectsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy());
}
