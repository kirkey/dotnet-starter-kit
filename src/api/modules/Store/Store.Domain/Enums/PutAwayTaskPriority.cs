namespace Store.Domain.Enums;

/// <summary>
/// Represents the priority levels for put-away tasks.
/// </summary>
/// <remarks>
/// Put-away task priorities help optimize warehouse operations by prioritizing
/// time-sensitive or high-value items for storage.
/// 
/// Priority levels:
/// - Low (1): Standard items, non-urgent
/// - Normal (5): Regular priority items
/// - High (10): Time-sensitive items, perishables
/// - Critical (20): Emergency items, production stoppers
/// 
/// Business rules:
/// - Higher numbers indicate higher priority
/// - Priority affects task assignment and execution order
/// - Critical items should be put away first
/// - Perishable items typically get high priority
/// </remarks>
public static class PutAwayTaskPriority
{
    /// <summary>
    /// Low priority put-away task (1).
    /// For standard items that are not time-sensitive.
    /// </summary>
    public const int Low = 1;

    /// <summary>
    /// Normal priority put-away task (5).
    /// For regular items with standard handling requirements.
    /// </summary>
    public const int Normal = 5;

    /// <summary>
    /// High priority put-away task (10).
    /// For time-sensitive items, perishables, or high-velocity items.
    /// </summary>
    public const int High = 10;

    /// <summary>
    /// Critical priority put-away task (20).
    /// For emergency items, production stoppers, or urgent customer orders.
    /// </summary>
    public const int Critical = 20;

    /// <summary>
    /// Gets all valid put-away task priority values.
    /// </summary>
    public static readonly int[] All = [Low, Normal, High, Critical];
}
