namespace Accounting.Application.GeneralLedgers.Get.v1;

/// <summary>
/// Handler for retrieving a general ledger entry by ID.
/// </summary>
public sealed class GeneralLedgerGetHandler(
    [FromKeyedServices("accounting:general-ledger")] IReadRepository<GeneralLedger> repository,
    ILogger<GeneralLedgerGetHandler> logger)
    : IRequestHandler<GeneralLedgerGetRequest, GeneralLedgerGetResponse>
{
    /// <summary>
    /// Handles the general ledger entry retrieval request.
    /// </summary>
    public async Task<GeneralLedgerGetResponse> Handle(GeneralLedgerGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Retrieving general ledger entry with ID {LedgerId}", request.Id);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null)
        {
            logger.LogWarning("General ledger entry with ID {LedgerId} not found", request.Id);
            throw new NotFoundException($"General ledger entry with ID {request.Id} not found");
        }

        logger.LogInformation("General ledger entry {LedgerId} retrieved successfully", entry.Id);

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
