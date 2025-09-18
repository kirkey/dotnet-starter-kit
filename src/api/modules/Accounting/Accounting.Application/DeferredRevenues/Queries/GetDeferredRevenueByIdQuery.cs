using Accounting.Application.DeferredRevenues.Dtos;

namespace Accounting.Application.DeferredRevenues.Queries;

public class GetDeferredRevenueByIdQuery(DefaultIdType id) : IRequest<DeferredRevenueDto>
{
    public DefaultIdType Id { get; set; } = id;
}