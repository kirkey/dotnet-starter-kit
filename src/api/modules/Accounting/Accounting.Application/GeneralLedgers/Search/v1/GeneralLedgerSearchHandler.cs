namespace Accounting.Application.GeneralLedgers.Search.v1;

/// <summary>
/// Handler for searching general ledger entries.
/// </summary>
public sealed class GeneralLedgerSearchHandler : IRequestHandler<GeneralLedgerSearchQuery, PagedList<GeneralLedgerSearchResponse>>
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
    /// Handles the general ledger search query.
    /// </summary>
    public async Task<PagedList<GeneralLedgerSearchResponse>> Handle(GeneralLedgerSearchQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Searching general ledger entries with filters");

        var spec = new GeneralLedgerSearchSpec(request);
        var entries = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(cancellationToken);

        var response = entries.Select(e => new GeneralLedgerSearchResponse
        {
            Id = e.Id,
            EntryId = e.EntryId,
            AccountId = e.AccountId,
            Debit = e.Debit,
            Credit = e.Credit,
            Memo = e.Memo,
            UsoaClass = e.UsoaClass,
            TransactionDate = e.TransactionDate,
            ReferenceNumber = e.ReferenceNumber,
            PeriodId = e.PeriodId,
            Description = e.Description,
            CreatedOn = e.CreatedOn.DateTime
        }).ToList();

        _logger.LogInformation("Found {Count} general ledger entries", response.Count);

        return new PagedList<GeneralLedgerSearchResponse>(
            response,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}

