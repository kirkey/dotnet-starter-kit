namespace Accounting.Application.BankReconciliations.Update.v1;

/// <summary>
/// Command to update reconciliation items and recalculate adjusted balance.
/// Used during the reconciliation process to record outstanding checks, deposits in transit, and errors.
/// </summary>
public sealed class UpdateBankReconciliationCommand : BaseRequest, IRequest<DefaultIdType>
{
    /// <summary>
    /// Total of outstanding checks not yet cleared by the bank.
    /// Required. Must be non-negative.
    /// </summary>
    public decimal OutstandingChecksTotal { get; set; }

    /// <summary>
    /// Total of deposits in transit not yet shown on the bank statement.
    /// Required. Must be non-negative.
    /// </summary>
    public decimal DepositsInTransitTotal { get; set; }

    /// <summary>
    /// Bank errors requiring correction by the bank.
    /// Can be positive or negative. Maximum 999999999.99.
    /// </summary>
    public decimal BankErrors { get; set; }

    /// <summary>
    /// Book errors requiring adjustment entries to the general ledger.
    /// Can be positive or negative. Maximum 999999999.99.
    /// </summary>
    public decimal BookErrors { get; set; }
}
