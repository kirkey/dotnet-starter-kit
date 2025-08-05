using MediatR;
using Accounting.Application.FixedAssets.Dtos;

namespace Accounting.Application.FixedAssets.Get;

public record GetFixedAssetRequest(DefaultIdType Id) : IRequest<FixedAssetDto>;
