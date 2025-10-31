namespace Accounting.Application.Payments.Void;

/// <summary>
/// Handler for voiding a payment.
/// </summary>
public sealed class VoidPaymentHandler(
    ILogger<VoidPaymentHandler> logger,
    [FromKeyedServices("accounting:payments")] IRepository<Payment> repository)
    : IRequestHandler<VoidPaymentCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(VoidPaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payment = await repository.GetByIdAsync(request.PaymentId, cancellationToken);
        
        if (payment == null)
        {
            throw new NotFoundException($"Payment with id {request.PaymentId} not found");
        }

        payment.Void(request.VoidReason);

        await repository.UpdateAsync(payment, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Payment {PaymentId} voided. Reason: {Reason}", 
            request.PaymentId, request.VoidReason ?? "Not specified");

        return payment.Id;
    }
}
