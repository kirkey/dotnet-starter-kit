using Accounting.Application.FixedAssets.Responses;

namespace Accounting.Application.FixedAssets.Get;

public class GetFixedAssetRequest(DefaultIdType id) : IRequest<FixedAssetResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
