
using Accounting.Application.DeferredRevenues.Responses;

namespace Accounting.Application.DeferredRevenues.Queries;

public class SearchDeferredRevenuesQuery : IRequest<List<DeferredRevenueResponse>>
{
    public string? DeferredRevenueNumber { get; set; }
    public string? Description { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
