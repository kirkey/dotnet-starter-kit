namespace Accounting.Application.InterCompanyTransactions.Create.v1;

/// <summary>
/// Command to create a new inter-company transaction.
/// </summary>
public record InterCompanyTransactionCreateCommand(
    string TransactionNumber,
    DefaultIdType FromEntityId,
    string FromEntityName,
    DefaultIdType ToEntityId,
    string ToEntityName,
    DateTime TransactionDate,
    decimal Amount,
    string TransactionType,
    DefaultIdType FromAccountId,
    DefaultIdType ToAccountId,
    string? ReferenceNumber = null,
    DateTime? DueDate = null,
    bool RequiresElimination = true,
    DefaultIdType? PeriodId = null,
    string? Description = null,
    string? Notes = null
) : IRequest<InterCompanyTransactionCreateResponse>;

