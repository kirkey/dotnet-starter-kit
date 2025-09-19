namespace Accounting.Application.Accruals.Update;

public class UpdateAccrualCommand : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string? AccrualNumber { get; set; }
    public DateTime? AccrualDate { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
}

