namespace FSH.Starter.Blazor.Client.Pages.Accounting.FixedAssets;

/// <summary>
/// ViewModel used for creating or editing fixed assets.
/// </summary>
public sealed class FixedAssetViewModel
{
    public DefaultIdType? Id { get; set; }
    public string? AssetName { get; set; }
    public string? AssetType { get; set; }
    public DateTime? PurchaseDate { get; set; } = DateTime.Today;
    public decimal PurchasePrice { get; set; }
    public int ServiceLife { get; set; }
    public DefaultIdType? DepreciationMethodId { get; set; }
    public decimal SalvageValue { get; set; }
    public DefaultIdType? AccumulatedDepreciationAccountId { get; set; }
    public DefaultIdType? DepreciationExpenseAccountId { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public string? Department { get; set; }
    public string? Manufacturer { get; set; }
    public string? ModelNumber { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public bool IsDisposed { get; set; }
    public decimal CurrentBookValue { get; set; }
}

