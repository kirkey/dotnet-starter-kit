namespace FSH.Starter.Blazor.Client.Constants;

/// <summary>
/// Client-side constants for warehouse types used in the UI.
/// These values must match the backend validation in Store.Domain.Constants.WarehouseTypes.
/// </summary>
public static class WarehouseTypeConstants
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
    /// Array of all supported warehouse types.
    /// Most common types are listed first for UI prioritization.
    /// </summary>
    public static readonly string[] All = new[]
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
    /// Read-only list of all supported warehouse types for easy enumeration.
    /// </summary>
    public static IReadOnlyList<string> WarehouseTypes => All;
}

