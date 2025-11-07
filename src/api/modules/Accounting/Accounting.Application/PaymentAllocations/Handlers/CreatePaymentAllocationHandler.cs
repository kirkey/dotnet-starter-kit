using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Application.PaymentAllocations.Handlers;

/// <summary>
/// Handler for creating a new payment allocation.
/// Allocates a payment amount to a specific invoice.
/// </summary>
public class CreatePaymentAllocationHandler : IRequestHandler<CreatePaymentAllocationCommand, DefaultIdType>
{
    private readonly IRepository<PaymentAllocation> _repository;
    private readonly ILogger<CreatePaymentAllocationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePaymentAllocationHandler"/> class.
    /// </summary>
    public CreatePaymentAllocationHandler(
        IRepository<PaymentAllocation> repository,
        ILogger<CreatePaymentAllocationHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the payment allocation creation command.
    /// </summary>
    public async Task<DefaultIdType> Handle(CreatePaymentAllocationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation(
            "Creating payment allocation: Payment {PaymentId} to Invoice {InvoiceId} for amount {Amount}",
            request.PaymentId, request.InvoiceId, request.Amount);

        var allocation = PaymentAllocation.Create(
            request.PaymentId,
            request.InvoiceId,
            request.Amount,
            request.Notes);

        await _repository.AddAsync(allocation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Payment allocation created successfully with ID {AllocationId}", allocation.Id);

        return allocation.Id;
    }
}
