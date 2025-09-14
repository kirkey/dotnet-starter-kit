namespace Accounting.Application.ConsumptionData.Commands;

public class DeleteConsumptionDataCommand : IRequest
{
    public DefaultIdType Id { get; set; }
}

