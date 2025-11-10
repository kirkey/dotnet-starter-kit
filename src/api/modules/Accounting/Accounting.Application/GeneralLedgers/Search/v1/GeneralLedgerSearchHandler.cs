namespace Accounting.Application.GeneralLedgers.Search.v1;

/// <summary>
/// Handler for searching general ledger entries.
/// </summary>
public sealed class GeneralLedgerSearchHandler(
    [FromKeyedServices("accounting:general-ledger")] IReadRepository<GeneralLedger> repository,
    ILogger<GeneralLedgerSearchHandler> logger)
    : IRequestHandler<GeneralLedgerSearchRequest, PagedList<GeneralLedgerSearchResponse>>
{
    /// <summary>
    /// Handles the general ledger search request.
    /// </summary>
    public async Task<PagedList<GeneralLedgerSearchResponse>> Handle(GeneralLedgerSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Searching general ledger entries with filters");

        var spec = new GeneralLedgerSearchSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        logger.LogInformation("Found {Count} general ledger entries", items.Count);

        return new PagedList<GeneralLedgerSearchResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

