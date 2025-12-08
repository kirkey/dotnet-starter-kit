namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareTransactions;

/// <summary>
/// ViewModel for share transaction entities (read-only view).
/// Transactions are created via buy/sell operations on share accounts.
/// </summary>
public class ShareTransactionViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Reference number for the transaction.
    /// </summary>
    public string? Reference { get; set; }
    
    /// <summary>
    /// Type of transaction (Buy, Sell, Dividend, etc.).
    /// </summary>
    public string? TransactionType { get; set; }
    
    /// <summary>
    /// Number of shares involved in transaction.
    /// </summary>
    public int NumberOfShares { get; set; }
    
    /// <summary>
    /// Total transaction amount.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Description of the transaction.
    /// </summary>
    public string? Description { get; set; }
}
