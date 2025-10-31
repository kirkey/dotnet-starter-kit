using Accounting.Application.Payments.Commands;

namespace Accounting.Application.Payments.Handlers;

public sealed class AllocatePaymentHandler(
    IRepository<Payment> paymentRepository,
    IRepository<Invoice> invoiceRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<AllocatePaymentCommand>
{
    public async Task Handle(AllocatePaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payment = await paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);
        _ = payment ?? throw new PaymentByIdNotFoundException(request.PaymentId);

        decimal totalAllocated = 0m;
        foreach (var item in request.Allocations)
        {
            var invoice = await invoiceRepository.GetByIdAsync(item.InvoiceId, cancellationToken);
            _ = invoice ?? throw new InvoiceByIdNotFoundException(item.InvoiceId);

            var outstanding = invoice.GetOutstandingAmount();
            if (item.Amount > outstanding) throw new InvalidOperationException($"Allocation amount {item.Amount} exceeds outstanding {outstanding} for invoice {item.InvoiceId}");

            // Apply allocation to Payment domain (tracks allocations)
            payment.AllocateToInvoice(item.InvoiceId, item.Amount);

            // Apply payment to invoice
            invoice.ApplyPayment(item.Amount, DateTime.UtcNow, payment.PaymentMethod);

            await invoiceRepository.UpdateAsync(invoice, cancellationToken);

            totalAllocated += item.Amount;
        }

        if (totalAllocated > 0)
        {
            await paymentRepository.UpdateAsync(payment, cancellationToken);

            // Adjust member balance if present
            if (payment.MemberId.HasValue && payment.MemberId != DefaultIdType.Empty)
            {
                var member = await memberRepository.GetByIdAsync(payment.MemberId.Value, cancellationToken);
                if (member != null)
                {
                    var newBalance = member.CurrentBalance - totalAllocated;
                    member.UpdateBalance(newBalance);
                    await memberRepository.UpdateAsync(member, cancellationToken);
                }
            }

            await paymentRepository.SaveChangesAsync(cancellationToken);
        }
    }
}

