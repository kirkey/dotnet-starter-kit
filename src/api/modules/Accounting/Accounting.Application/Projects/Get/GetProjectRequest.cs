using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Get;

public class GetProjectQuery(DefaultIdType id) : IRequest<ProjectResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
