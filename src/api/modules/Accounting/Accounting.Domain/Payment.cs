using Accounting.Domain.Events.Payment;

namespace Accounting.Domain;

/// <summary>
/// Represents a customer/member payment, including method and allocations to invoices.
/// </summary>
/// <remarks>
/// Tracks unapplied balance for allocation to one or more invoices. Defaults: <see cref="UnappliedAmount"/>
/// initialized to <see cref="Amount"/> on creation; string properties trimmed.
/// </remarks>
public class Payment : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique payment number (e.g., receipt number).
    /// </summary>
    public string PaymentNumber { get; private set; }

    /// <summary>
    /// Optional member identifier if the payment is associated with a specific member.
    /// </summary>
    public DefaultIdType? MemberId { get; private set; }

    /// <summary>
    /// Date the payment was received.
    /// </summary>
    public DateTime PaymentDate { get; private set; }

    /// <summary>
    /// Total payment amount received; must be positive.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Portion of the payment not yet allocated to invoices.
    /// </summary>
    public decimal UnappliedAmount { get; private set; }

    /// <summary>
    /// Payment method such as Cash, Check, EFT, CreditCard.
    /// </summary>
    public string PaymentMethod { get; private set; } // Cash, Check, EFT, CreditCard

    /// <summary>
    /// Optional check/reference number.
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Optional deposit account code (e.g., bank or cash account) where the payment is deposited.
    /// </summary>
    public string? DepositToAccountCode { get; private set; } // Bank account or cash account

    private readonly List<PaymentAllocation> _allocations = new();
    /// <summary>
    /// Allocations of this payment to one or more invoices.
    /// </summary>
    public IReadOnlyCollection<PaymentAllocation> Allocations => _allocations.AsReadOnly();

    private Payment() { PaymentNumber = string.Empty; PaymentMethod = string.Empty; }

    private Payment(string paymentNumber, DefaultIdType? memberId, DateTime paymentDate, decimal amount, string paymentMethod, string? referenceNumber = null, string? depositToAccountCode = null, string? description = null, string? notes = null)
    {
        PaymentNumber = paymentNumber.Trim();
        MemberId = memberId;
        PaymentDate = paymentDate;
        Amount = amount;
        UnappliedAmount = amount;
        PaymentMethod = paymentMethod.Trim();
        ReferenceNumber = referenceNumber?.Trim();
        DepositToAccountCode = depositToAccountCode?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new PaymentReceived(Id, PaymentNumber, MemberId, null, Amount, PaymentDate, PaymentMethod));
    }

    /// <summary>
    /// Create a payment with validation of required fields and positive amount.
    /// </summary>
    public static Payment Create(string paymentNumber, DefaultIdType? memberId, DateTime paymentDate, decimal amount, string paymentMethod, string? referenceNumber = null, string? depositToAccountCode = null, string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(paymentNumber)) throw new ArgumentException("Payment number is required.");
        if (amount <= 0) throw new ArgumentException("Payment amount must be positive.");
        if (string.IsNullOrWhiteSpace(paymentMethod)) throw new ArgumentException("Payment method is required.");

        return new Payment(paymentNumber, memberId, paymentDate, amount, paymentMethod, referenceNumber, depositToAccountCode, description, notes);
    }

    /// <summary>
    /// Allocate part of this payment to an invoice; decrements <see cref="UnappliedAmount"/>.
    /// </summary>
    public Payment AllocateToInvoice(DefaultIdType invoiceId, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Allocation amount must be positive.");
        if (amount > UnappliedAmount) throw new InvalidOperationException("Allocation exceeds unapplied payment amount.");

        var allocation = PaymentAllocation.Create(Id, invoiceId, amount);
        _allocations.Add(allocation);
        UnappliedAmount -= amount;

        QueueDomainEvent(new PaymentAppliedToInvoice(Id, PaymentNumber, invoiceId, amount));
        return this;
    }

    /// <summary>
    /// Issue a refund from unallocated/available funds; decreases <see cref="UnappliedAmount"/>.
    /// </summary>
    public Payment Refund(decimal amount, DateTime refundedDate, string? refundReference = null)
    {
        if (amount <= 0) throw new ArgumentException("Refund amount must be positive.");
        if (amount > (Amount - _allocations.Sum(a => a.Amount))) throw new InvalidOperationException("Cannot refund more than unallocated/available amount.");

        // For simplicity mark as negative unapplied and queue event
        UnappliedAmount -= amount;
        QueueDomainEvent(new PaymentReferenceUpdated(Id, PaymentNumber, refundReference));
        return this;
    }

    /// <summary>
    /// Update the external reference number (e.g., check no.).
    /// </summary>
    public Payment UpdateReference(string? referenceNumber)
    {
        ReferenceNumber = referenceNumber?.Trim();
        QueueDomainEvent(new PaymentReferenceUpdated(Id, PaymentNumber, ReferenceNumber));
        return this;
    }
}
