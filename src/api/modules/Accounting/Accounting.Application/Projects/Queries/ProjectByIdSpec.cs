using Accounting.Application.Projects.Dtos;

namespace Accounting.Application.Projects.Queries;

public sealed class ProjectByIdSpec :
    Specification<Project, ProjectDto>,
    ISingleResultSpecification<Project, ProjectDto>
{
    public ProjectByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
