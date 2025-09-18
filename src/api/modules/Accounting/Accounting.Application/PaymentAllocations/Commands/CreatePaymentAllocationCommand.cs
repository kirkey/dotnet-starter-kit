namespace Accounting.Application.PaymentAllocations.Commands
{
    public class CreatePaymentAllocationCommand : IRequest<DefaultIdType>
    {
        public DefaultIdType PaymentId { get; set; }
        public DefaultIdType InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}

