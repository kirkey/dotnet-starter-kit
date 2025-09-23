using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Costs.Get;

/// <summary>
/// Query to list all cost entries for a given project.
/// </summary>
public sealed class GetProjectCostEntriesQuery(DefaultIdType projectId)
    : IRequest<ICollection<ProjectCostEntryResponse>>
{
    public DefaultIdType ProjectId { get; init; } = projectId;
}
