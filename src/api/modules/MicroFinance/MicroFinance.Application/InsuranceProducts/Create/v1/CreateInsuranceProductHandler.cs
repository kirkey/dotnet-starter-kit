using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Create.v1;

public sealed class CreateInsuranceProductHandler(
    ILogger<CreateInsuranceProductHandler> logger,
    [FromKeyedServices("microfinance:insuranceproducts")] IRepository<InsuranceProduct> repository)
    : IRequestHandler<CreateInsuranceProductCommand, CreateInsuranceProductResponse>
{
    public async Task<CreateInsuranceProductResponse> Handle(CreateInsuranceProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = InsuranceProduct.Create(
            code: request.Code,
            name: request.Name,
            insuranceType: request.InsuranceType,
            minCoverage: request.MinCoverage,
            maxCoverage: request.MaxCoverage,
            premiumCalculation: request.PremiumCalculation,
            premiumRate: request.PremiumRate,
            description: request.Description,
            provider: request.Provider,
            waitingPeriodDays: request.WaitingPeriodDays,
            premiumUpfront: request.PremiumUpfront,
            mandatoryWithLoan: request.MandatoryWithLoan,
            minAge: request.MinAge,
            maxAge: request.MaxAge);

        await repository.AddAsync(product, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Insurance product created: {Code} - {Name}", product.Code, product.Name);

        return new CreateInsuranceProductResponse(product.Id, product.Code, product.Name);
    }
}
