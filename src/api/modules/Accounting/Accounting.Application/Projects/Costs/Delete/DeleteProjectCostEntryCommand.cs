namespace Accounting.Application.Projects.Costs.Delete;

/// <summary>
/// Command to delete a project cost entry by id.
/// </summary>
public sealed class DeleteProjectCostEntryCommand(
    DefaultIdType projectId,
    DefaultIdType entryId
) : IRequest
{
    public DefaultIdType ProjectId { get; init; } = projectId;
    public DefaultIdType EntryId { get; init; } = entryId;
}
