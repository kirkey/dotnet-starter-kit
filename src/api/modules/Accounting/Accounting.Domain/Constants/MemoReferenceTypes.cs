namespace Accounting.Domain.Constants;

/// <summary>
/// String-based enumeration of supported reference types for credit/debit memos.
/// </summary>
public static class MemoReferenceTypes
{
    /// <summary>
    /// Allowed, normalized reference type names.
    /// </summary>
    public static readonly string[] Allowed = new[]
    {
        "Customer",
        "Vendor",
        "Member"
    };

    /// <summary>
    /// Returns true if the provided type matches one of the allowed names (case-insensitive).
    /// </summary>
    public static bool Contains(string? referenceType) =>
        !string.IsNullOrWhiteSpace(referenceType)
        && Allowed.Contains(referenceType.Trim(), StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Returns a comma-separated list of allowed type names for messages.
    /// </summary>
    public static string AsDisplayList() => string.Join(", ", Allowed);
}

