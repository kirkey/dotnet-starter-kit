namespace FSH.Starter.WebApi.Todo.Features.TodoItems.Export;

/// <summary>
/// DTO representing a Todo Item for Excel export.
/// Contains all relevant fields formatted for export.
/// </summary>
public sealed class TodoItemExportDto
{
    /// <summary>
    /// The title of the todo item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The description of the todo item.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Priority level.
    /// </summary>
    public string Priority { get; set; } = string.Empty;

    /// <summary>
    /// Due date for the todo item.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Indicates whether the item is completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Completion date if completed.
    /// </summary>
    public DateTime? CompletedDate { get; set; }

    /// <summary>
    /// Tags associated with the todo item.
    /// </summary>
    public string Tags { get; set; } = string.Empty;

    /// <summary>
    /// Category of the todo item.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Date the item was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// User who created the item.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}

