namespace Accounting.Application.FixedAssets.Responses;

/// <summary>
/// Response model representing a fixed asset entity.
/// Contains comprehensive asset information including purchase details, depreciation, and disposal status.
/// </summary>
public class FixedAssetResponse(
    string assetName,
    string assetType,
    DateTime purchaseDate,
    decimal purchasePrice,
    int serviceLife,
    DefaultIdType depreciationMethodId,
    decimal salvageValue,
    decimal currentBookValue,
    DefaultIdType accumulatedDepreciationAccountId,
    DefaultIdType depreciationExpenseAccountId,
    string? serialNumber,
    string? location,
    string? department,
    bool isDisposed,
    DateTime? disposalDate,
    decimal? disposalAmount,
    string? gpsCoordinates,
    string? substationName,
    DefaultIdType? assetUsoaId,
    string? regulatoryClassification,
    decimal? voltageRating,
    decimal? capacity,
    string? manufacturer,
    string? modelNumber,
    DateTime? lastMaintenanceDate,
    DateTime? nextMaintenanceDate,
    bool requiresUsoaReporting,
    string? description,
    string? notes) : BaseDto
{
    /// <summary>
    /// Unique identifier for the fixed asset.
    /// </summary>
    public new DefaultIdType Id { get; set; }

    /// <summary>
    /// Human-readable name of the asset.
    /// </summary>
    public string AssetName { get; set; } = assetName;

    /// <summary>
    /// Asset category such as Transformer, Transmission Line, Power Plant, Vehicle.
    /// </summary>
    public string AssetType { get; set; } = assetType;

    /// <summary>
    /// Date when the asset was purchased.
    /// </summary>
    public DateTime PurchaseDate { get; set; } = purchaseDate;

    /// <summary>
    /// Original purchase price of the asset.
    /// </summary>
    public decimal PurchasePrice { get; set; } = purchasePrice;

    /// <summary>
    /// Expected service life of the asset in years.
    /// </summary>
    public int ServiceLife { get; set; } = serviceLife;

    /// <summary>
    /// Depreciation method identifier used for this asset.
    /// </summary>
    public DefaultIdType DepreciationMethodId { get; set; } = depreciationMethodId;

    /// <summary>
    /// Expected salvage value at end of service life.
    /// </summary>
    public decimal SalvageValue { get; set; } = salvageValue;

    /// <summary>
    /// Current book value after depreciation.
    /// </summary>
    public decimal CurrentBookValue { get; set; } = currentBookValue;

    /// <summary>
    /// Account identifier for accumulated depreciation.
    /// </summary>
    public DefaultIdType AccumulatedDepreciationAccountId { get; set; } = accumulatedDepreciationAccountId;

    /// <summary>
    /// Account identifier for depreciation expense.
    /// </summary>
    public DefaultIdType DepreciationExpenseAccountId { get; set; } = depreciationExpenseAccountId;

    /// <summary>
    /// Serial number of the asset for identification.
    /// </summary>
    public string? SerialNumber { get; set; } = serialNumber;

    /// <summary>
    /// Physical location where the asset is situated.
    /// </summary>
    public string? Location { get; set; } = location;

    /// <summary>
    /// Department or division responsible for the asset.
    /// </summary>
    public string? Department { get; set; } = department;

    /// <summary>
    /// Indicates whether the asset has been disposed of.
    /// </summary>
    public bool IsDisposed { get; set; } = isDisposed;

    /// <summary>
    /// Date when the asset was disposed (if applicable).
    /// </summary>
    public DateTime? DisposalDate { get; set; } = disposalDate;

    /// <summary>
    /// Amount received from disposal (if applicable).
    /// </summary>
    public decimal? DisposalAmount { get; set; } = disposalAmount;

    /// <summary>
    /// Geographic coordinates in "lat,lng" format.
    /// </summary>
    public string? GpsCoordinates { get; set; } = gpsCoordinates;

    /// <summary>
    /// Substation name where the asset is located.
    /// </summary>
    public string? SubstationName { get; set; } = substationName;

    /// <summary>
    /// Link to USOA account classification for the asset.
    /// </summary>
    public DefaultIdType? AssetUsoaId { get; set; } = assetUsoaId;

    /// <summary>
    /// Regulatory classification such as FERC category text.
    /// </summary>
    public string? RegulatoryClassification { get; set; } = regulatoryClassification;

    /// <summary>
    /// Voltage rating for electrical equipment.
    /// </summary>
    public decimal? VoltageRating { get; set; } = voltageRating;

    /// <summary>
    /// Capacity (e.g., MW for generators, MVA for transformers).
    /// </summary>
    public decimal? Capacity { get; set; } = capacity;

    /// <summary>
    /// Manufacturer name.
    /// </summary>
    public string? Manufacturer { get; set; } = manufacturer;

    /// <summary>
    /// Manufacturer model number.
    /// </summary>
    public string? ModelNumber { get; set; } = modelNumber;

    /// <summary>
    /// Date of the last maintenance performed.
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; } = lastMaintenanceDate;

    /// <summary>
    /// Scheduled date for the next maintenance activity.
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; } = nextMaintenanceDate;

    /// <summary>
    /// Whether this asset requires USOA reporting in regulatory filings.
    /// </summary>
    public bool RequiresUsoaReporting { get; set; } = requiresUsoaReporting;
}
