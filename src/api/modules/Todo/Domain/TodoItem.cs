using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Todo.Domain.Events;

namespace FSH.Starter.WebApi.Todo.Domain;

/// <summary>
/// Represents a todo item entity in the todo domain.
/// Each todo item has a name, description, and optional notes.
/// 
/// Notes:
/// - Name is required (1-256 characters)
/// - Description and Notes are optional and can be cleared by passing null or empty string
/// - Metrics are only incremented on successful state changes
/// - Domain events are only raised when actual changes are detected
/// </summary>
public sealed class TodoItem : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the todo item name field. (2^8 = 256)
    /// </summary>
    public const int NameMaxLength = 256;

    /// <summary>
    /// Maximum length for the todo item description field. (2^11 = 2048)
    /// </summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>
    /// Maximum length for the todo item notes field. (2^12 = 4096)
    /// </summary>
    public const int NotesMaxLength = 4096;

    /// <summary>
    /// Minimum length for the todo item name field.
    /// </summary>
    public const int NameMinLength = 1;

    private TodoItem() { }

    private TodoItem(string name, string description, string notes)
    {
        ValidateAndSetName(name);
        ValidateAndSetDescription(description);
        ValidateAndSetNotes(notes);

        // Only queue event, metrics will be incremented by the repository/application layer
        // This ensures metrics are only counted on successful persistence
        QueueDomainEvent(new TodoItemCreated(Id, Name, Description, Notes));
    }

    /// <summary>
    /// Creates a new TodoItem instance using the factory method pattern.
    /// </summary>
    /// <param name="name">The todo item name (required, 1-256 characters).</param>
    /// <param name="description">The todo item description (required, max 2048 characters).</param>
    /// <param name="notes">The todo item notes (required, max 4096 characters).</param>
    /// <returns>A new TodoItem instance.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public static TodoItem Create(string name, string description, string notes) =>
        new(name, description, notes);

    /// <summary>
    /// Updates the todo item information with validation and change detection.
    /// Only raises domain events if actual changes are detected.
    /// Allows clearing optional fields (Description and Notes) by passing null or empty string.
    /// </summary>
    /// <param name="name">The updated todo item name. If null or whitespace, name is not updated.</param>
    /// <param name="description">The updated todo item description. Can be set to null to clear. If null/whitespace, not updated.</param>
    /// <param name="notes">The updated todo item notes. Can be set to null to clear. If null/whitespace, not updated.</param>
    /// <returns>The updated TodoItem instance.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public TodoItem Update(string? name, string? description, string? notes)
    {
        bool hasChanges = false;

        // Update name only if provided (non-null, non-whitespace)
        if (!string.IsNullOrWhiteSpace(name))
        {
            string trimmedName = name.Trim();
            if (!string.Equals(Name, trimmedName, StringComparison.OrdinalIgnoreCase))
            {
                ValidateAndSetName(trimmedName);
                hasChanges = true;
            }
        }

        // Update description only if explicitly provided (even if clearing with empty string)
        // Note: This follows the convention where null parameter means "don't update",
        // but we still allow passing empty string through the controller/validator if needed
        if (description != null)
        {
            string? trimmedDescription = description.Length > 0 ? description.Trim() : null;
            
            // Check if there's an actual change
            if (!string.Equals(Description, trimmedDescription, StringComparison.OrdinalIgnoreCase))
            {
                ValidateAndSetDescription(trimmedDescription);
                hasChanges = true;
            }
        }

        // Update notes only if explicitly provided (even if clearing with empty string)
        // Note: This follows the convention where null parameter means "don't update"
        if (notes != null)
        {
            string? trimmedNotes = notes.Length > 0 ? notes.Trim() : null;
            
            // Check if there's an actual change
            if (!string.Equals(Notes, trimmedNotes, StringComparison.OrdinalIgnoreCase))
            {
                ValidateAndSetNotes(trimmedNotes);
                hasChanges = true;
            }
        }

        // Only raise event if changes were detected
        // Metrics are incremented by the application layer after successful persistence
        if (hasChanges)
        {
            QueueDomainEvent(new TodoItemUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Validates and sets the todo item name.
    /// Name is required and cannot be empty.
    /// </summary>
    /// <param name="name">The name to validate and set.</param>
    /// <exception cref="ArgumentException">Thrown when name is invalid.</exception>
    private void ValidateAndSetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Todo item name cannot be empty.", nameof(name));
        }

        string trimmedName = name.Trim();

        if (trimmedName.Length < NameMinLength)
        {
            throw new ArgumentException($"Todo item name must be at least {NameMinLength} character.", nameof(name));
        }

        if (trimmedName.Length > NameMaxLength)
        {
            throw new ArgumentException($"Todo item name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        Name = trimmedName;
    }

    /// <summary>
    /// Validates and sets the todo item description.
    /// Description is optional and can be cleared by passing null.
    /// </summary>
    /// <param name="description">The description to validate and set. Can be null to clear the description.</param>
    /// <exception cref="ArgumentException">Thrown when description exceeds maximum length.</exception>
    private void ValidateAndSetDescription(string? description)
    {
        // Allow null to clear description
        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
            return;
        }

        string trimmedDescription = description.Trim();

        if (trimmedDescription.Length > DescriptionMaxLength)
        {
            throw new ArgumentException($"Todo item description cannot exceed {DescriptionMaxLength} characters.", nameof(description));
        }

        Description = trimmedDescription;
    }

    /// <summary>
    /// Validates and sets the todo item notes.
    /// Notes are optional and can be cleared by passing null.
    /// </summary>
    /// <param name="notes">The notes to validate and set. Can be null to clear the notes.</param>
    /// <exception cref="ArgumentException">Thrown when notes exceed maximum length.</exception>
    private void ValidateAndSetNotes(string? notes)
    {
        // Allow null to clear notes
        if (string.IsNullOrWhiteSpace(notes))
        {
            Notes = null;
            return;
        }

        string trimmedNotes = notes.Trim();

        if (trimmedNotes.Length > NotesMaxLength)
        {
            throw new ArgumentException($"Todo item notes cannot exceed {NotesMaxLength} characters.", nameof(notes));
        }

        Notes = trimmedNotes;
    }
}


