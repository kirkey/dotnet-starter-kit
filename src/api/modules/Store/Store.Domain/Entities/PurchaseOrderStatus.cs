namespace Store.Domain.Entities;

/// <summary>
/// Well-known lifecycle statuses for <see cref="PurchaseOrder"/> including helper methods.
/// </summary>
public static class PurchaseOrderStatus
{
    public const string Draft = "Draft";
    public const string Submitted = "Submitted";
    public const string Approved = "Approved";
    public const string Sent = "Sent";
    public const string Received = "Received";
    public const string Cancelled = "Cancelled";

    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        Draft, Submitted, Approved, Sent, Received, Cancelled
    };

    /// <summary>
    /// Returns true if the provided status is one of the allowed values.
    /// </summary>
    public static bool IsAllowed(string? status) => status is not null && Allowed.Contains(status);

    /// <summary>
    /// Returns true if the status allows modification of order header and lines.
    /// </summary>
    public static bool IsModifiable(string status) => !string.Equals(status, Cancelled, StringComparison.OrdinalIgnoreCase)
                                                      && !string.Equals(status, Received, StringComparison.OrdinalIgnoreCase);
}

