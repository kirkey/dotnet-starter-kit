namespace Accounting.Application.FixedAssets.Dtos;

public class FixedAssetDto
{
    public DefaultIdType Id { get; set; }
    public string AssetName { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public int ServiceLife { get; set; }
    public DefaultIdType DepreciationMethodId { get; set; }
    public decimal SalvageValue { get; set; }
    public decimal CurrentBookValue { get; set; }
    public DefaultIdType AccumulatedDepreciationAccountId { get; set; }
    public DefaultIdType DepreciationExpenseAccountId { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public string? Department { get; set; }
    public bool IsDisposed { get; set; }
    public DateTime? DisposalDate { get; set; }
    public decimal? DisposalAmount { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public FixedAssetDto(
        DefaultIdType id,
        string assetName,
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
        string? description,
        string? notes)
    {
        Id = id;
        AssetName = assetName;
        PurchaseDate = purchaseDate;
        PurchasePrice = purchasePrice;
        ServiceLife = serviceLife;
        DepreciationMethodId = depreciationMethodId;
        SalvageValue = salvageValue;
        CurrentBookValue = currentBookValue;
        AccumulatedDepreciationAccountId = accumulatedDepreciationAccountId;
        DepreciationExpenseAccountId = depreciationExpenseAccountId;
        SerialNumber = serialNumber;
        Location = location;
        Department = department;
        IsDisposed = isDisposed;
        DisposalDate = disposalDate;
        DisposalAmount = disposalAmount;
        Description = description;
        Notes = notes;
    }
}

public class DepreciationEntryDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType AssetId { get; set; }
    public DateTime DepreciationDate { get; set; }
    public decimal DepreciationAmount { get; set; }
    public string Method { get; set; } = null!;
    public string? Notes { get; set; }

    public DepreciationEntryDto(
        DefaultIdType id,
        DefaultIdType assetId,
        DateTime depreciationDate,
        decimal depreciationAmount,
        string method,
        string? notes)
    {
        Id = id;
        AssetId = assetId;
        DepreciationDate = depreciationDate;
        DepreciationAmount = depreciationAmount;
        Method = method;
        Notes = notes;
    }
}
