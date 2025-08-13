using MediatR;

namespace Accounting.Application.FixedAssets.Update;

public class UpdateFixedAssetRequest : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string? AssetName { get; set; }
    public DefaultIdType? DepreciationMethodId { get; set; }
    public int? ServiceLife { get; set; }
    public decimal? SalvageValue { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public string? Department { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public UpdateFixedAssetRequest(
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
    {
        Id = id;
        AssetName = assetName;
        DepreciationMethodId = depreciationMethodId;
        ServiceLife = serviceLife;
        SalvageValue = salvageValue;
        SerialNumber = serialNumber;
        Location = location;
        Department = department;
        Description = description;
        Notes = notes;
    }
}
