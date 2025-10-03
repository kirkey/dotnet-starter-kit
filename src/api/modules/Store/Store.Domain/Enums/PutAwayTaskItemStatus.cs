namespace Store.Domain.Enums;

/// <summary>
/// Represents the status of individual put-away task items.
/// </summary>
/// <remarks>
/// Put-away task item statuses track the progress of individual line items
/// within a put-away task from pending to completion.
/// 
/// Status flow:
/// Pending → PutAway
///     ↓
/// Exception (if issues occur)
/// 
/// Business rules:
/// - Items start in Pending status
/// - Successfully processed items move to PutAway status
/// - Items with issues (wrong quantity, damaged, etc.) move to Exception status
/// - Exception items require manual intervention
/// </remarks>
public static class PutAwayTaskItemStatus
{
    /// <summary>
    /// Put-away task item is pending and waiting to be processed.
    /// </summary>
    public const string Pending = "Pending";

    /// <summary>
    /// Put-away task item has been successfully put away.
    /// </summary>
    public const string PutAway = "PutAway";

    /// <summary>
    /// Put-away task item has an exception and requires manual intervention.
    /// </summary>
    public const string Exception = "Exception";

    /// <summary>
    /// Gets all valid put-away task item status values.
    /// </summary>
    public static readonly string[] All = [Pending, PutAway, Exception];

    /// <summary>
    /// Gets active put-away task item status values (not completed).
    /// </summary>
    public static readonly string[] Active = [Pending, Exception];
}
