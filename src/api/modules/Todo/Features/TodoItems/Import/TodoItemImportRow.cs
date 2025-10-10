namespace FSH.Starter.WebApi.Todo.Features.TodoItems.Import;

/// <summary>
/// DTO representing a single row from the Todo Items import Excel file.
/// Properties must match Excel column headers for automatic mapping.
/// </summary>
public sealed class TodoItemImportRow
{
    /// <summary>
    /// The title of the todo item.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The description of the todo item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Priority level (e.g., Low, Medium, High).
    /// </summary>
    public string? Priority { get; set; }

    /// <summary>
    /// Due date for the todo item.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Indicates whether the item is completed.
    /// </summary>
    public bool? IsCompleted { get; set; }

    /// <summary>
    /// Tags associated with the todo item (comma-separated).
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Category of the todo item.
    /// </summary>
    public string? Category { get; set; }
}

