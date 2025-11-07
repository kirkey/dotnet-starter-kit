using Accounting.Application.PaymentAllocations.Queries;
using Accounting.Application.PaymentAllocations.Responses;

namespace Accounting.Application.PaymentAllocations.Handlers;

/// <summary>
/// Handler for retrieving a payment allocation by ID.
/// </summary>
public class GetPaymentAllocationByIdHandler : IRequestHandler<GetPaymentAllocationByIdQuery, PaymentAllocationResponse>
{
    private readonly IReadRepository<PaymentAllocation> _repository;
    private readonly ILogger<GetPaymentAllocationByIdHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPaymentAllocationByIdHandler"/> class.
    /// </summary>
    public GetPaymentAllocationByIdHandler(
        IReadRepository<PaymentAllocation> repository,
        ILogger<GetPaymentAllocationByIdHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the get payment allocation by ID query.
    /// </summary>
    public async Task<PaymentAllocationResponse> Handle(GetPaymentAllocationByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Retrieving payment allocation with ID {AllocationId}", request.Id);

        var allocation = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (allocation == null)
        {
            _logger.LogWarning("Payment allocation with ID {AllocationId} not found", request.Id);
            throw new PaymentAllocationByIdNotFoundException(request.Id);
        }

        _logger.LogInformation("Payment allocation {AllocationId} retrieved successfully", allocation.Id);

        return new PaymentAllocationResponse
        {
            Id = allocation.Id,
            PaymentId = allocation.PaymentId,
            InvoiceId = allocation.InvoiceId,
            Amount = allocation.Amount,
            Notes = allocation.Notes,
            CreatedOn = allocation.CreatedOn.DateTime
        };
    }
}
