namespace Accounting.Application.Projects.Costing.Specs;

/// <summary>
/// Specification to get a project costing entry by project ID and account ID.
/// </summary>
public sealed class ProjectCostingByProjectAndAccountSpec : Specification<ProjectCostEntry>
{
    public ProjectCostingByProjectAndAccountSpec(DefaultIdType projectId, DefaultIdType accountId)
    {
        Query.Where(x => x.ProjectId == projectId && x.AccountId == accountId);
    }
}
