using Accounting.Application.Projects.Costing.Responses;

namespace Accounting.Application.Projects.Costing.Search;

public sealed class SearchProjectCostingsSpec : EntitiesByPaginationFilterSpec<ProjectCostEntry, ProjectCostingResponse>
{
    public SearchProjectCostingsSpec(SearchProjectCostingsQuery request) : base(request)
    {
        Query
            .OrderBy(c => c.EntryDate, !request.HasOrderBy())
            .Where(c => c.ProjectId == request.ProjectId, request.ProjectId is not null)
            .Where(c => c.AccountId == request.AccountId, request.AccountId is not null)
            .Where(c => c.Category!.Contains(request.Category!), !string.IsNullOrEmpty(request.Category))
            .Where(c => c.EntryDate >= request.FromDate, request.FromDate.HasValue)
            .Where(c => c.EntryDate <= request.ToDate, request.ToDate.HasValue)
            .Where(c => c.IsBillable == request.IsBillable, request.IsBillable.HasValue)
            .Where(c => c.IsApproved == request.IsApproved, request.IsApproved.HasValue);
    }
}
