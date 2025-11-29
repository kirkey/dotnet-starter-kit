namespace FSH.Starter.WebApi.Todo.Features.Import;

/// <summary>
/// Represents a single row of data from a Todo import file.
/// Contains all fields that can be imported for a TodoItem entity.
/// </summary>
public sealed class TodoImportRow
{
    /// <summary>
    /// Gets or sets the name of the todo item.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the todo item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the notes for the todo item.
    /// </summary>
    public string? Notes { get; set; }
}
