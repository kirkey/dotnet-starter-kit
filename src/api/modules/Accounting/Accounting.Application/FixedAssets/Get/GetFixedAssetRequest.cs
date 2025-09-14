using Accounting.Application.FixedAssets.Dtos;

namespace Accounting.Application.FixedAssets.Get;

public class GetFixedAssetRequest(DefaultIdType id) : IRequest<FixedAssetDto>
{
    public DefaultIdType Id { get; set; } = id;
}
