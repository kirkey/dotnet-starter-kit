using MediatR;

namespace Accounting.Application.FixedAssets.Delete;

public class DeleteFixedAssetRequest(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}
