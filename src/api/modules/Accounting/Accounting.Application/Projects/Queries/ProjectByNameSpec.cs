using Accounting.Application.Projects.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Queries;

public sealed class ProjectByNameSpec :
    Specification<Project, ProjectResponse>,
    ISingleResultSpecification<Project, ProjectResponse>
{
    public ProjectByNameSpec(string name) =>
        Query.Where(w => w.Name == name);
}
