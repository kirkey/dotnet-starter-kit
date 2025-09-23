namespace Accounting.Application.Projects.Queries;

/// <summary>
/// Specification to retrieve a project by id including its job costing entries.
/// </summary>
public sealed class ProjectWithCostEntriesByIdSpec : Specification<Project>, ISingleResultSpecification<Project>
{
    public ProjectWithCostEntriesByIdSpec(DefaultIdType id)
    {
        Query.Where(p => p.Id == id)
             .Include(p => p.CostingEntries);
    }
}

