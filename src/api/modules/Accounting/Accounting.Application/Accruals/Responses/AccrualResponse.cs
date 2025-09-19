namespace Accounting.Application.Accruals.Responses;

/// <summary>
/// Response model representing an accrual entry.
/// Contains accrual information including amount, timing, and reversal status.
/// </summary>
public class AccrualResponse
{
    /// <summary>
    /// Unique identifier for the accrual entry.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Unique accrual number for reference and tracking.
    /// </summary>
    public string AccrualNumber { get; set; } = default!;
    
    /// <summary>
    /// Date when the accrual was recorded.
    /// </summary>
    public DateTime AccrualDate { get; set; }
    
    /// <summary>
    /// Amount of the accrual entry.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Description explaining the purpose or nature of the accrual.
    /// </summary>
    public string Description { get; set; } = default!;
    
    /// <summary>
    /// Indicates whether this accrual has been reversed.
    /// </summary>
    public bool IsReversed { get; set; }
    
    /// <summary>
    /// Date when the accrual was reversed (if applicable).
    /// </summary>
    public DateTime? ReversalDate { get; set; }
}
