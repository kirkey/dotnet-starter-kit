namespace Accounting.Application.Consumptions.Commands;

public class DeleteConsumptionCommand : IRequest
{
    public DefaultIdType Id { get; set; }
}

