using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Activate.v1;

public sealed class ActivateInsuranceProductHandler(
    ILogger<ActivateInsuranceProductHandler> logger,
    [FromKeyedServices("microfinance:insuranceproducts")] IRepository<InsuranceProduct> repository)
    : IRequestHandler<ActivateInsuranceProductCommand, ActivateInsuranceProductResponse>
{
    public async Task<ActivateInsuranceProductResponse> Handle(ActivateInsuranceProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await repository.FirstOrDefaultAsync(
            new InsuranceProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (product is null)
            throw new NotFoundException($"Insurance product with ID {request.Id} not found.");

        product.Activate();

        await repository.UpdateAsync(product, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Insurance product activated: {ProductId}", product.Id);

        return new ActivateInsuranceProductResponse(product.Id, product.Status);
    }
}
