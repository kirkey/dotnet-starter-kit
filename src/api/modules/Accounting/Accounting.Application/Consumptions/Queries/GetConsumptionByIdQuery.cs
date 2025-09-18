using Accounting.Application.Consumptions.Dtos;

namespace Accounting.Application.Consumptions.Queries;

public class GetConsumptionByIdQuery(DefaultIdType id) : IRequest<ConsumptionDto>
{
    public DefaultIdType Id { get; set; } = id;
}

