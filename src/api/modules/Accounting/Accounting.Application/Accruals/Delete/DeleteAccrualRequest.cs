namespace Accounting.Application.Accruals.Delete;

public class DeleteAccrualRequest : IRequest<Unit>
{
    public DefaultIdType Id { get; set; }
}

