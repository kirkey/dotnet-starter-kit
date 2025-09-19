namespace Accounting.Application.FixedAssets.Responses;

/// <summary>
/// Response model representing a depreciation entry for a fixed asset.
/// Contains depreciation calculation details and timing information.
/// </summary>
public class DepreciationEntryResponse(
    DefaultIdType id,
    DefaultIdType assetId,
    DateTime depreciationDate,
    decimal depreciationAmount,
    string method,
    string? notes)
{
    /// <summary>
    /// Unique identifier for the depreciation entry.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;
    
    /// <summary>
    /// Reference to the fixed asset being depreciated.
    /// </summary>
    public DefaultIdType AssetId { get; set; } = assetId;
    
    /// <summary>
    /// Date when this depreciation was calculated/recorded.
    /// </summary>
    public DateTime DepreciationDate { get; set; } = depreciationDate;
    
    /// <summary>
    /// Amount of depreciation for this period.
    /// </summary>
    public decimal DepreciationAmount { get; set; } = depreciationAmount;
    
    /// <summary>
    /// Depreciation method used for this calculation.
    /// </summary>
    public string Method { get; set; } = method;
    
    /// <summary>
    /// Optional notes or comments about this depreciation entry.
    /// </summary>
    public string? Notes { get; set; } = notes;
}
