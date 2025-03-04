using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.Update.v1;
public sealed record UpdateTodoCommand(
    DefaultIdType Id,
    string? Title,
    string? Note = null): IRequest<UpdateTodoResponse>;



