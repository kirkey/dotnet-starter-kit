namespace Store.Domain.Enums;

/// <summary>
/// Represents the status of a put-away task.
/// </summary>
/// <remarks>
/// Put-away task statuses represent the workflow stages from creation to completion.
/// 
/// Status flow:
/// Created → Assigned → InProgress → Completed
///             ↓           ↓
///         Cancelled   Cancelled
/// 
/// Business rules:
/// - Tasks start in Created status
/// - Only Created tasks can be assigned
/// - Only Assigned tasks can be started
/// - Only InProgress tasks can be completed
/// - Tasks can be cancelled from any status except Completed
/// </remarks>
public static class PutAwayTaskStatus
{
    /// <summary>
    /// Put-away task has been created but not yet assigned to a worker.
    /// </summary>
    public const string Created = "Created";

    /// <summary>
    /// Put-away task has been assigned to a worker but not yet started.
    /// </summary>
    public const string Assigned = "Assigned";

    /// <summary>
    /// Put-away task is currently being executed by the assigned worker.
    /// </summary>
    public const string InProgress = "InProgress";

    /// <summary>
    /// Put-away task has been completed successfully.
    /// </summary>
    public const string Completed = "Completed";

    /// <summary>
    /// Put-away task has been cancelled and will not be executed.
    /// </summary>
    public const string Cancelled = "Cancelled";

    /// <summary>
    /// Gets all valid put-away task status values.
    /// </summary>
    public static readonly string[] All = [Created, Assigned, InProgress, Completed, Cancelled];

    /// <summary>
    /// Gets active put-away task status values (not completed or cancelled).
    /// </summary>
    public static readonly string[] Active = [Created, Assigned, InProgress];
}
