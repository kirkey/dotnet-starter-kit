namespace Accounting.Application.Projects.Specifications;

/// <summary>
/// Specification to load a project with all its associated cost entries for comprehensive reporting.
/// </summary>
public class ProjectWithCostEntriesSpec : Specification<Project>
{
    public ProjectWithCostEntriesSpec(DefaultIdType projectId) => 
        Query.Where(p => p.Id == projectId)
             .Include(p => p.CostingEntries);
}
