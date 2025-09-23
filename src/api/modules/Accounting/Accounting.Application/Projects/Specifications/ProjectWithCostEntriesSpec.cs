using Ardalis.Specification;

namespace Accounting.Application.Projects.Specifications;

/// <summary>
/// Specification to retrieve a project by ID including its cost entries.
/// </summary>
public sealed class ProjectWithCostEntriesSpec : Specification<Project>
{
    public ProjectWithCostEntriesSpec(DefaultIdType projectId)
    {
        Query.Where(p => p.Id == projectId)
             .Include(p => p.CostingEntries);
    }
}

