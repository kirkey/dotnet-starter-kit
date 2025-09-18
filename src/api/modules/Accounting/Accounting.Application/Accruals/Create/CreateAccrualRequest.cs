namespace Accounting.Application.Accruals.Create;

public class CreateAccrualRequest : IRequest<DefaultIdType>
{
    public string AccrualNumber { get; set; } = default!;
    public DateTime AccrualDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = default!;
}

