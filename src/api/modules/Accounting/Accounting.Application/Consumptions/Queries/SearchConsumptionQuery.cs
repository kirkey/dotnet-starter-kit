using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Queries;

public class SearchConsumptionQuery : PaginationFilter, IRequest<PagedList<ConsumptionResponse>>
{
    public DefaultIdType? MeterId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
