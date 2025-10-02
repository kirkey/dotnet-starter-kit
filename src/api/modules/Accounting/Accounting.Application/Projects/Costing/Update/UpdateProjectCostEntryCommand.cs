namespace Accounting.Application.Projects.Costing.Update;

/// <summary>
/// Command to update fields of an existing project cost entry.
/// Returns the identifier of the updated entry.
/// </summary>
public sealed class UpdateProjectCostEntryCommand(
    DefaultIdType projectId,
    DefaultIdType entryId,
    DateTime? date,
    string? description,
    decimal? amount,
    string? category
) : IRequest<DefaultIdType>
{
    public DefaultIdType ProjectId { get; init; } = projectId;
    public DefaultIdType EntryId { get; init; } = entryId;
    public DateTime? Date { get; init; } = date;
    public string? Description { get; init; } = description;
    public decimal? Amount { get; init; } = amount;
    public string? Category { get; init; } = category;
}
