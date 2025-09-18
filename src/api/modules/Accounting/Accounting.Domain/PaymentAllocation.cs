

namespace Accounting.Domain;

/// <summary>
/// Represents an allocation of a payment to a specific invoice.
/// </summary>
/// <remarks>
/// Used to split a single payment across multiple invoices. Amount must be positive.
/// </remarks>
public class PaymentAllocation : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Parent payment identifier.
    /// </summary>
    public DefaultIdType PaymentId { get; private set; }

    /// <summary>
    /// Target invoice identifier that this allocation is applied to.
    /// </summary>
    public DefaultIdType InvoiceId { get; private set; }

    /// <summary>
    /// Allocation amount applied to the invoice; must be positive.
    /// </summary>
    public decimal Amount { get; private set; }

    private PaymentAllocation() { }

    private PaymentAllocation(DefaultIdType paymentId, DefaultIdType invoiceId, decimal amount, string? notes)
    {
        PaymentId = paymentId;
        InvoiceId = invoiceId;
        Amount = amount;
        Notes = notes;
    }

    /// <summary>
    /// Factory to create a payment allocation; validates positive amount.
    /// </summary>
    public static PaymentAllocation Create(DefaultIdType paymentId, DefaultIdType invoiceId, decimal amount, string? notes = null)
    {
        if (amount <= 0) throw new ArgumentException("Allocation amount must be positive.");
        return new PaymentAllocation(paymentId, invoiceId, amount, notes);
    }
}