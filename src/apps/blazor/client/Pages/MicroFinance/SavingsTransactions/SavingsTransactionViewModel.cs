namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsTransactions;

/// <summary>
/// ViewModel for savings transactions (read-only entity).
/// Used for display purposes only as transactions are created through deposits/withdrawals.
/// </summary>
public class SavingsTransactionViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Reference for the transaction.
    /// </summary>
    public string? Reference { get; set; }
    
    /// <summary>
    /// Type of transaction (Deposit, Withdrawal, Interest, Fee, etc.).
    /// </summary>
    public string? TransactionType { get; set; }
    
    /// <summary>
    /// Transaction amount.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Description of the transaction.
    /// </summary>
    public string? Description { get; set; }
}
