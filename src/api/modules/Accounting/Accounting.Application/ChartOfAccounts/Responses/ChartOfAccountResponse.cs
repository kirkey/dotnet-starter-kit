namespace Accounting.Application.ChartOfAccounts.Responses;

/// <summary>
/// Response model representing a chart of account entry.
/// Contains account information including code, type, balance, and hierarchical relationships.
/// </summary>
public class ChartOfAccountResponse : BaseDto
{
    /// <summary>
    /// Unique account code identifier.
    /// </summary>
    public string AccountCode { get; set; } = null!;
    
    /// <summary>
    /// Type of account (Asset, Liability, Equity, Revenue, Expense).
    /// </summary>
    public string AccountType { get; set; } = null!;
    
    /// <summary>
    /// Parent account identifier for hierarchical account structure.
    /// </summary>
    public DefaultIdType? ParentAccountId { get; set; } = null!;
    
    /// <summary>
    /// Uniform System of Accounts category classification.
    /// </summary>
    public string UsoaCategory { get; set; } = null!;
    
    /// <summary>
    /// Indicates if the account is currently active for transactions.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Parent account code for display purposes.
    /// </summary>
    public string ParentCode { get; set; } = null!;
    
    /// <summary>
    /// Current account balance.
    /// </summary>
    public decimal Balance { get; set; }
    
    /// <summary>
    /// Indicates if this is a control account that summarizes subsidiary accounts.
    /// </summary>
    public bool IsControlAccount { get; set; }
    
    /// <summary>
    /// Normal balance type (Debit or Credit) for this account.
    /// </summary>
    public string NormalBalance { get; set; } = null!;
    
    /// <summary>
    /// Depth level in the account hierarchy.
    /// </summary>
    public int AccountLevel { get; set; }
}
