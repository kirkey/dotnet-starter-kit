using Accounting.Application.Payments.Exceptions;

namespace Accounting.Application.Payments.Delete.v1;

/// <summary>
/// Handler for deleting a payment.
/// </summary>
public sealed class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand>
{
    private readonly IRepository<Payment> _repository;
    private readonly ILogger<DeletePaymentHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePaymentHandler"/> class.
    /// </summary>
    public DeletePaymentHandler(
        IRepository<Payment> repository,
        ILogger<DeletePaymentHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the payment deletion command.
    /// </summary>
    public async Task Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Deleting payment with ID {PaymentId}", request.Id);

        var payment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (payment == null)
        {
            _logger.LogWarning("Payment with ID {PaymentId} not found", request.Id);
            throw new PaymentNotFoundException(request.Id);
        }

        // Business rule: Cannot delete payment with allocations
        if (payment.Allocations.Any())
        {
            _logger.LogWarning("Cannot delete payment {PaymentNumber} - has allocations", payment.PaymentNumber);
            throw new InvalidOperationException($"Cannot delete payment {payment.PaymentNumber} because it has allocations. Consider voiding instead.");
        }

        await _repository.DeleteAsync(payment, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Payment {PaymentNumber} deleted successfully", payment.PaymentNumber);
    }
}
