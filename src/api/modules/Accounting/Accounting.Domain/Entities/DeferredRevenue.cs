namespace Accounting.Domain;

/// <summary>
/// Represents revenue that has been billed or received but not yet earned, to be recognized over time or at future dates.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track prepaid service fees collected in advance from customers.
/// - Manage subscription revenue recognition over contract periods.
/// - Handle customer deposits and connection fees earned over time.
/// - Support compliance with revenue recognition standards (ASC 606/IFRS 15).
/// - Enable proper matching of revenue with service delivery periods.
/// - Maintain accurate liability reporting for unearned revenue.
/// - Process systematic revenue recognition through automated entries.
/// 
/// Default values:
/// - DeferredRevenueNumber: required unique identifier (example: "DEF-2025-001")
/// - RecognitionDate: required future date when revenue should be recognized (example: 2025-12-31)
/// - Amount: required positive decimal (example: 12000.00 for annual prepaid service)
/// - Description: optional description (example: "Annual maintenance fee - ABC Corp")
/// - IsRecognized: false (revenue starts as unrecognized)
/// - RecognizedDate: null (set when revenue is actually recognized)
/// - RecognizedAmount: 0.00 (tracks partial recognition for multi-period deferrals)
/// 
/// Business rules:
/// - Amount must be positive
/// - RecognitionDate should be in the future for new deferrals
/// - Cannot recognize more than the original deferred amount
/// - Once fully recognized, no further changes allowed
/// - Supports partial recognition for multi-period deferrals
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.DeferredRevenue.DeferredRevenueCreated"/>
/// <seealso cref="Accounting.Domain.Events.DeferredRevenue.DeferredRevenueRecognized"/>
/// <seealso cref="Accounting.Domain.Events.DeferredRevenue.DeferredRevenuePartiallyRecognized"/>
/// <seealso cref="Accounting.Domain.Events.DeferredRevenue.DeferredRevenueAdjusted"/>
public class DeferredRevenue : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// A unique identifier for the deferred revenue entry.
    /// </summary>
    public string DeferredRevenueNumber { get; private set; }

    /// <summary>
    /// The date on which the deferred revenue should be recognized.
    /// </summary>
    public DateTime RecognitionDate { get; private set; }

    /// <summary>
    /// The deferred amount to be recognized; must be positive.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Optional description for the deferred revenue entry. Hides the base Description.
    /// </summary>
    public new string? Description { get; private set; }

    /// <summary>
    /// Whether the deferred revenue has been recognized.
    /// </summary>
    /// <remarks>Defaults to <c>false</c> on creation; set to <c>true</c> by <see cref="Recognize"/>.</remarks>
    public bool IsRecognized { get; private set; }

    /// <summary>
    /// When the deferred revenue was recognized, if applicable.
    /// </summary>
    public DateTime? RecognizedDate { get; private set; }

    private DeferredRevenue() {
        DeferredRevenueNumber = string.Empty;
        Description = string.Empty;
    }

    private DeferredRevenue(string deferredRevenueNumber, DateTime recognitionDate, decimal amount, string description)
    {
        DeferredRevenueNumber = deferredRevenueNumber.Trim();
        RecognitionDate = recognitionDate;
        Amount = amount;
        Description = description.Trim();
        IsRecognized = false;
    }

    /// <summary>
    /// Factory to create a new deferred revenue entry with validation.
    /// </summary>
    public static DeferredRevenue Create(string deferredRevenueNumber, DateTime recognitionDate, decimal amount, string description)
    {
        if (string.IsNullOrWhiteSpace(deferredRevenueNumber))
            throw new ArgumentException("Deferred revenue number is required.");
        if (amount <= 0)
            throw new ArgumentException("Deferred revenue amount must be positive.");
        return new DeferredRevenue(deferredRevenueNumber, recognitionDate, amount, description);
    }

    /// <summary>
    /// Mark the deferred revenue as recognized and set the recognition date.
    /// </summary>
    public void Recognize(DateTime recognizedDate)
    {
        if (IsRecognized)
            throw new InvalidOperationException("Deferred revenue already recognized.");
        IsRecognized = true;
        RecognizedDate = recognizedDate;
    }
}
