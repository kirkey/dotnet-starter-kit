using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Todo.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Todo.Features.Create.v1;

/// <summary>
/// Handler for creating a new todo item.
/// Validates input, creates the todo item entity, and persists it to the database.
/// </summary>
public sealed class CreateTodoHandler(
    ILogger<CreateTodoHandler> logger,
    [FromKeyedServices("todo")] IRepository<TodoItem> repository)
    : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
{
    /// <summary>
    /// Handles the CreateTodoCommand request.
    /// </summary>
    /// <param name="request">The create todo command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A response containing the newly created todo item ID.</returns>
    public async Task<CreateTodoResponse> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var item = TodoItem.Create(request.Name, request.Description, request.Notes);
        await repository.AddAsync(item, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Todo item created with ID: {TodoItemId}, Name: {TodoName}", item.Id, item.Name);
        
        return new CreateTodoResponse(item.Id);
    }
}
