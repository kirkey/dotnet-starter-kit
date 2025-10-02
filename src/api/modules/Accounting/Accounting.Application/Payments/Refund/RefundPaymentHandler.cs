using Accounting.Domain.Entities;

namespace Accounting.Application.Payments.Refund;

/// <summary>
/// Handler for refunding a payment.
/// </summary>
public sealed class RefundPaymentHandler(
    ILogger<RefundPaymentHandler> logger,
    [FromKeyedServices("accounting:payments")] IRepository<Payment> repository)
    : IRequestHandler<RefundPaymentCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payment = await repository.GetByIdAsync(request.PaymentId, cancellationToken);
        
        if (payment == null)
        {
            throw new NotFoundException($"Payment with id {request.PaymentId} not found");
        }

        if (request.RefundAmount <= 0)
        {
            throw new ArgumentException("Refund amount must be positive.");
        }

        payment.Refund(request.RefundAmount, DateTime.UtcNow, request.RefundReference);

        await repository.UpdateAsync(payment, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Payment {PaymentId} refunded {Amount}. Reference: {Reference}", 
            request.PaymentId, request.RefundAmount, request.RefundReference ?? "Not specified");

        return payment.Id;
    }
}
