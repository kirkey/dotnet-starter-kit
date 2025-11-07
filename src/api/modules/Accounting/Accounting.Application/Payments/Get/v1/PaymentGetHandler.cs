using Accounting.Application.Payments.Exceptions;

namespace Accounting.Application.Payments.Get.v1;

/// <summary>
/// Handler for retrieving a payment by ID.
/// </summary>
public sealed class PaymentGetHandler : IRequestHandler<PaymentGetQuery, PaymentGetResponse>
{
    private readonly IReadRepository<Payment> _repository;
    private readonly ILogger<PaymentGetHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentGetHandler"/> class.
    /// </summary>
    public PaymentGetHandler(
        IReadRepository<Payment> repository,
        ILogger<PaymentGetHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the payment retrieval query.
    /// </summary>
    public async Task<PaymentGetResponse> Handle(PaymentGetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Retrieving payment with ID {PaymentId}", request.Id);

        var payment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (payment == null)
        {
            _logger.LogWarning("Payment with ID {PaymentId} not found", request.Id);
            throw new PaymentNotFoundException(request.Id);
        }

        _logger.LogInformation("Payment {PaymentNumber} retrieved successfully", payment.PaymentNumber);

        return new PaymentGetResponse
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            MemberId = payment.MemberId,
            PaymentDate = payment.PaymentDate,
            Amount = payment.Amount,
            UnappliedAmount = payment.UnappliedAmount,
            PaymentMethod = payment.PaymentMethod,
            ReferenceNumber = payment.ReferenceNumber,
            DepositToAccountCode = payment.DepositToAccountCode,
            Description = payment.Description,
            Notes = payment.Notes,
            Allocations = payment.Allocations.Select(a => new PaymentAllocationDto
            {
                Id = a.Id,
                InvoiceId = a.InvoiceId,
                Amount = a.Amount
            }).ToList(),
            CreatedOn = payment.CreatedOn.DateTime,
            LastModifiedOn = payment.LastModifiedOn.DateTime
        };
    }
}

