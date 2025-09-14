namespace Accounting.Domain;

public class PaymentAllocation : BaseEntity
{
    public DefaultIdType PaymentId { get; private set; }
    public DefaultIdType InvoiceId { get; private set; }
    public decimal Amount { get; private set; }

    private PaymentAllocation() { }

    private PaymentAllocation(DefaultIdType paymentId, DefaultIdType invoiceId, decimal amount)
    {
        PaymentId = paymentId;
        InvoiceId = invoiceId;
        Amount = amount;
    }

    public static PaymentAllocation Create(DefaultIdType paymentId, DefaultIdType invoiceId, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Allocation amount must be positive.");
        return new PaymentAllocation(paymentId, invoiceId, amount);
    }
}

