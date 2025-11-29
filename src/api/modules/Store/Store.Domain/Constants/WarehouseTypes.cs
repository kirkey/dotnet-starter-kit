namespace Store.Domain.Constants;

/// <summary>
/// String-based enumeration of supported warehouse types for classification and operational management.
/// Keep values human-readable; comparisons are case-insensitive.
/// </summary>
public static class WarehouseTypes
{
    /// <summary>
    /// Standard general-purpose warehouse for typical inventory storage.
    /// </summary>
    public const string Standard = "Standard";

    /// <summary>
    /// Retail store location with point-of-sale operations.
    /// </summary>
    public const string Store = "Store";

    /// <summary>
    /// Climate-controlled facility for temperature-sensitive goods (e.g., food, pharmaceuticals).
    /// </summary>
    public const string ColdStorage = "Cold Storage";

    /// <summary>
    /// Specialized facility for frozen goods requiring temperatures below 0°C.
    /// </summary>
    public const string Frozen = "Frozen";

    /// <summary>
    /// Hazardous materials storage facility with special safety and regulatory compliance.
    /// </summary>
    public const string Hazmat = "Hazmat";

    /// <summary>
    /// Specialized facility for storing pharmaceutical products with strict quality control.
    /// </summary>
    public const string Pharmaceutical = "Pharmaceutical";

    /// <summary>
    /// Specialized facility for automotive parts and vehicle storage.
    /// </summary>
    public const string Automotive = "Automotive";

    /// <summary>
    /// Allowed, normalized warehouse type names.
    /// Most common types are listed first for UI prioritization.
    /// </summary>
    public static readonly string[] Allowed = new[]
    {
        Standard,
        Store,
        ColdStorage,
        Frozen,
        Hazmat,
        Pharmaceutical,
        Automotive
    };

    /// <summary>
    /// Returns true if the provided type matches one of the allowed names (case-insensitive).
    /// </summary>
    public static bool Contains(string? warehouseType) =>
        !string.IsNullOrWhiteSpace(warehouseType)
        && Allowed.Contains(warehouseType.Trim(), StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Returns a comma-separated list of allowed type names for display in messages.
    /// </summary>
    public static string AsDisplayList() => string.Join(", ", Allowed);
}

