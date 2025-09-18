namespace Accounting.Application.Consumptions.Commands;

public class DeleteConsumptionDataCommand : IRequest
{
    public DefaultIdType Id { get; set; }
}

