using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Get.v1;

public sealed class GetInsuranceProductHandler(
    [FromKeyedServices("microfinance:insuranceproducts")] IReadRepository<InsuranceProduct> repository)
    : IRequestHandler<GetInsuranceProductRequest, InsuranceProductResponse>
{
    public async Task<InsuranceProductResponse> Handle(GetInsuranceProductRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await repository.FirstOrDefaultAsync(
            new InsuranceProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (product is null)
            throw new NotFoundException($"Insurance product with ID {request.Id} not found.");

        return new InsuranceProductResponse(
            product.Id,
            product.Code,
            product.Name,
            product.InsuranceType,
            product.Provider,
            product.MinCoverage,
            product.MaxCoverage,
            product.PremiumCalculation,
            product.PremiumRate,
            product.MinAge,
            product.MaxAge,
            product.WaitingPeriodDays,
            product.PremiumUpfront,
            product.MandatoryWithLoan,
            product.Status,
            product.Description,
            product.CoveredEvents,
            product.Exclusions,
            product.TermsConditions,
            product.Notes);
    }
}
