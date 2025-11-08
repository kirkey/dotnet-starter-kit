namespace Accounting.Application.AccountReconciliations.Commands.ReconcileAccount.v1;

/// <summary>
/// Command to reconcile a general ledger account (chart of account) against a statement balance for a given date.
/// </summary>
public class ReconcileGeneralLedgerAccountCommand : BaseRequest, IRequest<DefaultIdType>
{
    /// <summary>
    /// The unique identifier of the chart of account to reconcile.
    /// </summary>
    public DefaultIdType ChartOfAccountId { get; set; }

    /// <summary>
    /// The date for which reconciliation is performed.
    /// </summary>
    public DateTime ReconciliationDate { get; set; }

    /// <summary>
    /// The statement balance as per the external statement.
    /// </summary>
    public decimal StatementBalance { get; set; }

    /// <summary>
    /// Optional reference for the reconciliation.
    /// </summary>
    public string? ReconciliationReference { get; set; }

    /// <summary>
    /// List of reconciliation line items detailing transactions.
    /// </summary>
    public List<ReconciliationLineDto> ReconciliationLines { get; set; } = new();
}

/// <summary>
/// DTO representing a single line item in account reconciliation.
/// </summary>
public class ReconciliationLineDto
{
    /// <summary>
    /// Optional transaction identifier.
    /// </summary>
    public DefaultIdType? TransactionId { get; set; }

    /// <summary>
    /// Date of the transaction.
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Description of the transaction.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Amount of the transaction.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Indicates if the transaction is cleared.
    /// </summary>
    public bool IsCleared { get; set; }
}
