using Accounting.Application.Payments.Exceptions;

namespace Accounting.Application.Payments.Update.v1;

/// <summary>
/// Handler for updating payment information.
/// </summary>
public sealed class UpdatePaymentHandler : IRequestHandler<UpdatePaymentCommand, PaymentUpdateResponse>
{
    private readonly IRepository<Payment> _repository;
    private readonly ILogger<UpdatePaymentHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePaymentHandler"/> class.
    /// </summary>
    public UpdatePaymentHandler(
        IRepository<Payment> repository,
        ILogger<UpdatePaymentHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the payment update command.
    /// </summary>
    public async Task<PaymentUpdateResponse> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Updating payment with ID {PaymentId}", request.Id);

        var payment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (payment == null)
        {
            _logger.LogWarning("Payment with ID {PaymentId} not found", request.Id);
            throw new PaymentNotFoundException(request.Id);
        }

        // Update only the modifiable fields
        payment.Update(
            referenceNumber: request.ReferenceNumber,
            depositToAccountCode: request.DepositToAccountCode,
            description: request.Description,
            notes: request.Notes
        );

        await _repository.UpdateAsync(payment, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Payment {PaymentNumber} updated successfully", payment.PaymentNumber);

        return new PaymentUpdateResponse
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            LastModifiedOn = payment.LastModifiedOn.DateTime
        };
    }
}

