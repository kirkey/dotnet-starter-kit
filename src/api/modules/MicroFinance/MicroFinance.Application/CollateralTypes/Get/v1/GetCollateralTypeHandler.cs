using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Get.v1;

/// <summary>
/// Handler for getting a collateral type by ID.
/// </summary>
public sealed class GetCollateralTypeHandler(
    [FromKeyedServices("microfinance:collateraltypes")] IReadRepository<CollateralType> repository,
    ILogger<GetCollateralTypeHandler> logger)
    : IRequestHandler<GetCollateralTypeRequest, CollateralTypeResponse>
{
    public async Task<CollateralTypeResponse> Handle(GetCollateralTypeRequest request, CancellationToken cancellationToken)
    {
        var spec = new CollateralTypeByIdSpec(request.Id);
        var collateralType = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (collateralType is null)
        {
            throw new NotFoundException($"Collateral type with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved collateral type {CollateralTypeId}", request.Id);

        return collateralType;
    }
}
