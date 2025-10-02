using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Costing.Specs;

/// <summary>
/// Specification to get all project costing entries for a specific project.
/// </summary>
public sealed class ProjectCostingsByProjectIdSpec : Specification<ProjectCostEntry>
{
    public ProjectCostingsByProjectIdSpec(DefaultIdType projectId)
    {
        Query.Where(x => x.ProjectId == projectId);
    }
}
