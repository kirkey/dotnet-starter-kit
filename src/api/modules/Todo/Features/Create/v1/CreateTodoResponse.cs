namespace FSH.Starter.WebApi.Todo.Features.Create.v1;

/// <summary>
/// Response for creating a new todo item.
/// Contains the unique identifier of the newly created todo item.
/// </summary>
public record CreateTodoResponse(
    /// <summary>The unique identifier of the newly created todo item.</summary>
    DefaultIdType? Id);

