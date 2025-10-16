namespace Accounting.Domain.Constants;

/// <summary>
/// String-based enumeration of supported statuses for debit memos.
/// </summary>
/// <remarks>
/// Status lifecycle:
/// - Draft: Initial state, can be modified or deleted
/// - Approved: Cannot be modified or deleted
/// - Applied: Applied to invoices/bills
/// - Voided: Cancelled and no longer valid
/// </remarks>
public static class DebitMemoStatus
{
    /// <summary>
    /// Draft status - memo is being prepared and can be modified or deleted.
    /// </summary>
    public const string Draft = "Draft";

    /// <summary>
    /// Approved status - memo has been approved and can be applied.
    /// </summary>
    public const string Approved = "Approved";

    /// <summary>
    /// Applied status - memo has been applied to invoices/bills.
    /// </summary>
    public const string Applied = "Applied";

    /// <summary>
    /// Voided status - memo has been cancelled and is no longer valid.
    /// </summary>
    public const string Voided = "Voided";

    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        Draft, Approved, Applied, Voided
    };

    /// <summary>
    /// Returns true if the provided status is one of the allowed values.
    /// </summary>
    /// <param name="status">The status to validate.</param>
    /// <returns>True if the status is allowed, false otherwise.</returns>
    public static bool IsAllowed(string? status) => status is not null && Allowed.Contains(status);

    /// <summary>
    /// Returns true if the status allows modification of the memo.
    /// </summary>
    /// <param name="status">The status to check.</param>
    /// <returns>True if modifications are allowed, false otherwise.</returns>
    public static bool IsModifiable(string status) => string.Equals(status, Draft, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns true if the status allows deletion of the memo.
    /// </summary>
    /// <param name="status">The status to check.</param>
    /// <returns>True if deletion is allowed, false otherwise.</returns>
    public static bool IsDeletable(string status) => string.Equals(status, Draft, StringComparison.OrdinalIgnoreCase);
}
