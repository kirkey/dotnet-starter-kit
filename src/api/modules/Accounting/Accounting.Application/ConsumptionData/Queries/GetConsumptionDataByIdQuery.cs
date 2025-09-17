using Accounting.Application.ConsumptionData.Dtos;

namespace Accounting.Application.ConsumptionData.Queries;

public class GetConsumptionDataByIdQuery(DefaultIdType id) : IRequest<ConsumptionDataDto>
{
    public DefaultIdType Id { get; set; } = id;
}

