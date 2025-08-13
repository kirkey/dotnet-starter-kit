using MediatR;

namespace Accounting.Application.FixedAssets.Update;

public class UpdateFixedAssetRequest(
    DefaultIdType id,
    string? assetName = null,
    DefaultIdType? depreciationMethodId = null,
    int? serviceLife = null,
    decimal? salvageValue = null,
    string? serialNumber = null,
    string? location = null,
    string? department = null,
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
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
