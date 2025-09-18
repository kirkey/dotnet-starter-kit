namespace Accounting.Application.PaymentAllocations.Commands;

public class UpdatePaymentAllocationCommand : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public decimal? Amount { get; set; }
    public string? Notes { get; set; }
}
