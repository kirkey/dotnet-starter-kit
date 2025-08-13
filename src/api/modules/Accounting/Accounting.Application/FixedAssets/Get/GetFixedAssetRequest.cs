using MediatR;
using Accounting.Application.FixedAssets.Dtos;

namespace Accounting.Application.FixedAssets.Get;

public class GetFixedAssetRequest : IRequest<FixedAssetDto>
{
    public DefaultIdType Id { get; set; }

    public GetFixedAssetRequest(DefaultIdType id)
    {
        Id = id;
    }
}
