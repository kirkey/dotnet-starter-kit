﻿using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Todo.Domain;
using FSH.Starter.WebApi.Todo.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Todo.Features.Update.v1;

/// <summary>
/// Handler for updating an existing todo item.
/// Validates input, retrieves the todo item, updates it, and persists changes.
/// </summary>
public sealed class UpdateTodoHandler(
    ILogger<UpdateTodoHandler> logger,
    [FromKeyedServices("todo")] IRepository<TodoItem> repository)
    : IRequestHandler<UpdateTodoCommand, UpdateTodoResponse>
{
    /// <summary>
    /// Handles the UpdateTodoCommand request.
    /// </summary>
    /// <param name="request">The update todo command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A response containing the updated todo item ID.</returns>
    public async Task<UpdateTodoResponse> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var todo = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = todo ?? throw new TodoItemNotFoundException(request.Id);
        
        var updatedTodo = todo.Update(request.Name, request.Description, request.Notes);
        await repository.UpdateAsync(updatedTodo, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Todo item updated with ID: {TodoItemId}, Name: {TodoName}", updatedTodo.Id, updatedTodo.Name);
        
        return new UpdateTodoResponse(updatedTodo.Id);
    }
}
