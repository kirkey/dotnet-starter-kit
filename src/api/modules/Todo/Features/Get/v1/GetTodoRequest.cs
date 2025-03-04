using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.Get.v1;
public class GetTodoRequest(DefaultIdType id) : IRequest<GetTodoResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
