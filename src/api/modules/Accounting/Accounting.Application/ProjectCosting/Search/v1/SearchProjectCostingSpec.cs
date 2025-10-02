namespace Accounting.Application.ProjectCosting.Search.v1;

/// <summary>
/// Specification for searching project costing entries with filters.
/// </summary>
public sealed class SearchProjectCostingSpec : Specification<ProjectCostEntry>
{
    public SearchProjectCostingSpec(SearchProjectCostingQuery query)
    {
        Query
            .Where(p => p.ProjectId == query.ProjectId, query.ProjectId.HasValue)
            .Where(p => p.EntryDate >= query.StartDate, query.StartDate.HasValue)
            .Where(p => p.EntryDate <= query.EndDate, query.EndDate.HasValue)
            .Where(p => p.Category == query.Category, !string.IsNullOrEmpty(query.Category))
            .Where(p => p.CostCenter == query.CostCenter, !string.IsNullOrEmpty(query.CostCenter))
            .Where(p => p.IsBillable == query.IsBillable, query.IsBillable.HasValue)
            .Where(p => p.IsApproved == query.IsApproved, query.IsApproved.HasValue)
            .OrderByDescending(p => p.EntryDate)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize);
    }
}
