using Accounting.Application.InterCompanyTransactions.Queries;
using Accounting.Application.InterCompanyTransactions.Responses;

namespace Accounting.Application.InterCompanyTransactions.Search.v1;

/// <summary>
/// Handler for searching inter-company transactions with filters.
/// </summary>
public sealed class SearchInterCompanyTransactionsHandler(
    ILogger<SearchInterCompanyTransactionsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<InterCompanyTransaction> repository)
    : IRequestHandler<SearchInterCompanyTransactionsRequest, List<InterCompanyTransactionResponse>>
{
    public async Task<List<InterCompanyTransactionResponse>> Handle(SearchInterCompanyTransactionsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new InterCompanyTransactionSearchSpec(
            request.TransactionNumber,
            request.FromEntityId,
            request.ToEntityId,
            request.TransactionType,
            request.Status,
            request.IsReconciled);

        var transactions = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} inter-company transactions", transactions.Count);

        return transactions.Select(t => new InterCompanyTransactionResponse
        {
            Id = t.Id,
            TransactionNumber = t.TransactionNumber,
            FromEntityId = t.FromEntityId,
            FromEntityName = t.FromEntityName,
            ToEntityId = t.ToEntityId,
            ToEntityName = t.ToEntityName,
            TransactionDate = t.TransactionDate,
            Amount = t.Amount,
            TransactionType = t.TransactionType,
            Status = t.Status,
            IsReconciled = t.IsReconciled,
            ReconciliationDate = t.ReconciliationDate,
            FromAccountId = t.FromAccountId,
            ToAccountId = t.ToAccountId,
            ReferenceNumber = t.ReferenceNumber,
            Description = t.Description,
            Notes = t.Notes
        }).ToList();
    }
}
