using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Deactivate.v1;

public sealed class DeactivateInvestmentProductHandler(
    [FromKeyedServices("microfinance:investmentproducts")] IRepository<InvestmentProduct> repository,
    ILogger<DeactivateInvestmentProductHandler> logger)
    : IRequestHandler<DeactivateInvestmentProductCommand, DeactivateInvestmentProductResponse>
{
    public async Task<DeactivateInvestmentProductResponse> Handle(
        DeactivateInvestmentProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await repository.FirstOrDefaultAsync(
            new InvestmentProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (product is null)
        {
            throw new NotFoundException($"Investment product with ID {request.Id} not found.");
        }

        product.Deactivate();

        await repository.UpdateAsync(product, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Investment product deactivated: {ProductId}", request.Id);

        return new DeactivateInvestmentProductResponse(product.Id);
    }
}
