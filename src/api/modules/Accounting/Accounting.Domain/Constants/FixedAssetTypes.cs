namespace Accounting.Domain.Constants;

/// <summary>
/// String-based enumeration of supported fixed asset categories for classification and reporting.
/// Keep values human-readable; comparisons are case-insensitive.
/// </summary>
public static class FixedAssetTypes
{
    /// <summary>
    /// Allowed, normalized asset type names.
    /// </summary>
    public static readonly string[] Allowed = new[]
    {
        "Transformer",
        "Transmission Line",
        "Distribution Line",
        "Power Plant",
        "Generator",
        "Substation",
        "Meter",
        "Vehicle",
        "Building",
        "Equipment",
        "Software",
        "Land",
        "IT Equipment",
        "Furniture"
    };

    /// <summary>
    /// Returns true if the provided type matches one of the allowed names (case-insensitive).
    /// </summary>
    public static bool Contains(string? assetType) =>
        !string.IsNullOrWhiteSpace(assetType)
        && Allowed.Contains(assetType.Trim(), StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Returns a comma-separated list of allowed type names for display in messages.
    /// </summary>
    public static string AsDisplayList() => string.Join(", ", Allowed);
}
