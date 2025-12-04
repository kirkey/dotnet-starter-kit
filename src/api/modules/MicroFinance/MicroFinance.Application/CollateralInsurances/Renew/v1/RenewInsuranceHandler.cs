using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Renew.v1;

public sealed class RenewInsuranceHandler(
    [FromKeyedServices("microfinance:collateralinsurances")] IRepository<CollateralInsurance> repository,
    ILogger<RenewInsuranceHandler> logger)
    : IRequestHandler<RenewInsuranceCommand, RenewInsuranceResponse>
{
    public async Task<RenewInsuranceResponse> Handle(
        RenewInsuranceCommand request,
        CancellationToken cancellationToken)
    {
        var insurance = await repository.FirstOrDefaultAsync(
            new CollateralInsuranceByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral insurance {request.Id} not found");

        insurance.Renew(request.NewExpiryDate, request.NewPremium);
        await repository.UpdateAsync(insurance, cancellationToken);

        logger.LogInformation("Insurance renewed: {InsuranceId}", insurance.Id);

        return new RenewInsuranceResponse(insurance.Id, request.NewExpiryDate, insurance.Status);
    }
}
