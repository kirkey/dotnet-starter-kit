using Accounting.Application.Projects.Costing.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Costing.Search;

public sealed class SearchProjectCostingsSpec : EntitiesByPaginationFilterSpec<ProjectCostEntry, ProjectCostingResponse>
{
    public SearchProjectCostingsSpec(SearchProjectCostingsQuery request) : base(request)
    {
        Query
            .OrderBy(c => c.EntryDate, !request.HasOrderBy())
            .Where(c => c.ProjectId == request.ProjectId, request.ProjectId.HasValue && request.ProjectId != default)
            .Where(c => c.AccountId == request.AccountId, request.AccountId.HasValue && request.AccountId != default)
            .Where(c => c.Category!.Contains(request.Category!), !string.IsNullOrEmpty(request.Category))
            .Where(c => c.EntryDate >= request.FromDate, request.FromDate.HasValue)
            .Where(c => c.EntryDate <= request.ToDate, request.ToDate.HasValue)
            .Where(c => c.IsBillable == request.IsBillable, request.IsBillable.HasValue)
            .Where(c => c.IsApproved == request.IsApproved, request.IsApproved.HasValue);
    }
}
