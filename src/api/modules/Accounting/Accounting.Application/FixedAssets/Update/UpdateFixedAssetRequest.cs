namespace Accounting.Application.FixedAssets.Update;

/// <summary>
/// Command to update an existing Fixed Asset.
/// </summary>
public class UpdateFixedAssetCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// Asset identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Asset name.
    /// </summary>
    public string? AssetName { get; set; }
    
    /// <summary>
    /// Depreciation method identifier.
    /// </summary>
    public DefaultIdType? DepreciationMethodId { get; set; }
    
    /// <summary>
    /// Service life in years.
    /// </summary>
    public int? ServiceLife { get; set; }
    
    /// <summary>
    /// Salvage value.
    /// </summary>
    public decimal? SalvageValue { get; set; }
    
    /// <summary>
    /// Serial number.
    /// </summary>
    public string? SerialNumber { get; set; }
    
    /// <summary>
    /// Location.
    /// </summary>
    public string? Location { get; set; }
    
    /// <summary>
    /// Department.
    /// </summary>
    public string? Department { get; set; }
    
    /// <summary>
    /// GPS coordinates.
    /// </summary>
    public string? GpsCoordinates { get; set; }
    
    /// <summary>
    /// Substation name.
    /// </summary>
    public string? SubstationName { get; set; }
    
    /// <summary>
    /// Regulatory classification.
    /// </summary>
    public string? RegulatoryClassification { get; set; }
    
    /// <summary>
    /// Voltage rating.
    /// </summary>
    public decimal? VoltageRating { get; set; }
    
    /// <summary>
    /// Capacity.
    /// </summary>
    public decimal? Capacity { get; set; }
    
    /// <summary>
    /// Manufacturer.
    /// </summary>
    public string? Manufacturer { get; set; }
    
    /// <summary>
    /// Model number.
    /// </summary>
    public string? ModelNumber { get; set; }
    
    /// <summary>
    /// Requires USOA reporting.
    /// </summary>
    public bool RequiresUsoaReporting { get; set; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Notes.
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Image URL for visual documentation.
    /// </summary>
    public string? ImageUrl { get; set; }
}
