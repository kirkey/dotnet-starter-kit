namespace Accounting.Application.FixedAssets.Update;

/// <summary>
/// Request to update an existing Fixed Asset.
/// </summary>
public class UpdateFixedAssetRequest(
    DefaultIdType id,
    string? assetName = null,
    DefaultIdType? depreciationMethodId = null,
    int? serviceLife = null,
    decimal? salvageValue = null,
    string? serialNumber = null,
    string? location = null,
    string? department = null,
    string? gpsCoordinates = null,
    string? substationName = null,
    string? regulatoryClassification = null,
    decimal? voltageRating = null,
    decimal? capacity = null,
    string? manufacturer = null,
    string? modelNumber = null,
    bool requiresUsoaReporting = false,
    string? description = null,
    string? notes = null)
    : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; } = id;
    public string? AssetName { get; set; } = assetName;
    public DefaultIdType? DepreciationMethodId { get; set; } = depreciationMethodId;
    public int? ServiceLife { get; set; } = serviceLife;
    public decimal? SalvageValue { get; set; } = salvageValue;
    public string? SerialNumber { get; set; } = serialNumber;
    public string? Location { get; set; } = location;
    public string? Department { get; set; } = department;
    public string? GpsCoordinates { get; set; } = gpsCoordinates;
    public string? SubstationName { get; set; } = substationName;
    public string? RegulatoryClassification { get; set; } = regulatoryClassification;
    public decimal? VoltageRating { get; set; } = voltageRating;
    public decimal? Capacity { get; set; } = capacity;
    public string? Manufacturer { get; set; } = manufacturer;
    public string? ModelNumber { get; set; } = modelNumber;
    public bool RequiresUsoaReporting { get; set; } = requiresUsoaReporting;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
