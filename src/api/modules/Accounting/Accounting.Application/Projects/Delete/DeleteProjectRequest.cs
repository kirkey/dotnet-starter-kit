namespace Accounting.Application.Projects.Delete;

public class DeleteProjectRequest(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}
