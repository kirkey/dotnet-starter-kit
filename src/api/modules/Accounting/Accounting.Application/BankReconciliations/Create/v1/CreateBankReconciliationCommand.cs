namespace Accounting.Application.BankReconciliations.Create.v1;

/// <summary>
/// Command to create a new bank reconciliation.
/// Initializes a reconciliation with opening balances from bank statement and general ledger.
/// </summary>
public class CreateBankReconciliationCommand : BaseRequest, IRequest<DefaultIdType>
{
    /// <summary>
    /// Reference to the bank account in chart of accounts being reconciled.
    /// Required and must reference a valid bank account.
    /// </summary>
    public DefaultIdType BankAccountId { get; set; }

    /// <summary>
    /// Date of the bank statement being reconciled.
    /// Required. Cannot be in the future.
    /// </summary>
    public DateTime ReconciliationDate { get; set; }

    /// <summary>
    /// Ending balance per bank statement.
    /// Required. Must be non-negative.
    /// </summary>
    public decimal StatementBalance { get; set; }

    /// <summary>
    /// Book balance per general ledger before adjustments.
    /// Required. Must be non-negative.
    /// </summary>
    public decimal BookBalance { get; set; }

    /// <summary>
    /// Optional reference number for the bank statement.
    /// Maximum 100 characters.
    /// </summary>
    public string? StatementNumber { get; set; }

    /// <summary>
    /// Optional description of the reconciliation.
    /// Maximum 2048 characters.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional internal notes about the reconciliation.
    /// Maximum 2048 characters.
    /// </summary>
    public string? Notes { get; set; }
}
