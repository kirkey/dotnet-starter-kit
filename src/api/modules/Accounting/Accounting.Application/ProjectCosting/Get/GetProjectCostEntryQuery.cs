namespace Accounting.Application.ProjectCosting.Get;

/// <summary>
/// Query to fetch a single cost entry from a project by entry id.
/// </summary>
public sealed class GetProjectCostEntryQuery(DefaultIdType projectId, DefaultIdType entryId)
    : IRequest<ProjectCostResponse>
{
    public DefaultIdType ProjectId { get; init; } = projectId;
    public DefaultIdType EntryId { get; init; } = entryId;
}
