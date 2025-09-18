using Accounting.Application.Consumptions.Dtos;

namespace Accounting.Application.Consumptions.Queries;

public class GetConsumptionDataByIdQuery(DefaultIdType id) : IRequest<ConsumptionDataDto>
{
    public DefaultIdType Id { get; set; } = id;
}

