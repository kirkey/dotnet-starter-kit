namespace Accounting.Application.GeneralLedgers.Get.v1;

/// <summary>
/// Handler for retrieving a general ledger entry by ID.
/// </summary>
public sealed class GeneralLedgerGetHandler : IRequestHandler<GeneralLedgerGetQuery, GeneralLedgerGetResponse>
{
    private readonly IReadRepository<GeneralLedger> _repository;
    private readonly ILogger<GeneralLedgerGetHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralLedgerGetHandler"/> class.
    /// </summary>
    public GeneralLedgerGetHandler(
        IReadRepository<GeneralLedger> repository,
        ILogger<GeneralLedgerGetHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the general ledger entry retrieval query.
    /// </summary>
    public async Task<GeneralLedgerGetResponse> Handle(GeneralLedgerGetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Retrieving general ledger entry with ID {LedgerId}", request.Id);

        var entry = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null)
        {
            _logger.LogWarning("General ledger entry with ID {LedgerId} not found", request.Id);
            throw new NotFoundException($"General ledger entry with ID {request.Id} not found");
        }

        _logger.LogInformation("General ledger entry {LedgerId} retrieved successfully", entry.Id);

        return new GeneralLedgerGetResponse
        {
            Id = entry.Id,
            EntryId = entry.EntryId,
            AccountId = entry.AccountId,
            Debit = entry.Debit,
            Credit = entry.Credit,
            Memo = entry.Memo,
            UsoaClass = entry.UsoaClass ?? string.Empty,
            TransactionDate = entry.TransactionDate,
            ReferenceNumber = entry.ReferenceNumber,
            PeriodId = entry.PeriodId,
            Description = entry.Description,
            Notes = entry.Notes,
            CreatedOn = entry.CreatedOn.DateTime,
            CreatedByUserName = entry.CreatedByUserName ?? string.Empty,
            IsPosted = entry.IsPosted,
            PostedDate = entry.PostedDate,
            PostedBy = entry.PostedBy,
            Source = entry.Source,
            SourceId = entry.SourceId
        };
    }
}
