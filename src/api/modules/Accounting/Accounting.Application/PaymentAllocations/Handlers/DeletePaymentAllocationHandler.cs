using Accounting.Application.PaymentAllocations.Commands;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.PaymentAllocations.Handlers;

/// <summary>
/// Handler for deleting a payment allocation.
/// </summary>
public class DeletePaymentAllocationHandler : IRequestHandler<DeletePaymentAllocationCommand, Unit>
{
    private readonly IRepository<PaymentAllocation> _repository;
    private readonly ILogger<DeletePaymentAllocationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePaymentAllocationHandler"/> class.
    /// </summary>
    public DeletePaymentAllocationHandler(
        IRepository<PaymentAllocation> repository,
        ILogger<DeletePaymentAllocationHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the payment allocation deletion command.
    /// </summary>
    public async Task<Unit> Handle(DeletePaymentAllocationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Deleting payment allocation with ID {AllocationId}", request.Id);

        var allocation = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (allocation == null)
        {
            _logger.LogWarning("Payment allocation with ID {AllocationId} not found", request.Id);
            throw new PaymentAllocationByIdNotFoundException(request.Id);
        }

        await _repository.DeleteAsync(allocation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Payment allocation {AllocationId} deleted successfully (Payment: {PaymentId}, Invoice: {InvoiceId}, Amount: {Amount})",
            allocation.Id, allocation.PaymentId, allocation.InvoiceId, allocation.Amount);

        return Unit.Value;
    }
}
