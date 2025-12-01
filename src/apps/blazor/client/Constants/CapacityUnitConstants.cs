namespace FSH.Starter.Blazor.Client.Constants;

/// <summary>
/// Client-side constants for warehouse and location capacity units used in the UI.
/// These values represent standardized measurements for warehouse storage capacity.
/// </summary>
public static class CapacityUnitConstants
{
    /// <summary>
    /// Square feet - standard measurement for warehouse floor space.
    /// </summary>
    public const string SquareFeet = "sqft";

    /// <summary>
    /// Square meters - metric measurement for warehouse floor space.
    /// </summary>
    public const string SquareMeters = "sqm";

    /// <summary>
    /// Cubic feet - volume measurement for 3D storage capacity.
    /// </summary>
    public const string CubicFeet = "cuft";

    /// <summary>
    /// Cubic meters - metric volume measurement for 3D storage capacity.
    /// </summary>
    public const string CubicMeters = "cum";

    /// <summary>
    /// Pallets - standard unit for counting warehouse pallet positions.
    /// </summary>
    public const string Pallets = "pallets";

    /// <summary>
    /// Units - generic count for inventory units.
    /// </summary>
    public const string Units = "units";

    /// <summary>
    /// Kilograms - weight measurement for storage capacity.
    /// </summary>
    public const string Kilograms = "kg";

    /// <summary>
    /// Pounds - weight measurement for storage capacity.
    /// </summary>
    public const string Pounds = "lbs";

    /// <summary>
    /// Tons - large weight measurement for storage capacity (metric tons).
    /// </summary>
    public const string Tons = "tons";

    /// <summary>
    /// Array of all supported capacity units, ordered by most common usage.
    /// </summary>
    public static readonly string[] All = new[]
    {
        SquareFeet,
        SquareMeters,
        CubicFeet,
        CubicMeters,
        Pallets,
        Units,
        Kilograms,
        Pounds,
        Tons
    };

    /// <summary>
    /// Dictionary mapping capacity unit codes to display names.
    /// </summary>
    public static readonly Dictionary<string, string> DisplayNames = new()
    {
        { SquareFeet, "Square Feet (sqft)" },
        { SquareMeters, "Square Meters (sqm)" },
        { CubicFeet, "Cubic Feet (cuft)" },
        { CubicMeters, "Cubic Meters (cum)" },
        { Pallets, "Pallets" },
        { Units, "Units" },
        { Kilograms, "Kilograms (kg)" },
        { Pounds, "Pounds (lbs)" },
        { Tons, "Metric Tons (tons)" }
    };

    /// <summary>
    /// Gets the display name for a capacity unit code.
    /// </summary>
    /// <param name="capacityUnit">The capacity unit code (e.g., "sqft").</param>
    /// <returns>The display name (e.g., "Square Feet (sqft)"), or the original code if not found.</returns>
    public static string GetDisplayName(string? capacityUnit)
    {
        if (string.IsNullOrWhiteSpace(capacityUnit))
        {
            return "Unknown";
        }

        return DisplayNames.TryGetValue(capacityUnit, out var displayName) 
            ? displayName 
            : capacityUnit;
    }
}

