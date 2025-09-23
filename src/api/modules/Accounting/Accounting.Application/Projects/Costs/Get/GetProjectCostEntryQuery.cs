using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Costs.Get;

/// <summary>
/// Query to fetch a single cost entry from a project by entry id.
/// </summary>
public sealed class GetProjectCostEntryQuery(DefaultIdType projectId, DefaultIdType entryId)
    : IRequest<ProjectCostEntryResponse>
{
    public DefaultIdType ProjectId { get; init; } = projectId;
    public DefaultIdType EntryId { get; init; } = entryId;
}
