namespace FSH.Starter.WebApi.Todo.Features.Update.v1;

/// <summary>
/// Response for updating an existing todo item.
/// Contains the unique identifier of the updated todo item.
/// </summary>
public record UpdateTodoResponse(
    /// <summary>The unique identifier of the updated todo item.</summary>
    DefaultIdType? Id);

