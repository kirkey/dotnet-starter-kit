using Accounting.Application.Payments.Exceptions;

namespace Accounting.Application.Payments.Create.v1;

/// <summary>
/// Handler for creating a new payment.
/// Validates uniqueness of payment number and creates the payment record.
/// </summary>
public sealed class PaymentCreateHandler(
    [FromKeyedServices("accounting:payments")] IRepository<Payment> repository,
    ILogger<PaymentCreateHandler> logger)
    : IRequestHandler<PaymentCreateCommand, PaymentCreateResponse>
{
    /// <summary>
    /// Handles the payment creation command.
    /// </summary>
    public async Task<PaymentCreateResponse> Handle(PaymentCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Creating payment {PaymentNumber} for quantity {Amount}", 
            request.PaymentNumber, request.Amount);

        // Check if payment number already exists
        var existingPayments = await repository.ListAsync(cancellationToken);
        if (existingPayments.Any(p => p.PaymentNumber.Equals(request.PaymentNumber, StringComparison.OrdinalIgnoreCase)))
        {
            logger.LogWarning("Payment number {PaymentNumber} already exists", request.PaymentNumber);
            throw new PaymentNumberAlreadyExistsException(request.PaymentNumber);
        }

        // Create the payment entity
        var payment = Payment.Create(
            paymentNumber: request.PaymentNumber,
            memberId: request.MemberId,
            paymentDate: request.PaymentDate,
            amount: request.Amount,
            paymentMethod: request.PaymentMethod,
            referenceNumber: request.ReferenceNumber,
            depositToAccountCode: request.DepositToAccountCode,
            description: request.Description,
            notes: request.Notes
        );

        // Persist the payment
        await repository.AddAsync(payment, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Payment {PaymentNumber} created successfully with ID {PaymentId}", 
            payment.PaymentNumber, payment.Id);

        // Return response
        return new PaymentCreateResponse
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            Amount = payment.Amount,
            UnappliedAmount = payment.UnappliedAmount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod
        };
    }
}

