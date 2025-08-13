using MediatR;

namespace Accounting.Application.FixedAssets.Create;

public class CreateFixedAssetRequest(
    string assetName,
    DateTime purchaseDate,
    decimal purchasePrice,
    DefaultIdType depreciationMethodId,
    int serviceLife,
    decimal salvageValue,
    DefaultIdType accumulatedDepreciationAccountId,
    DefaultIdType depreciationExpenseAccountId,
    string? serialNumber = null,
    string? location = null,
    string? department = null,
    string? description = null,
    string? notes = null) : IRequest<DefaultIdType>
{
    public string AssetName { get; set; } = assetName;
    public DateTime PurchaseDate { get; set; } = purchaseDate;
    public decimal PurchasePrice { get; set; } = purchasePrice;
    public DefaultIdType DepreciationMethodId { get; set; } = depreciationMethodId;
    public int ServiceLife { get; set; } = serviceLife;
    public decimal SalvageValue { get; set; } = salvageValue;
    public DefaultIdType AccumulatedDepreciationAccountId { get; set; } = accumulatedDepreciationAccountId;
    public DefaultIdType DepreciationExpenseAccountId { get; set; } = depreciationExpenseAccountId;
    public string? SerialNumber { get; set; } = serialNumber;
    public string? Location { get; set; } = location;
    public string? Department { get; set; } = department;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
