using Accounting.Application.Projects.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Projects.Queries;

public sealed class ProjectByIdSpec :
    Specification<Project, ProjectDto>,
    ISingleResultSpecification<Project, ProjectDto>
{
    public ProjectByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
