namespace Accounting.Application.FixedAssets.Responses;

/// <summary>
/// Response model representing a fixed asset entity.
/// Contains comprehensive asset information including purchase details, depreciation, and disposal status.
/// </summary>
public class FixedAssetResponse(
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
    string? description) : BaseDto
{
    /// <summary>
    /// Unique identifier for the fixed asset.
    /// </summary>
    public new DefaultIdType Id { get; set; }

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
    /// Description or additional details about the asset.
    /// </summary>
    public string? Description { get; set; } = description;
}
