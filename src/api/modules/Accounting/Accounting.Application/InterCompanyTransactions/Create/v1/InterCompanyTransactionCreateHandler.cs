using Accounting.Application.InterCompanyTransactions.Queries;

namespace Accounting.Application.InterCompanyTransactions.Create.v1;

/// <summary>
/// Handler for creating a new inter-company transaction.
/// </summary>
public sealed class InterCompanyTransactionCreateHandler(
    ILogger<InterCompanyTransactionCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<InterCompanyTransaction> repository)
    : IRequestHandler<InterCompanyTransactionCreateCommand, InterCompanyTransactionCreateResponse>
{
    public async Task<InterCompanyTransactionCreateResponse> Handle(InterCompanyTransactionCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate transaction number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new InterCompanyTransactionByNumberSpec(request.TransactionNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicateInterCompanyTransactionNumberException(request.TransactionNumber);
        }

        var transaction = InterCompanyTransaction.Create(
            transactionNumber: request.TransactionNumber,
            fromEntityId: request.FromEntityId,
            fromEntityName: request.FromEntityName,
            toEntityId: request.ToEntityId,
            toEntityName: request.ToEntityName,
            transactionDate: request.TransactionDate,
            amount: request.Amount,
            transactionType: request.TransactionType,
            fromAccountId: request.FromAccountId,
            toAccountId: request.ToAccountId,
            referenceNumber: request.ReferenceNumber,
            dueDate: request.DueDate,
            requiresElimination: request.RequiresElimination,
            periodId: request.PeriodId,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(transaction, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Inter-company transaction created {TransactionId} - {TransactionNumber}", 
            transaction.Id, transaction.TransactionNumber);
        return new InterCompanyTransactionCreateResponse(transaction.Id);
    }
}

