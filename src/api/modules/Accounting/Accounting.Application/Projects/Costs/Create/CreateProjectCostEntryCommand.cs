namespace Accounting.Application.Projects.Costs.Create;

/// <summary>
/// Command to add a new cost entry to a project.
/// </summary>
public sealed class CreateProjectCostEntryCommand(
    DefaultIdType projectId,
    DateTime date,
    string description,
    decimal amount,
    DefaultIdType accountId,
    DefaultIdType? journalEntryId = null,
    string? category = null
) : IRequest<DefaultIdType>
{
    public DefaultIdType ProjectId { get; init; } = projectId;
    public DateTime Date { get; init; } = date;
    public string Description { get; init; } = description;
    public decimal Amount { get; init; } = amount;
    public DefaultIdType AccountId { get; init; } = accountId;
    public DefaultIdType? JournalEntryId { get; init; } = journalEntryId;
    public string? Category { get; init; } = category;
}
