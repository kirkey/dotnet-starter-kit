using MediatR;

namespace Accounting.Application.FixedAssets.Delete;

public record DeleteFixedAssetRequest(DefaultIdType Id) : IRequest;
