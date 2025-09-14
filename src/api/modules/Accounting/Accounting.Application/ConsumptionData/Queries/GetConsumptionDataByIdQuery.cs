using Accounting.Application.ConsumptionData.Dtos;

namespace Accounting.Application.ConsumptionData.Queries;

public class GetConsumptionDataByIdQuery : IRequest<ConsumptionDataDto>
{
    public DefaultIdType Id { get; set; }

    public GetConsumptionDataByIdQuery(DefaultIdType id) { Id = id; }
}

