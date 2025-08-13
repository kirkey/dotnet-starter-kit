using FSH.Framework.Core.Extensions.Dto;

namespace Accounting.Application.FixedAssets.Dtos;

public class FixedAssetDto(
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
    public DateTime PurchaseDate { get; set; } = purchaseDate;
    public decimal PurchasePrice { get; set; } = purchasePrice;
    public int ServiceLife { get; set; } = serviceLife;
    public DefaultIdType DepreciationMethodId { get; set; } = depreciationMethodId;
    public decimal SalvageValue { get; set; } = salvageValue;
    public decimal CurrentBookValue { get; set; } = currentBookValue;
    public DefaultIdType AccumulatedDepreciationAccountId { get; set; } = accumulatedDepreciationAccountId;
    public DefaultIdType DepreciationExpenseAccountId { get; set; } = depreciationExpenseAccountId;
    public string? SerialNumber { get; set; } = serialNumber;
    public string? Location { get; set; } = location;
    public string? Department { get; set; } = department;
    public bool IsDisposed { get; set; } = isDisposed;
    public DateTime? DisposalDate { get; set; } = disposalDate;
    public decimal? DisposalAmount { get; set; } = disposalAmount;
    public string? Description { get; set; } = description;
}

public class DepreciationEntryDto(
    DefaultIdType id,
    DefaultIdType assetId,
    DateTime depreciationDate,
    decimal depreciationAmount,
    string method,
    string? notes)
{
    public DefaultIdType Id { get; set; } = id;
    public DefaultIdType AssetId { get; set; } = assetId;
    public DateTime DepreciationDate { get; set; } = depreciationDate;
    public decimal DepreciationAmount { get; set; } = depreciationAmount;
    public string Method { get; set; } = method;
    public string? Notes { get; set; } = notes;
}
