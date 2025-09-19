using Accounting.Application.Projects.Dtos;

namespace Accounting.Application.Projects.Get;

public class GetProjectQuery(DefaultIdType id) : IRequest<ProjectDto>
{
    public DefaultIdType Id { get; set; } = id;
}
