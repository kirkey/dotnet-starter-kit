namespace Accounting.Domain.Constants;

/// <summary>
/// String-based enumeration of supported statuses for credit memos.
/// </summary>
/// <remarks>
/// Status lifecycle:
/// - Draft: Initial state, can be modified or deleted
/// - Approved: Cannot be modified or deleted
/// - Applied: Applied to invoices/bills
/// - Refunded: Refunded directly to customer/vendor
/// - Voided: Cancelled and no longer valid
/// </remarks>
public static class CreditMemoStatus
{
    /// <summary>
    /// Draft status - memo is being prepared and can be modified or deleted.
    /// </summary>
    public const string Draft = "Draft";

    /// <summary>
    /// Approved status - memo has been approved and can be applied or refunded.
    /// </summary>
    public const string Approved = "Approved";

    /// <summary>
    /// Applied status - memo has been applied to invoices/bills.
    /// </summary>
    public const string Applied = "Applied";

    /// <summary>
    /// Refunded status - memo amount has been refunded directly.
    /// </summary>
    public const string Refunded = "Refunded";

    /// <summary>
    /// Voided status - memo has been cancelled and is no longer valid.
    /// </summary>
    public const string Voided = "Voided";

    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        Draft, Approved, Applied, Refunded, Voided
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

