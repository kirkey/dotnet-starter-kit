using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.RecordPremium.v1;

public sealed class RecordCollateralInsurancePremiumHandler(
    [FromKeyedServices("microfinance:collateralinsurances")] IRepository<CollateralInsurance> repository,
    ILogger<RecordCollateralInsurancePremiumHandler> logger)
    : IRequestHandler<RecordCollateralInsurancePremiumCommand, RecordCollateralInsurancePremiumResponse>
{
    public async Task<RecordCollateralInsurancePremiumResponse> Handle(
        RecordCollateralInsurancePremiumCommand request,
        CancellationToken cancellationToken)
    {
        var insurance = await repository.FirstOrDefaultAsync(
            new CollateralInsuranceByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral insurance {request.Id} not found");

        insurance.RecordPremiumPayment(request.PaymentDate, request.NextDueDate);
        await repository.UpdateAsync(insurance, cancellationToken);

        logger.LogInformation("Premium payment recorded for insurance: {InsuranceId}", insurance.Id);

        return new RecordCollateralInsurancePremiumResponse(insurance.Id, request.PaymentDate, request.NextDueDate);
    }
}
