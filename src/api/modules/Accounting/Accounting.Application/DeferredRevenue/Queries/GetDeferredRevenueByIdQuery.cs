using Accounting.Application.DeferredRevenue.Dtos;

namespace Accounting.Application.DeferredRevenue.Queries
{
    public class GetDeferredRevenueByIdQuery(DefaultIdType id) : IRequest<DeferredRevenueDto>
    {
        public DefaultIdType Id { get; set; } = id;
    }
}

