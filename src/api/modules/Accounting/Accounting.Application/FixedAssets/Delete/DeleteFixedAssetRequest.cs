using MediatR;

namespace Accounting.Application.FixedAssets.Delete;

public class DeleteFixedAssetRequest : IRequest
{
    public DefaultIdType Id { get; set; }

    public DeleteFixedAssetRequest(DefaultIdType id)
    {
        Id = id;
    }
}
