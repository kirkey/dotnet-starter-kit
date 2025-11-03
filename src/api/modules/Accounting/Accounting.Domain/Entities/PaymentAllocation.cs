namespace Accounting.Domain.Entities;

/// <summary>
/// Represents an allocation of a payment amount to a specific invoice for accounts receivable management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Split a single payment across multiple outstanding invoices for the same customer.
/// - Apply partial payments to specific invoices based on customer preferences or aging.
/// - Track payment application for accounts receivable aging and collection reporting.
/// - Support payment reconciliation and dispute resolution with detailed allocation records.
/// - Enable automatic payment allocation based on business rules (oldest first, by invoice number).
/// - Handle overpayments by allocating exact amounts and tracking remaining credits.
/// - Support payment reversals and reallocation for billing adjustments.
/// 
/// Default values:
/// - PaymentId: required reference to parent payment entity
/// - InvoiceId: required reference to target invoice being paid
/// - Amount: required positive decimal amount (example: 150.00 for partial payment)
/// - Notes: optional allocation notes (example: "Applied to oldest outstanding balance")
/// - AllocationDate: system timestamp when allocation is created
/// 
/// Business rules:
/// - Amount must be positive (cannot allocate negative amounts)
/// - Total allocations for a payment cannot exceed the payment amount
/// - Cannot allocate to an invoice that is already fully paid
/// - Allocation creates a credit on the target invoice
/// - Cannot delete allocation after invoice is marked as paid
/// - Must maintain referential integrity with payment and invoice entities
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Payment.PaymentAllocated"/>
/// <seealso cref="Accounting.Domain.Events.Payment.PaymentAllocationReversed"/>
/// <seealso cref="Accounting.Domain.Events.Invoice.InvoicePaymentApplied"/>
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

    /// <summary>
    /// Update allocation amount and/or notes. Amount must remain positive.
    /// </summary>
    /// <param name="amount">Updated allocation amount (optional, must be positive)</param>
    /// <param name="notes">Updated notes (optional)</param>
    /// <returns>This instance for fluent chaining</returns>
    /// <exception cref="ArgumentException">Thrown if validation fails</exception>
    public PaymentAllocation Update(decimal? amount, string? notes)
    {
        bool isUpdated = false;

        if (amount.HasValue && Amount != amount.Value)
        {
            if (amount.Value <= 0)
                throw new ArgumentException("Allocation amount must be positive");
            Amount = amount.Value;
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes;
            isUpdated = true;
        }

        return this;
    }
}
