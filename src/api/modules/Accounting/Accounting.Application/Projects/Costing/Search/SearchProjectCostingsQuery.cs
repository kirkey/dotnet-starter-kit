using Accounting.Application.Projects.Costing.Responses;

namespace Accounting.Application.Projects.Costing.Search;

public sealed class SearchProjectCostingsQuery : PaginationFilter, IRequest<PagedList<ProjectCostingResponse>>
{
    public DefaultIdType? ProjectId { get; set; }
    public DefaultIdType? AccountId { get; set; }
    public string? Category { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? IsBillable { get; set; }
    public bool? IsApproved { get; set; }
}
