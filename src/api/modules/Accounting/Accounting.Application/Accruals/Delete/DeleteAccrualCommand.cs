namespace Accounting.Application.Accruals.Delete;

public class DeleteAccrualCommand : IRequest
{
    public DefaultIdType Id { get; set; }
}

