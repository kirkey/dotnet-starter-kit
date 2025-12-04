using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Create.v1;

/// <summary>
/// Handler for creating a new collateral type.
/// </summary>
public sealed class CreateCollateralTypeHandler(
    [FromKeyedServices("microfinance:collateraltypes")] IRepository<CollateralType> repository,
    ILogger<CreateCollateralTypeHandler> logger)
    : IRequestHandler<CreateCollateralTypeCommand, CreateCollateralTypeResponse>
{
    public async Task<CreateCollateralTypeResponse> Handle(CreateCollateralTypeCommand request, CancellationToken cancellationToken)
    {
        var collateralType = CollateralType.Create(
            request.Name,
            request.Code,
            request.Category,
            request.DefaultLtvPercent,
            request.MaxLtvPercent,
            request.DefaultUsefulLifeYears,
            request.AnnualDepreciationRate,
            request.Description);

        await repository.AddAsync(collateralType, cancellationToken);

        logger.LogInformation("Collateral type {Code} created - Name: {Name}, Category: {Category}",
            request.Code, request.Name, request.Category);

        return new CreateCollateralTypeResponse(
            collateralType.Id,
            collateralType.Name,
            collateralType.Code,
            collateralType.Category,
            collateralType.Status);
    }
}
