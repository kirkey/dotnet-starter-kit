using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Queries;

public sealed class ProjectByIdSpec :
    Specification<Project, ProjectResponse>,
    ISingleResultSpecification<Project, ProjectResponse>
{
    public ProjectByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
