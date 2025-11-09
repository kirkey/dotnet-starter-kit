namespace Accounting.Application.GeneralLedgers.Search.v1;

/// <summary>
/// Handler for searching general ledger entries.
/// </summary>
public sealed class GeneralLedgerSearchHandler : IRequestHandler<GeneralLedgerSearchRequest, PagedList<GeneralLedgerSearchResponse>>
{
    private readonly IReadRepository<GeneralLedger> _repository;
    private readonly ILogger<GeneralLedgerSearchHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralLedgerSearchHandler"/> class.
    /// </summary>
    public GeneralLedgerSearchHandler(
        IReadRepository<GeneralLedger> repository,
        ILogger<GeneralLedgerSearchHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the general ledger search request.
    /// </summary>
    public async Task<PagedList<GeneralLedgerSearchResponse>> Handle(GeneralLedgerSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Searching general ledger entries with filters");

        var spec = new GeneralLedgerSearchSpec(request);
        var items = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(spec, cancellationToken);

        _logger.LogInformation("Found {Count} general ledger entries", items.Count);

        return new PagedList<GeneralLedgerSearchResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

