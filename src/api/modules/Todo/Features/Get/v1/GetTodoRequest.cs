using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.Get.v1;
public class GetTodoRequest : IRequest<GetTodoResponse>
{
    public DefaultIdType Id { get; set; }
    public GetTodoRequest(DefaultIdType id) => Id = id;
}
