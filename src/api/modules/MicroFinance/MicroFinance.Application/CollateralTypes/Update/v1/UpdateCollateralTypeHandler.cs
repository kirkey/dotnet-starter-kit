using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Update.v1;

/// <summary>
/// Handler for updating a collateral type.
/// </summary>
public sealed class UpdateCollateralTypeHandler(
    ILogger<UpdateCollateralTypeHandler> logger,
    [FromKeyedServices("microfinance:collateraltypes")] IRepository<CollateralType> repository)
    : IRequestHandler<UpdateCollateralTypeCommand, UpdateCollateralTypeResponse>
{
    public async Task<UpdateCollateralTypeResponse> Handle(UpdateCollateralTypeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collateralType = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (collateralType is null)
        {
            throw new NotFoundException($"Collateral type with ID {request.Id} not found.");
        }

        collateralType.Update(
            name: request.Name,
            description: request.Description,
            defaultLtvPercent: request.DefaultLtvPercent,
            maxLtvPercent: request.MaxLtvPercent,
            defaultUsefulLifeYears: request.DefaultUsefulLifeYears,
            annualDepreciationRate: request.AnnualDepreciationRate,
            requiresInsurance: request.RequiresInsurance,
            requiresAppraisal: request.RequiresAppraisal,
            revaluationFrequencyMonths: request.RevaluationFrequencyMonths,
            requiresRegistration: request.RequiresRegistration,
            requiredDocuments: request.RequiredDocuments,
            notes: request.Notes,
            displayOrder: request.DisplayOrder);

        await repository.UpdateAsync(collateralType, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Collateral type updated with ID: {Id}, Name: {Name}", collateralType.Id, collateralType.Name);

        return new UpdateCollateralTypeResponse(collateralType.Id);
    }
}
