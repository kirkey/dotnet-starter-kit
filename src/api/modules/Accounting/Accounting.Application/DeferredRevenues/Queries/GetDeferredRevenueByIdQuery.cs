using Accounting.Application.DeferredRevenues.Responses;

namespace Accounting.Application.DeferredRevenues.Queries;

public class GetDeferredRevenueByIdQuery(DefaultIdType id) : IRequest<DeferredRevenueResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
