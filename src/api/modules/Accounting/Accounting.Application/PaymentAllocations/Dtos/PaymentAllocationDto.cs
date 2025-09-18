namespace Accounting.Application.PaymentAllocations.Dtos;

public class PaymentAllocationDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType PaymentId { get; set; }
    public DefaultIdType InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}

