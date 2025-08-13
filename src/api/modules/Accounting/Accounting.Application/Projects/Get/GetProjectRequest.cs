using MediatR;
using Accounting.Application.Projects.Dtos;

namespace Accounting.Application.Projects.Get;

public class GetProjectRequest : IRequest<ProjectDto>
{
    public DefaultIdType Id { get; set; }

    public GetProjectRequest(DefaultIdType id)
    {
        Id = id;
    }
}
