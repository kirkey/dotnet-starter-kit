using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Queries;

public class GetConsumptionByIdQuery(DefaultIdType id) : IRequest<ConsumptionResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
