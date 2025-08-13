using MediatR;

namespace Accounting.Application.Projects.Delete;

public class DeleteProjectRequest : IRequest
{
    public DefaultIdType Id { get; set; }

    public DeleteProjectRequest(DefaultIdType id)
    {
        Id = id;
    }
}
