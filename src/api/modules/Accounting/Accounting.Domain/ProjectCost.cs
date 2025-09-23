namespace Accounting.Domain;

/// <summary>
/// Represents a single project cost entry (positive amount) or revenue (negative amount) linked to a Project.
/// </summary>
public class ProjectCostEntry : BaseEntity
{
    /// <summary>
    /// Parent project identifier.
    /// </summary>
    public DefaultIdType ProjectId { get; private set; }

    /// <summary>
    /// Date of the cost/revenue entry.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Description of the entry.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Amount (positive for cost, negative for revenue).
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Related GL account identifier.
    /// </summary>
    public DefaultIdType AccountId { get; private set; }

    /// <summary>
    /// Optional link to the journal entry that recorded this transaction.
    /// </summary>
    public DefaultIdType? JournalEntryId { get; private set; }

    /// <summary>
    /// Optional category text (e.g., Revenue when created via Project.AddRevenueEntry).
    /// </summary>
    public string? Category { get; private set; }

    private ProjectCostEntry(DefaultIdType projectId, DateTime date, string description,
        decimal amount, DefaultIdType accountId, DefaultIdType? journalEntryId = null, string? category = null)
    {
        ProjectId = projectId;
        Date = date;
        Description = description.Trim();
        Amount = amount;
        AccountId = accountId;
        JournalEntryId = journalEntryId;
        Category = category?.Trim();
    }

    /// <summary>
    /// Create a project cost entry.
    /// </summary>
    public static ProjectCostEntry Create(DefaultIdType projectId, DateTime date, string description,
        decimal amount, DefaultIdType accountId, DefaultIdType? journalEntryId = null, string? category = null)
    {
        return new ProjectCostEntry(projectId, date, description, amount, accountId, journalEntryId, category);
    }

    /// <summary>
    /// Update fields of the project cost entry.
    /// </summary>
    public ProjectCostEntry Update(DateTime? date, string? description, decimal? amount, string? category)
    {
        if (date.HasValue && Date != date.Value)
        {
            Date = date.Value;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            Description = description.Trim();
        }

        if (amount.HasValue && Amount != amount.Value)
        {
            Amount = amount.Value;
        }

        if (category != Category)
        {
            Category = category?.Trim();
        }

        return this;
    }
}
