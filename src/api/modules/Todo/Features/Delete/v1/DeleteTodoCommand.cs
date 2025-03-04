using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.Delete.v1;
public sealed record DeleteTodoCommand(
    DefaultIdType Id) : IRequest;



