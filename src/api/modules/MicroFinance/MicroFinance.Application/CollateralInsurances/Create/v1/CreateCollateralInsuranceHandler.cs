using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Create.v1;

public sealed class CreateCollateralInsuranceHandler(
    [FromKeyedServices("microfinance:collateralinsurances")] IRepository<CollateralInsurance> repository,
    ILogger<CreateCollateralInsuranceHandler> logger)
    : IRequestHandler<CreateCollateralInsuranceCommand, CreateCollateralInsuranceResponse>
{
    public async Task<CreateCollateralInsuranceResponse> Handle(
        CreateCollateralInsuranceCommand request,
        CancellationToken cancellationToken)
    {
        var insurance = CollateralInsurance.Create(
            request.CollateralId,
            request.PolicyNumber,
            request.InsurerName,
            request.InsuranceType,
            request.CoverageAmount,
            request.PremiumAmount,
            request.Deductible,
            request.EffectiveDate,
            request.ExpiryDate,
            request.IsMfiAsBeneficiary);

        await repository.AddAsync(insurance, cancellationToken);

        logger.LogInformation("Collateral insurance created: {InsuranceId} for collateral {CollateralId}",
            insurance.Id, request.CollateralId);

        return new CreateCollateralInsuranceResponse(insurance.Id);
    }
}
