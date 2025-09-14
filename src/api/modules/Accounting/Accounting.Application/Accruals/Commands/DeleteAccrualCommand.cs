namespace Accounting.Application.Accruals.Commands;

public class DeleteAccrualCommand : IRequest
{
    public DefaultIdType Id { get; set; }
}

