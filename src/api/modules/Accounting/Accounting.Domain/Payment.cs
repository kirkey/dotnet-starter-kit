using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Accounting.Domain.Events.Payment;

namespace Accounting.Domain;

public class Payment : AuditableEntity, IAggregateRoot
{
    public string PaymentNumber { get; private set; }
    public DefaultIdType? MemberId { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public decimal Amount { get; private set; }
    public decimal UnappliedAmount { get; private set; }
    public string PaymentMethod { get; private set; } // Cash, Check, EFT, CreditCard
    public string? ReferenceNumber { get; private set; }
    public string? DepositToAccountCode { get; private set; } // Bank account or cash account

    private readonly List<PaymentAllocation> _allocations = new();
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

    public static Payment Create(string paymentNumber, DefaultIdType? memberId, DateTime paymentDate, decimal amount, string paymentMethod, string? referenceNumber = null, string? depositToAccountCode = null, string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(paymentNumber)) throw new ArgumentException("Payment number is required.");
        if (amount <= 0) throw new ArgumentException("Payment amount must be positive.");
        if (string.IsNullOrWhiteSpace(paymentMethod)) throw new ArgumentException("Payment method is required.");

        return new Payment(paymentNumber, memberId, paymentDate, amount, paymentMethod, referenceNumber, depositToAccountCode, description, notes);
    }

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

    public Payment Refund(decimal amount, DateTime refundedDate, string? refundReference = null)
    {
        if (amount <= 0) throw new ArgumentException("Refund amount must be positive.");
        if (amount > (Amount - _allocations.Sum(a => a.Amount))) throw new InvalidOperationException("Cannot refund more than unallocated/available amount.");

        // For simplicity mark as negative unapplied and queue event
        UnappliedAmount -= amount;
        QueueDomainEvent(new Events.Payment.PaymentReferenceUpdated(Id, PaymentNumber, refundReference));
        return this;
    }

    public Payment UpdateReference(string? referenceNumber)
    {
        ReferenceNumber = referenceNumber?.Trim();
        QueueDomainEvent(new PaymentReferenceUpdated(Id, PaymentNumber, ReferenceNumber));
        return this;
    }
}
