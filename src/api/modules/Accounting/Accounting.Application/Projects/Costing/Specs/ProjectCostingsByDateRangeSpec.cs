using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Costing.Specs;

/// <summary>
/// Specification to get project costing entries by date range.
/// </summary>
public sealed class ProjectCostingsByDateRangeSpec : Specification<ProjectCostEntry>
{
    public ProjectCostingsByDateRangeSpec(DateTime fromDate, DateTime toDate)
    {
        Query.Where(x => x.EntryDate >= fromDate && x.EntryDate <= toDate);
    }
}
