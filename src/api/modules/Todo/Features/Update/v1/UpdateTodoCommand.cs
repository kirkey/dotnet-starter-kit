using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.Update.v1;
public sealed record UpdateTodoCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? Notes = null): IRequest<UpdateTodoResponse>;



