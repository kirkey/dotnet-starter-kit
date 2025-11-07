using Accounting.Application.PaymentAllocations.Commands;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.PaymentAllocations.Handlers;

/// <summary>
/// Handler for updating payment allocation details.
/// </summary>
public class UpdatePaymentAllocationHandler : IRequestHandler<UpdatePaymentAllocationCommand, DefaultIdType>
{
    private readonly IRepository<PaymentAllocation> _repository;
    private readonly ILogger<UpdatePaymentAllocationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePaymentAllocationHandler"/> class.
    /// </summary>
    public UpdatePaymentAllocationHandler(
        IRepository<PaymentAllocation> repository,
        ILogger<UpdatePaymentAllocationHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the payment allocation update command.
    /// </summary>
    public async Task<DefaultIdType> Handle(UpdatePaymentAllocationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Updating payment allocation with ID {AllocationId}", request.Id);

        var allocation = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (allocation == null)
        {
            _logger.LogWarning("Payment allocation with ID {AllocationId} not found", request.Id);
            throw new PaymentAllocationByIdNotFoundException(request.Id);
        }

        // Use the entity's Update method
        allocation.Update(request.Amount, request.Notes);

        await _repository.UpdateAsync(allocation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Payment allocation {AllocationId} updated successfully", allocation.Id);

        return allocation.Id;
    }
}
