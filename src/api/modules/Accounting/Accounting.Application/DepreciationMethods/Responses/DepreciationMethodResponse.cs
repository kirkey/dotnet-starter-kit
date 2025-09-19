namespace Accounting.Application.DepreciationMethods.Responses;

/// <summary>
/// Response model representing a depreciation method entity.
/// Contains depreciation calculation method information including code, formula, and status.
/// </summary>
public class DepreciationMethodResponse(
    string methodCode,
    string? formula,
    bool isActive) : BaseDto
{
    /// <summary>
    /// Unique code identifying the depreciation method (e.g., "SL" for Straight Line, "DB" for Double Declining Balance).
    /// </summary>
    public string MethodCode { get; set; } = methodCode;
    
    /// <summary>
    /// Mathematical formula or algorithm used for depreciation calculation.
    /// </summary>
    public string? Formula { get; set; } = formula;
    
    /// <summary>
    /// Indicates whether this depreciation method is currently active and available for use.
    /// </summary>
    public bool IsActive { get; set; } = isActive;
}
