using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Get.v1;

public sealed class GetCollateralInsuranceHandler(
    [FromKeyedServices("microfinance:collateralinsurances")] IReadRepository<CollateralInsurance> repository)
    : IRequestHandler<GetCollateralInsuranceRequest, CollateralInsuranceResponse>
{
    public async Task<CollateralInsuranceResponse> Handle(
        GetCollateralInsuranceRequest request,
        CancellationToken cancellationToken)
    {
        var insurance = await repository.FirstOrDefaultAsync(
            new CollateralInsuranceByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral insurance {request.Id} not found");

        return new CollateralInsuranceResponse(
            insurance.Id,
            insurance.CollateralId,
            insurance.PolicyNumber,
            insurance.InsurerName,
            insurance.InsuranceType,
            insurance.Status,
            insurance.CoverageAmount,
            insurance.PremiumAmount,
            insurance.Deductible,
            insurance.EffectiveDate,
            insurance.ExpiryDate,
            insurance.RenewalDate,
            insurance.InsurerContact,
            insurance.InsurerPhone,
            insurance.InsurerEmail,
            insurance.IsMfiAsBeneficiary,
            insurance.BeneficiaryName,
            insurance.PolicyDocumentPath,
            insurance.LastPremiumPaidDate,
            insurance.NextPremiumDueDate,
            insurance.Notes,
            insurance.RenewalReminderDays,
            insurance.AutoRenewal);
    }
}
