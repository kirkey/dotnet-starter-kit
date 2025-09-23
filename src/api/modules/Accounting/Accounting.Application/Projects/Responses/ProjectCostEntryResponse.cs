namespace Accounting.Application.Projects.Responses;

/// <summary>
/// Response model representing a single project cost entry.
/// </summary>
public sealed class ProjectCostEntryResponse(
    DefaultIdType projectId,
    DateTime date,
    string description,
    decimal amount,
    DefaultIdType accountId,
    DefaultIdType? journalEntryId,
    string? category
) : BaseDto
{
    /// <summary>
    /// The project this cost entry belongs to.
    /// </summary>
    public DefaultIdType ProjectId { get; init; } = projectId;

    /// <summary>
    /// The date of the cost entry.
    /// </summary>
    public DateTime Date { get; init; } = date;

    /// <summary>
    /// The description of the cost entry.
    /// </summary>
    public new string Description { get; init; } = description;

    /// <summary>
    /// The amount of the cost entry (positive for costs).
    /// </summary>
    public decimal Amount { get; init; } = amount;

    /// <summary>
    /// The GL account associated with the entry.
    /// </summary>
    public DefaultIdType AccountId { get; init; } = accountId;

    /// <summary>
    /// Optional associated journal entry id.
    /// </summary>
    public DefaultIdType? JournalEntryId { get; init; } = journalEntryId;

    /// <summary>
    /// Optional category of the cost entry.
    /// </summary>
    public string? Category { get; init; } = category;

    /// <summary>
    /// Alternate constructor that allows setting the identifier.
    /// </summary>
    public ProjectCostEntryResponse(
        DefaultIdType id,
        DefaultIdType projectId,
        DateTime date,
        string description,
        decimal amount,
        DefaultIdType accountId,
        DefaultIdType? journalEntryId,
        string? category) : this(projectId, date, description, amount, accountId, journalEntryId, category)
    {
        Id = id;
    }
}
