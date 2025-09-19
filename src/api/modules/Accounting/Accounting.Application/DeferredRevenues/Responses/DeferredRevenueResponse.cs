namespace Accounting.Application.DeferredRevenues.Responses;

/// <summary>
/// Response model representing a deferred revenue entry.
/// Contains deferred revenue information including recognition timing and status.
/// </summary>
public class DeferredRevenueResponse
{
    /// <summary>
    /// Unique identifier for the deferred revenue entry.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Unique deferred revenue number for reference and tracking.
    /// </summary>
    public string DeferredRevenueNumber { get; set; } = default!;
    
    /// <summary>
    /// Date when the revenue should be recognized.
    /// </summary>
    public DateTime RecognitionDate { get; set; }
    
    /// <summary>
    /// Amount of deferred revenue.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Description explaining the nature of the deferred revenue.
    /// </summary>
    public string Description { get; set; } = default!;
    
    /// <summary>
    /// Indicates whether this deferred revenue has been recognized.
    /// </summary>
    public bool IsRecognized { get; set; }
    
    /// <summary>
    /// Date when the revenue was actually recognized (if applicable).
    /// </summary>
    public DateTime? RecognizedDate { get; set; }
}
