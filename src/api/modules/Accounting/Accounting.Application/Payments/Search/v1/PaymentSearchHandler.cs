namespace Accounting.Application.Payments.Search.v1;

/// <summary>
/// Handler for searching payments.
/// </summary>
public sealed class PaymentSearchHandler : IRequestHandler<PaymentSearchRequest, PagedList<PaymentSearchResponse>>
{
    private readonly IReadRepository<Payment> _repository;
    private readonly ILogger<PaymentSearchHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentSearchHandler"/> class.
    /// </summary>
    public PaymentSearchHandler(
        IReadRepository<Payment> repository,
        ILogger<PaymentSearchHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the payment search query.
    /// </summary>
    public async Task<PagedList<PaymentSearchResponse>> Handle(PaymentSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Searching payments with filters");

        var spec = new PaymentSearchSpec(request);
        var payments = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(cancellationToken);

        var response = payments.Select(p => new PaymentSearchResponse
        {
            Id = p.Id,
            PaymentNumber = p.PaymentNumber,
            MemberId = p.MemberId,
            PaymentDate = p.PaymentDate,
            Amount = p.Amount,
            UnappliedAmount = p.UnappliedAmount,
            PaymentMethod = p.PaymentMethod,
            ReferenceNumber = p.ReferenceNumber,
            DepositToAccountCode = p.DepositToAccountCode,
            Description = p.Description,
            AllocationCount = p.Allocations.Count,
            CreatedOn = p.CreatedOn.DateTime
        }).ToList();

        _logger.LogInformation("Found {Count} payments", response.Count);

        return new PagedList<PaymentSearchResponse>(
            response,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
