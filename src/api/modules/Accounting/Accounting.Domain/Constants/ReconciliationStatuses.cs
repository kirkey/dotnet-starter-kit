namespace Accounting.Domain.Constants;

/// <summary>
/// String-based constants for bank reconciliation status values.
/// Used instead of enums to maintain flexibility and allow easy database-level filtering.
/// </summary>
public static class ReconciliationStatuses
{
    /// <summary>
    /// Initial status for newly created reconciliations.
    /// Reconciliation items have not yet been entered.
    /// </summary>
    public const string Pending = "Pending";

    /// <summary>
    /// Reconciliation is in progress.
    /// User is actively entering outstanding checks, deposits in transit, and errors.
    /// </summary>
    public const string InProgress = "InProgress";

    /// <summary>
    /// Reconciliation has been completed but awaiting approval.
    /// Balance verification passed and is ready for approval workflow.
    /// </summary>
    public const string Completed = "Completed";

    /// <summary>
    /// Reconciliation has been approved.
    /// Final status indicating reconciliation is complete and approved.
    /// </summary>
    public const string Approved = "Approved";

    /// <summary>
    /// Gets all valid reconciliation status values.
    /// </summary>
    public static string[] GetAllStatuses() => new[] { Pending, InProgress, Completed, Approved };

    /// <summary>
    /// Validates if a given status string is one of the allowed values.
    /// </summary>
    /// <param name="status">The status string to validate.</param>
    /// <returns>True if the status is valid; otherwise, false.</returns>
    public static bool IsValid(string status) => GetAllStatuses().Contains(status, StringComparer.OrdinalIgnoreCase);
}

