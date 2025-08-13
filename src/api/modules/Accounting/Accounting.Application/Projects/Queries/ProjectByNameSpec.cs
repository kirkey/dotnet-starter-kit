using Accounting.Application.Projects.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Projects.Queries;

public sealed class ProjectByNameSpec :
    Specification<Project, ProjectDto>,
    ISingleResultSpecification<Project, ProjectDto>
{
    public ProjectByNameSpec(string name) =>
        Query.Where(w => w.Name == name);
}
