using Accounting.Application.PaymentAllocations.Queries;
using Accounting.Application.PaymentAllocations.Responses;

namespace Accounting.Application.PaymentAllocations.Handlers;

/// <summary>
/// Handler for searching payment allocations with filtering and pagination.
/// </summary>
public class SearchPaymentAllocationsHandler : IRequestHandler<SearchPaymentAllocationsQuery, PagedList<PaymentAllocationResponse>>
{
    private readonly IReadRepository<PaymentAllocation> _repository;
    private readonly ILogger<SearchPaymentAllocationsHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPaymentAllocationsHandler"/> class.
    /// </summary>
    public SearchPaymentAllocationsHandler(
        IReadRepository<PaymentAllocation> repository,
        ILogger<SearchPaymentAllocationsHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the search payment allocations query.
    /// </summary>
    /// <param name="request">The search query containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paged list of payment allocation responses.</returns>
    public async Task<PagedList<PaymentAllocationResponse>> Handle(SearchPaymentAllocationsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Searching payment allocations with filters");

        var spec = new SearchPaymentAllocationsSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await _repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Found {Count} payment allocations", list.Count);

        return new PagedList<PaymentAllocationResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
