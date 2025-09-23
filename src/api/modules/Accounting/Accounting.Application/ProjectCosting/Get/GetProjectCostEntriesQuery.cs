namespace Accounting.Application.ProjectCosting.Get;

/// <summary>
/// Query to list all cost entries for a given project.
/// </summary>
public sealed class GetProjectCostEntriesQuery(DefaultIdType projectId)
    : IRequest<ICollection<ProjectCostResponse>>
{
    public DefaultIdType ProjectId { get; init; } = projectId;
}
