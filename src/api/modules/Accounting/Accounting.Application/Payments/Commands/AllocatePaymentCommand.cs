namespace Accounting.Application.Payments.Commands;

public record PaymentAllocationItem(DefaultIdType InvoiceId, decimal Amount);

public sealed class AllocatePaymentCommand : IRequest
{
    public DefaultIdType PaymentId { get; init; }
    public List<PaymentAllocationItem> Allocations { get; init; } = new();
}

