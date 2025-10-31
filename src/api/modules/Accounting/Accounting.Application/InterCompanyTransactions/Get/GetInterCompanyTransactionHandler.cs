using Accounting.Application.InterCompanyTransactions.Queries;
using Accounting.Application.InterCompanyTransactions.Responses;

namespace Accounting.Application.InterCompanyTransactions.Get;

/// <summary>
/// Handler for retrieving an inter-company transaction by ID.
/// </summary>
public class GetInterCompanyTransactionHandler(
    [FromKeyedServices("accounting")] IReadRepository<InterCompanyTransaction> repository)
    : IRequestHandler<GetInterCompanyTransactionRequest, InterCompanyTransactionResponse>
{
    public async Task<InterCompanyTransactionResponse> Handle(
        GetInterCompanyTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await repository.FirstOrDefaultAsync(
            new InterCompanyTransactionByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (transaction is null)
        {
            throw new NotFoundException(
                $"{nameof(InterCompanyTransaction)} with ID {request.Id} was not found.");
        }

        return new InterCompanyTransactionResponse
        {
            Id = transaction.Id,
            TransactionNumber = transaction.TransactionNumber,
            TransactionDate = transaction.TransactionDate,
            FromEntityId = transaction.FromEntityId,
            FromEntityName = transaction.FromEntityName,
            ToEntityId = transaction.ToEntityId,
            ToEntityName = transaction.ToEntityName,
            Amount = transaction.Amount,
            TransactionType = transaction.TransactionType,
            Status = transaction.Status,
            IsReconciled = transaction.IsReconciled,
            ReconciliationDate = transaction.ReconciliationDate,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            ReferenceNumber = transaction.ReferenceNumber,
            Description = transaction.Description,
            Notes = transaction.Notes
        };
    }
}

