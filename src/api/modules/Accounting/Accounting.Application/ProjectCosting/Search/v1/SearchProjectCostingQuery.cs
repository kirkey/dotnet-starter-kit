namespace Accounting.Application.ProjectCosting.Search.v1;

/// <summary>
/// Query to search project costing entries with filters and pagination.
/// </summary>
public sealed record SearchProjectCostingQuery : PaginationFilter, IRequest<PagedList<ProjectCostingResponse>>
{
    public DefaultIdType? ProjectId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? Category { get; init; }
    public string? CostCenter { get; init; }
    public bool? IsBillable { get; init; }
    public bool? IsApproved { get; init; }
}
