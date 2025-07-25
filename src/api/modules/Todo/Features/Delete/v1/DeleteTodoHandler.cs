﻿using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Todo.Domain;
using FSH.Starter.WebApi.Todo.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Todo.Features.Delete.v1;
public sealed class DeleteTodoHandler(
    ILogger<DeleteTodoHandler> logger,
    [FromKeyedServices("todo")] IRepository<TodoItem> repository)
    : IRequestHandler<DeleteTodoCommand>
{
    public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var todoItem = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = todoItem ?? throw new TodoItemNotFoundException(request.Id);
        await repository.DeleteAsync(todoItem, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("todo with id : {TodoId} successfully deleted", todoItem.Id);
    }
}
